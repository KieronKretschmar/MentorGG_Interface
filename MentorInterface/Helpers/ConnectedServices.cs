using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MentorInterface.Helpers
{
    /// <summary>
    /// A collection of connected services
    /// </summary>
    public static class ConnectedServices
    {
        /// <summary>
        /// Demo Central
        /// </summary>
        public static ConnectedService DemoCentral = new ConnectedService(
            "demo-central",
            "demo-central.default.svc.cluster.local",
            false);

        /// <summary>
        /// FaceIt Match Gatherer
        /// </summary>
        public static ConnectedService FaceitMatchGatherer = new ConnectedService(
            "faceit-match-gatherer",
            "faceit-match-gatherer.default.svc.cluster.local",
            false);

        /// <summary>
        /// Match Retriever
        /// </summary>
        public static ConnectedService MatchRetriever = new ConnectedService(
            "match-retriever",
            "match-retriever.default.svc.cluster.local",
            false);

        /// <summary>
        /// Sharing Code Gatherer
        /// </summary>
        public static ConnectedService SharingCodeGatherer = new ConnectedService(
            "sharing-code-gatherer",
            "sharing-code-gatherer.default.svc.cluster.local",
            false);

        /// <summary>
        /// Paddle Api
        /// </summary>
        public static ConnectedService PaddleApi = new ConnectedService(
            "paddle-api",
            "vendors.paddle.com/api/2.0",
            true);
    }


    /// <summary>
    /// A Connected Service that is communicated via MentorInterface.
    /// </summary>
    public struct ConnectedService
    {
        /// <summary>
        /// Human readable name of the service.
        /// </summary>
        public readonly string Name;

        /// <summary>
        /// DNS Address for internal communication.
        /// </summary>
        public readonly string DNSAddress;

        /// <summary>
        /// Whether to use Https instead of Http.
        /// </summary>
        public readonly bool UseHttps;

        /// <summary>
        /// Define a Connected Service.
        /// </summary>
        /// <param name="name">Name of the service</param>
        /// <param name="dnsAddress">DNS Address of the service</param>
        public ConnectedService(string name, string dnsAddress, bool useHttps)
        {
            Name = name;
            DNSAddress = dnsAddress;
            UseHttps = useHttps;
        }

        /// <summary>
        /// Return the ConnectedService's name
        /// </summary>
        public static implicit operator string(ConnectedService connectedService)
        {
            return connectedService.Name;
        }
    }

}
