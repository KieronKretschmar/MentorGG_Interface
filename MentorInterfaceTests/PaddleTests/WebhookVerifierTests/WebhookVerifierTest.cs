using MenterInterfaceTests;
using MentorInterface.Paddle;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MentorInterfaceTests.PaddleTests.WebhookVerifierTest
{
    [TestClass]
    public class WebhookVerifierTest
    {
        readonly WebhookVerifier webhookVerifier;

        public WebhookVerifierTest()
        {
            webhookVerifier = new WebhookVerifier(
                File.ReadAllText(
                    TestDataHelper.FilePath(
                        "WebhookAlerts/PaddlePublicKey.pem")));
        }

        [DataRow("WebhookAlerts/Valid/Valid_01.json", DisplayName = "Valid Alert")]
        [DataTestMethod]
        public void VerifyValidAlertTest(string jsonPath)
        {
            var alert = AlertFromJsonPath(jsonPath);
            Assert.IsTrue(
                webhookVerifier.IsAlertValid(alert));
        }

        [DataRow("WebhookAlerts/Invalid/Mismatch01.json", DisplayName = "Signature Mismatch 01")]
        [DataRow("WebhookAlerts/Invalid/Mismatch02.json", DisplayName = "Signature Mismatch 02")]
        [DataRow("WebhookAlerts/Invalid/MissingSignature.json", DisplayName = "Missing Signature Key")]
        [DataRow("WebhookAlerts/Invalid/EmptySignature.json", DisplayName = "Empty Signature")]
        [DataTestMethod]
        public void VerifyInvalidAlertTest(string jsonPath)
        {
            var alert = AlertFromJsonPath(jsonPath);
            Assert.IsFalse(
                webhookVerifier.IsAlertValid(alert));
        }

        /// <summary>
        /// Return an Alert Dictionary<string,string> from a JsonPath
        /// </summary>
        private Dictionary<string, string> AlertFromJsonPath(string jsonPath)
        {
            return JsonConvert.DeserializeObject<Dictionary<string, string>>(
                File.ReadAllText(TestDataHelper.FilePath(jsonPath)));
        }
    }
}
