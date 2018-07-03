using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PactNet;
using PactNet.Mocks.MockHttpService;
using PactNet.Models;

namespace pact_consumer.tests.context
{
    public class ConsumerApiPact : IDisposable
    {
        public IPactBuilder PactBuilder { get; }
        public IMockProviderService MockProviderService { get; }

        public int MockServerPort => 8009;
        public string MockProviderServiceBaseUri => String.Format("http://localhost:{0}", MockServerPort);

        public ConsumerApiPact()
        {
            PactBuilder = new PactBuilder(); 

            PactBuilder
                .ServiceConsumer("Consumer")
                .HasPactWith("Customer API");

            //Configure the http mock server
            MockProviderService = PactBuilder.MockService(MockServerPort, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver()
            }, host: IPAddress.Any);
        }

        public void Dispose()
        {
            PactBuilder.Build();
        }
    }
}