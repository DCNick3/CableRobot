using System;
using System.Collections.Generic;
using System.Data;
using System.Net;
using System.Runtime.Remoting.Messaging;
using System.Threading;
using System.Threading.Tasks;
using CableRobot.Fins;

namespace CableRobot
{
    public class AngleSender : IDisposable
    {
        private const ushort AnglesAddress =  800;
        private const ushort StopperAddress = 850;
        
        private readonly FinsClient _client;
        private readonly Rate _rate;

        private int _counter = 0;
        
        public AngleSender(IPEndPoint robotEndPoint, TimeSpan updateInterval)
        {
            _client = new FinsClient(robotEndPoint);
            _rate = new Rate(updateInterval);
        }

        public event EventHandler<(int current, int total)> ProgressUpdated;
        
        public double Rate => _rate.RealRate;
        
        public void SendBlock(Block<Angles> block)
            => SendBlock(block, CancellationToken.None, new ManualResetEvent(true));

        /// <summary>
        /// Sends angles block to robot with set up rate
        /// Blocks until all angles will be sent
        /// </summary>
        /// <param name="block">Block of angles to send</param>
        public void SendBlock(Block<Angles> block, CancellationToken cancellationToken,
            WaitHandle continueHandle)
        {
            var count = block.Elements.Length;
            var array = block.Elements;
            for (int i = 0; i < count; i++)
            {
                if (_counter % 64 == 0)
                    OnProgressUpdated(i, block.Elements.Length);
                _counter++;
                SendAngles(array[i], cancellationToken, continueHandle);
            }
        }

        public Task SendBlockAsync(Block<Angles> block, CancellationToken cancellationToken, WaitHandle continueHandle) 
            => Task.Run(() => SendBlock(block, cancellationToken, continueHandle), cancellationToken);

        public void Dispose()
        {
            _client?.Dispose();
        }
        
        public void SetStopperState(bool state)
        {
            _client.WriteDataNoResponse(StopperAddress, new[] { (ushort)(state ? 0xFFFF : 0x0000) });
        }

        private static ushort[] ToShorts(byte[] data)
        {
            var r =  new ushort[data.Length / 2];
            for (int i = 0; i < r.Length; i++)
            {
                r[i] = data[i * 2];
                r[i] = (ushort)(r[i] | data[i * 2 + 1] << 8);
            }
            return r;
        }
        
        public static ushort[] SerializeAngle(double angle)
        {
            return ToShorts(BitConverter.GetBytes(angle));
        }

        public ushort[] SerializeAngles(Angles angles)
        {
            var r = new List<ushort>();
            foreach (var theta in angles.Thetas)
                r.AddRange(SerializeAngle(theta));
            return r.ToArray();
        }
        
        private void SendAngles(Angles angles, CancellationToken cancellationToken, WaitHandle continueHandle)
        {
            cancellationToken.ThrowIfCancellationRequested();
            continueHandle.WaitOne();
            _rate.Sleep();
            _client.WriteDataNoResponse(AnglesAddress, SerializeAngles(angles));
        }
        
        private void OnProgressUpdated(int current, int total)
        {
            ProgressUpdated?.Invoke(this, (current, total));
        }

        public static double DeserializeAngle(ushort[] readData)
        {
            var r = new byte[readData.Length * 2];
            for (int i = 0; i < readData.Length; i++)
            {
                r[i * 2] = (byte) readData[i];
                r[i * 2 + 1] = (byte)(readData[i] >> 8);
            }

            return BitConverter.ToDouble(r, 0);
        }
    }
}