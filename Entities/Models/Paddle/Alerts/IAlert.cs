using System;
using System.Collections.Generic;
using System.Text;

namespace Entities.Models.Paddle.Alerts
{
    public interface IAlert
    {
        int AlertId { get; set; }

        DateTime EventTime { get; set; }
    }
}
