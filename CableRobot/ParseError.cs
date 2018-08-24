using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace CableRobot
{
    class ParseError : Exception
    {
        public ParseError()
        {
        }

        public ParseError(string message) : base(message)
        {
        }

        public ParseError(string message, Exception innerException) : base(message, innerException)
        {
        }

        protected ParseError(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}
