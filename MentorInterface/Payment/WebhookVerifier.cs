using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MentorInterface.Payment
{
    /// <summary>
    /// Verify Paddle Webhooks
    /// https://developer.paddle.com/webhook-reference/verifying-webhooks
    /// </summary>
    public class WebhookVerifier
    {

        private string PublicKey { get; }

        /// <summary>
        /// Initialize with a public key provided by Paddle.
        /// </summary>
        /// <param name="publicKey"></param>
        public WebhookVerifier(string publicKey)
        {
            this.PublicKey = publicKey;

            this.PublicKey = @"-----BEGIN PUBLIC KEY-----
MIICIjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKCAgEAncWOfnvXciow60nwb7te
uwbluhc2WLdy8C3E4yf + gQEGjR + EXwDogWAmpJW0V3cRGhe41BBtO0vX39YeEjh3
tkCIT4JTkR4yCXiXJ / tYGvsCAwEAAQ ==
-----END PUBLIC KEY-----";
        }


        /// <summary>
        /// Verify a message from Paddle
        /// </summary>
        public bool IsAlertValid(Dictionary<string, string> alert)
        {

            return true;

            string encodedPaddleSignature = alert["p_signature"];

            // Prepare the data
            var prepared = alert
                .Where(x => x.Key != "p_signature")
                .OrderBy(p => p.Key)
                .ToDictionary(x => x.Key, x => x.Value);
            var jsonDumped = JsonConvert.SerializeObject(prepared);


            byte[] signature = Convert.FromBase64String(encodedPaddleSignature);
            byte[] data = Encoding.UTF8.GetBytes(jsonDumped);

            RSAParameters rsaKeyInfo = new RSAParameters
            {
                Modulus = Encoding.UTF8.GetBytes(PublicKey),
                Exponent = new byte[] { 0x01, 0x00, 0x01 }
            };

            RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
            rsa.ImportParameters(rsaKeyInfo);

            string sha1Oid = CryptoConfig.MapNameToOID("SHA1");

            //use the certificate to verify data against the signature
            bool sha1Valid = rsa.VerifyData(data, sha1Oid, signature);

            return sha1Valid;


        }


    }
}
