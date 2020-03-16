using Newtonsoft.Json;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MentorInterface.Paddle
{
    /// <summary>
    /// Verify Paddle Webhooks
    /// https://developer.paddle.com/webhook-reference/verifying-webhooks
    /// </summary>
    public interface IWebhookVerifier
    {
        /// <summary>
        /// Verify a message from Paddle
        /// https://github.com/whitej031788/c--paddle-webhook-verify
        /// </summary>
        bool IsAlertValid(Dictionary<string, string> alert);
    }

    /// <summary>
    /// Verify Paddle Webhooks
    /// https://developer.paddle.com/webhook-reference/verifying-webhooks
    /// </summary>
    public class WebhookVerifier : IWebhookVerifier
    {

        private string PublicKey { get; }

        private const string SignatureKey = "p_signature";

        /// <summary>
        /// Initialize with a public key provided by Paddle.
        /// </summary>
        /// <param name="publicKey"></param>
        public WebhookVerifier(string publicKey)
        {
            this.PublicKey = publicKey;
        }


        /// <summary>
        /// Verify a message from Paddle
        /// https://github.com/whitej031788/c--paddle-webhook-verify
        /// </summary>
        public bool IsAlertValid(Dictionary<string, string> alert)
        {
            if (!alert.ContainsKey(SignatureKey))
            {
                return false;
            }

            PhpSerializer serializer = new PhpSerializer();

            byte[] signature = Convert.FromBase64String(alert[SignatureKey] ?? "");

            SortedDictionary<string, dynamic> sorted = new SortedDictionary<string, dynamic>();
            foreach (string key in alert.Keys)
            {
                var val = alert[key] ?? "";
                if (key != SignatureKey)
                {
                    sorted.Add(key, val);
                }
            }
            var sortedPayload = serializer.Serialize(sorted);
            bool isSuccess = VerifySignature(signature, sortedPayload, PublicKey);
            return isSuccess;
        }

        private bool VerifySignature(byte[] signatureBytes, string message, string pubKey)
        {
            StringReader newStringReader = new StringReader(pubKey);
            AsymmetricKeyParameter publicKey = (AsymmetricKeyParameter)new PemReader(newStringReader).ReadObject();
            ISigner sig = SignerUtilities.GetSigner("SHA1withRSA");
            sig.Init(false, publicKey);
            byte[] messageBytes = Encoding.UTF8.GetBytes(message);
            sig.BlockUpdate(messageBytes, 0, messageBytes.Length);
            return sig.VerifySignature(signatureBytes);
        }


    }
}
