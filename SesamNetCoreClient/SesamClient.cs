using System;
using System.Net.Http;
using System.Net.Http.Headers;

namespace SesamNetCoreClient
{
    public class Client {
        private static readonly HttpClient httpClient = new HttpClient();
        private static readonly string API_URL = "https://datahub-{0}.sesam.cloud/api/";

        public Client(string jwt, string subscriptionId)
        {
            var apiUrl = BuildApiUrlFromSubscriptionId(subscriptionId);

            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", jwt);
            httpClient.BaseAddress = new Uri(apiUrl);
        }

        private String BuildApiUrlFromSubscriptionId(string subId) {
            if (!subId.Contains('-')) {
                throw new ArgumentException("Bad subscription id");
            }
            return string.Format(API_URL, subId.Split('-')[0]);
        }

        /// <summary>
        /// Check if node is healthy
        /// </summary>
        /// <returns></returns>
        public bool Ping() {
            HttpResponseMessage response = httpClient.GetAsync("health").Result;
            response.EnsureSuccessStatusCode();
            return true;
        }


    }
}
