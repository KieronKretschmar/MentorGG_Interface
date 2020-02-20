using Microsoft.DotNet.PlatformAbstractions;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MenterInterfaceTests
{

    public static class TestDataHelper
    {
        public static readonly string TestDataRoot = "TestData";

        public static string FilePath(string fileName)
        {
            return Path.Combine(AbsoluteRootPath(), fileName);
        }
        public static string AbsoluteRootPath()
        {
            string startupPath = ApplicationEnvironment.ApplicationBasePath;
            var pathItems = startupPath.Split(Path.DirectorySeparatorChar);
            var pos = pathItems.Reverse().ToList().FindIndex(x => string.Equals("bin", x));
            string projectPath = String.Join(Path.DirectorySeparatorChar.ToString(), pathItems.Take(pathItems.Length - pos - 1));
            return Path.Combine(projectPath, TestDataRoot);
        }

        /// <summary>
        /// Returns a string used to mock a 
        /// </summary>
        /// <param name="overrideParams"></param>
        /// <returns></returns>
        public static string GetPaddleFormData(string alert_name, Dictionary<string,string> overrideParams)
        {
            var validDefaultJson = $"\"alert_id\": \"1885693839\" \n " +
              $"\"alert_name\": \"{alert_name}\"\n " +
              $"\"cancel_url\": \"https://checkout.paddle.com/subscription/cancel?user=1&subscription=7&hash=42216b42d61c6a67535430335baa88aea66873d8\"\n " +
              $"\"checkout_id\": \"8-3165615a1a7afa5-4b0c259b07\"\n " +
              $"\"currency\": \"GBP\"\n " +
              $"\"email\": \"willy.hoppe@example.net\"\n " +
              $"\"event_time\": \"2020-02-17 14:39:52\"\n " +
              $"\"linked_subscriptions\": \"6 + 4 + 5\"\n " +
              $"\"marketing_consent\": null\n " +
              $"\"next_bill_date\": \"2020-03-01\"\n " +
              $"\"passthrough\": \"Example String\"\n " +
              $"\"quantity\": \"99\"\n " +
              $"\"source\": \"Order\"\n " +
              $"\"status\": \"trialing\"\n " +
              $"\"subscription_id\": \"2\"\n " +
              $"\"subscription_plan_id\": \"4\"\n " +
              $"\"unit_price\": \"unit_price\"\n " +
              $"\"update_url\": \"https://checkout.paddle.com/subscription/update?user=9&subscription=7&hash=72b43522c9e5388176f29de0281ed18e03526176\"\n " +
              $"\"user_id\": \"2\"";

            var json = new JObject(validDefaultJson);

            foreach (var overrideKey in overrideParams.Keys)
            {
                json[overrideKey] = overrideParams[overrideKey];
            }
            var debug = json.ToString(); // TODO: Check if format is correct. If yes, replace code above with load from json
            return json.ToString();
        }
    }
}