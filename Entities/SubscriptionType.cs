using System;
using System.Collections.Generic;
using System.Text;

namespace Entities
{
    /// <summary>
    /// SubscriptionType
    /// </summary>
    public enum SubscriptionType : byte
    {
        /// <summary>
        /// Free
        /// </summary>
        Free = 1,

        /// <summary>
        /// Premium
        /// </summary>
        Premium = 2,

        /// <summary>
        /// Ultimate
        /// </summary>
        Ultimate = 3,
    }
}
