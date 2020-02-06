using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentorInterface.Payment.IncomingModels
{
    public interface IPaddleAlert
    {
        string AlertName { get; set; }
        string AlertId { get; set; }
        string PaddleSignature { get; set; }
    }
}
