using System.Collections.Generic;
using Microsoft.AspNetCore;
using PactNet;
using PactNet.Infrastructure.Outputters;
using Xunit;

namespace pact_service.tests
{
    public class AddUserTests
    {
        [Fact]
        public void EnsureSomethingApiHonoursPactWithConsumer()
        {
            //Arrange
            const string serviceUri = "http://localhost:9222";
            var config = new PactVerifierConfig
            {
                Outputters = new List<IOutput> //NOTE: We default to using a ConsoleOutput, however xUnit 2 does not capture the console output, so a custom outputter is required.
                {
                    new ConsoleOutput()
                },
                Verbose = true //Output verbose verification logs to the test output
            };

            using (WebHost.CreateDefaultBuilder<Startup>(serviceUri))
            {
                //Act / Assert
                IPactVerifier pactVerifier = new PactVerifier(config);
                pactVerifier
                    .ProviderState($"{serviceUri}/provider-states")
                    .ServiceProvider("Consumer API", serviceUri)
                    .HonoursPactWith("Consumer")
                    .PactUri("..\\..\\..\\pact-consumer.tests\\pacts\\consumer-customer_api.json")
                    .Verify();
            }
        }
    }
}