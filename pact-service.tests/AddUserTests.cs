using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using PactNet;
using PactNet.Infrastructure.Outputters;
using Xunit;
using Xunit.Abstractions;

namespace pact_service.tests
{
    public class XUnitOutput : IOutput
    {
        private readonly ITestOutputHelper _output;

        public XUnitOutput(ITestOutputHelper output)
        {
            _output = output;
        }

        public void WriteLine(string line)
        {
            Console.WriteLine(line);
        }
    }
    
    public class AddUserTests
    {
        private readonly ITestOutputHelper _output;

        public AddUserTests(ITestOutputHelper output)
        {
            _output = output;
        }
        
        [Fact]
        public void EnsureSomethingApiHonoursPactWithConsumer()
        {
            //Arrange
            const string serviceUri = "http://localhost:5000";
            var config = new PactVerifierConfig
            {
                Outputters = new List<IOutput> 
                {
                    new XUnitOutput(_output)
                },
                ProviderVersion = "2.0.0",
                Verbose = true 
            };

            var builder = new WebHostBuilder()
                .UseKestrel()
                .ConfigureLogging(logging =>
                {
                    logging.SetMinimumLevel(LogLevel.Warning);
                    logging.AddConsole();
                })
                .UseStartup<Startup>();

            using (var service = builder.Start())
            {
                //Act / Assert
                IPactVerifier pactVerifier = new PactVerifier(config);
                pactVerifier
                    .ProviderState($"{serviceUri}/provider-states")
                    .ServiceProvider("Consumer API", serviceUri)
                    .HonoursPactWith("Consumer")
                    .PactUri(
                        "http://Richards-MacBook-Pro-98861.local:32792/pacts/provider/Consumer%20API/consumer/Consumer/latest") //You can specify a http or https uri
                    .Verify();   
            }
        }
    }
}