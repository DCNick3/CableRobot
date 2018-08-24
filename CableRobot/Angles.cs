using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CableRobot
{
    public class Angles
    {
        private double[] _thetas = new double[4];

        public double Theta1 { get => _thetas[0]; set => _thetas[0] = value; }
        public double Theta2 { get => _thetas[1]; set => _thetas[1] = value; }
        public double Theta3 { get => _thetas[2]; set => _thetas[2] = value; }
        public double Theta4 { get => _thetas[3]; set => _thetas[3] = value; }

        public double[] Thetas => _thetas;

        public double this[int id]
        {
            get => _thetas[id];
            set => _thetas[id] = value;
        }

        public override string ToString() => ToString(CultureInfo.InvariantCulture);

        public string ToString(IFormatProvider formatProvider) => string.Join(" ", _thetas.Select(t => t.ToString(formatProvider)));
    }
}
