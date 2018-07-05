using System;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using PactNet;
using PactNet.Mocks.MockHttpService;
using IPAddress = PactNet.Models.IPAddress;

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
            PactBuilder = new PactBuilder(new PactConfig
            {
                PactDir = "../../../../pacts",
                SpecificationVersion = "2.0.0" 
            }); 

            PactBuilder
                .ServiceConsumer("Consumer")
                .HasPactWith("Consumer API");

            //Configure the http mock server
            MockProviderService = PactBuilder.MockService(MockServerPort, new JsonSerializerSettings
            {
                ContractResolver = new CamelCasePropertyNamesContractResolver(),
                DefaultValueHandling = DefaultValueHandling.Include,
                NullValueHandling = NullValueHandling.Ignore
            }, host:IPAddress.Any);
        }

        public void Dispose()
        {
            PactBuilder.Build();
        }
    }
}