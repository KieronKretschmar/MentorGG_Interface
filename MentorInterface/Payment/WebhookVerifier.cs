using MentorInterface.Payment.IncomingModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentorInterface.Payment
{
    /// <summary>
    /// Verify Paddle Webhooks
    /// https://developer.paddle.com/webhook-reference/verifying-webhooks
    /// </summary>
    public class WebhookVerifier
    {

        private string publicKey { get; }

        /// <summary>
        /// Initialize with a public key provided by Paddle.
        /// </summary>
        /// <param name="publicKey"></param>
        public WebhookVerifier(string publicKey)
        {
            this.publicKey = publicKey;
        }


        /// <summary>
        /// Verify a message from Paddle
        /// </summary>
        public bool IsAlertValid(IPaddleAlert alert)
        {
            string json = JsonConvert.SerializeObject(alert);
            return false;
        }


    }
}
