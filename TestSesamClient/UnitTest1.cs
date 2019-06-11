using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SesamNetCoreClient;
using System;
using System.IO;
using System.Linq;

namespace TestSesamClient
{
    [TestClass]
    public class UnitTest1
    {
        private string jwt;
        private string subscriptionId;

        [TestInitialize]
        public void Init() {
            using (var file = File.OpenText("../../../Properties/launchSettings.json"))
            {
                var reader = new JsonTextReader(file);
                var jObject = JObject.Load(reader);

                var variables = jObject
                    .GetValue("profiles")
                    //select a proper profile here
                    .SelectMany(profiles => profiles.Children())
                    .SelectMany(profile => profile.Children<JProperty>())
                    .Where(prop => prop.Name == "environmentVariables")
                    .SelectMany(prop => prop.Value.Children<JProperty>())
                    .ToList();

                foreach (var variable in variables)
                {
                    Environment.SetEnvironmentVariable(variable.Name, variable.Value.ToString());
                }
            }
            this.jwt = Environment.GetEnvironmentVariable("SESAM_JWT");
            this.subscriptionId = Environment.GetEnvironmentVariable("SUBSCRIPTION_ID");
        }

        [TestCleanup]
        public void DeInit()
        {
            using (var file = File.OpenText("../../../Properties/launchSettings.json"))
            {
                var reader = new JsonTextReader(file);
                var jObject = JObject.Load(reader);

                var variables = jObject
                    .GetValue("profiles")
                    //select a proper profile here
                    .SelectMany(profiles => profiles.Children())
                    .SelectMany(profile => profile.Children<JProperty>())
                    .Where(prop => prop.Name == "environmentVariables")
                    .SelectMany(prop => prop.Value.Children<JProperty>())
                    .ToList();

                foreach (var variable in variables)
                {
                    Environment.SetEnvironmentVariable(variable.Name, null);
                }
            }
        }

        [TestMethod]
        public void testInit()
        {

            Client client = new Client(this.jwt, this.subscriptionId);
            var result = client.Ping();
            Assert.AreEqual(true, result);
        }

        [TestMethod]
        [ExpectedException(typeof(System.InvalidOperationException))]
        public void testInitMustFail()
        {
            Client client = new Client("", "fake-id");
            var result = client.Ping();
            Assert.AreEqual(true, result);
        }
    }
}
