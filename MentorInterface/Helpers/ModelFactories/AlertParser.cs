using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentorInterface.Helpers.ModelFactories
{
    /// <summary>
    /// Helper to parse Alerts from Paddle.
    /// </summary>
    public static class AlertParser
    {
        public static bool ParseBool(string value)
        {
            // Try the convention Boolean Parse.
            bool result;
            if(Boolean.TryParse(value, out result))
            {
                return result;
            }

            if (value == "0")
            {
                return false;
            }

            if(value == "1")
            {
                return true;
            }

            if(value == null)
            {
                return false;
            }

            throw new ArgumentException($"Unexpected value, cannot parse {value}!");

        }
    }
}
