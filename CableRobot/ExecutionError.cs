using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CableRobot
{
    public class ExecutionError : Exception
    {
        public ExecutionError()
        {
        }

        public ExecutionError(string message) : base(message)
        {
        }

        public ExecutionError(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ExecutionError(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
