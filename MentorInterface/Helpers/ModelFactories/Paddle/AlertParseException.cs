using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentorInterface.Helpers.ModelFactories.Paddle
{
    /// <summary>
    /// Used to indicate when a Factory fails to parse a Paddle Alert.
    /// </summary>
    public class AlertParseException : FormatException
    {
        public AlertParseException() :base() { }
        public AlertParseException(string message) : base(message) { }
        public AlertParseException(string message, Exception innerException) : base(message, innerException) { }
    }
}
