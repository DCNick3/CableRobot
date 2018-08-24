using System;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CableRobot
{
    public class RobotContoller : IDisposable
    {
        private static readonly  TimeSpan UpdateInterval = TimeSpan.FromMilliseconds(4);
        private static readonly double DesiredRate = 1.0 / UpdateInterval.TotalSeconds;
        
        private readonly Block<Angles>[] _trajectory;
        private readonly object _counterLock = new object();
        private int _commandCounter;
        private int _blockCounter;
        private int _blockLocalCounter = 0;
        private readonly AngleSender _angleSender;
        private Thread _workerThread;
        private CancellationToken _cancellationToken;
        private CancellationTokenSource _cancellationTokenSource;
        private readonly ManualResetEvent _pauseEvent;
        private RobotControllerState _state;

        public RobotContoller(Block<Vector4>[] cableLengths, IPEndPoint remoteEndPoint)
        {
            _trajectory = InverseKinematicsComputer.ComputeAnglesFromLengths(cableLengths).ToArray();
            _commandCounter = 0;
            _blockCounter = 0;
            CommandCount = _trajectory.Select(_ => _.Elements.Length).Sum();
            _angleSender = new AngleSender(remoteEndPoint, UpdateInterval);
            _angleSender.ProgressUpdated += AngleSenderOnProgressUpdated;
            
            _pauseEvent = new ManualResetEvent(true);

            File.WriteAllText("angles.txt", ExportAngles());
        }

        public string ExportAngles()
        {
            var sb = new StringBuilder();
            int c = 0;
            foreach (var block in _trajectory)
            foreach (var v in block.Elements)
            {
                c++;
                if (c % 64 == 0)
                sb.AppendLine(string.Join(" ",
                                  v.Thetas.Select(_ => (_ / -180.0 * Math.PI).ToString(CultureInfo.InvariantCulture))) +
                              ";");
            }

            return sb.ToString();
        }

        private void AngleSenderOnProgressUpdated(object sender, (int current, int total) e)
        {
            // ReSharper disable once MethodSupportsCancellation
            Task.Run(() =>
            {
                UpdateProgress(BlocksExecuted, CommandsExecuted - _blockLocalCounter + e.current);
                _blockLocalCounter = e.current;
                if (Math.Abs(Rate - DesiredRate) > 40)
                    OnLog($"WARNING! RealRate({_angleSender.Rate}) is wrong!");
                OnValuesUpdated();
            });
        }

        public event EventHandler<LogEventArgs> Log;
        public event EventHandler ValuesUpdated;
        public event EventHandler<RobotControllerState> StateUpdated;

        public RobotControllerState State
        {
            get => _state;
            private set
            {
                OnLog($"State {_state} -> {value}");
                _state = value;
                OnStateUpdated(value);
            }
        }

        public int CommandCount { get; }
        public int CommandsLeft => CommandCount - CommandsExecuted;
        public int CommandsExecuted
        {
            get
            {
                lock (_counterLock)
                    return _commandCounter;
            }
        }
        
        public int BlockCount => _trajectory.Length;
        public int BlocksLeft => BlockCount - BlocksExecuted;
        public int BlocksExecuted
        {
            get
            {
                lock (_counterLock)
                    return _blockCounter;
            }
        }
        public double Rate => _angleSender.Rate;
        
        public TimeSpan TotalTime => TimeSpan.FromTicks(UpdateInterval.Ticks * CommandCount);
        public TimeSpan TimeLeft => TimeSpan.FromTicks(UpdateInterval.Ticks * CommandsLeft);

        public string CurrentBlockName
        {
            get
            {
                var ex = BlocksExecuted;
                if (ex >= _trajectory.Length)
                    return "None";
                return _trajectory[ex].Name;
            }
        }

        public void Dispose()
        {
            _angleSender?.Dispose();
        }

        public (int, int) GetCommandId()
        {
            lock (_counterLock)
            {
                return (_blockCounter, _blockLocalCounter);
            }
        }

        public Task SetStateAsync(RobotControllerState state)
        {
            return Task.Run(() =>
            {
                if (state == State)
                    return;
                if (State == RobotControllerState.Idle)
                {
                    if (state != RobotControllerState.Working)
                        throw new InvalidOperationException();
                    UpdateProgress(0, 0);
                    State = state;
                    StartWorker();
                    return;
                }

                if (State == RobotControllerState.Working)
                {
                    if (state == RobotControllerState.Idle)
                    {
                        StopWorker();
                        State = state;
                        return;
                    }

                    if (state == RobotControllerState.Suspended)
                    {
                        SuspendWorker();
                        State = state;
                        return;
                    }
                }

                if (State == RobotControllerState.Suspended)
                {
                    if (state == RobotControllerState.Idle)
                    {
                        SetStateAsync(RobotControllerState.Working);
                        SetStateAsync(RobotControllerState.Idle);
                        return;
                    }
                    else if (state == RobotControllerState.Working)
                    {
                        ResumeWorker();
                        State = state;
                        return;
                    }
                }
                throw new InvalidOperationException();
            });
        }

        public void EmergencyStop() => SetStateAsync(RobotControllerState.Idle).ContinueWith(task => _angleSender.SetStopperState(true));


        private void UpdateProgress(int blockCounter, int commandCounter)
        {
            lock (_counterLock)
            {
                _blockCounter = blockCounter;
                _commandCounter = commandCounter;
            }
        }
        
        private void StartWorker()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _cancellationToken = _cancellationTokenSource.Token;
            _workerThread = new Thread(Worker);
            _workerThread.Start();
        }

        private void StopWorker()
        {
            _cancellationTokenSource.Cancel();
            _workerThread.Join();
        }
        
        private void ResumeWorker()
        {
            _pauseEvent.Set();
        }

        private void SuspendWorker()
        {
            _pauseEvent.Reset();
        }

        private void Worker()
        {
            try
            {
                _angleSender.SetStopperState(false);
                Thread.Sleep(100);
                while (_blockCounter < BlockCount)
                {
                    var block = _trajectory[_blockCounter];

                    //OnLog($"Executing {block.Name}");

                    _blockLocalCounter = 0;
                    var nextProgress = CommandsExecuted + block.Elements.Length;

                    _angleSender.SendBlock(block, _cancellationToken, _pauseEvent);

                    UpdateProgress(BlocksExecuted + 1, nextProgress);
                }
            }
            catch (OperationCanceledException)
            {
            }

            State = RobotControllerState.Idle;
        }

        private void OnLog(string message)
        {
            Task.Run(() =>
                Log?.Invoke(this, new LogEventArgs()
                    {Message = message, Time = DateTime.Now}
                ));
        }

        private void OnValuesUpdated()
        {
            ValuesUpdated?.Invoke(this, EventArgs.Empty);
        }

        private void OnStateUpdated(RobotControllerState e)
        {
            StateUpdated?.Invoke(this, e);
        }
    }

    public enum RobotControllerState
    {
        Idle = 0,
        Working = 1,
        Suspended = 2,
    }
}