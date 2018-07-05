using System;
using System.Collections.Generic;
using domain;
using pact_consumer.tests.context;
using PactNet.Mocks.MockHttpService;
using PactNet.Mocks.MockHttpService.Models;
using Xunit;

namespace pact_consumer.tests
{
    public class AddConsumerTest : IClassFixture<ConsumerApiPact>, IDisposable
    {
        private IMockProviderService _mockProviderService;
        private string _mockProviderServiceBaseUri;

        public AddConsumerTest(ConsumerApiPact data)
        {
            _mockProviderService = data.MockProviderService;
            _mockProviderService.ClearInteractions(); 
            _mockProviderServiceBaseUri = data.MockProviderServiceBaseUri;
        }

        [Fact]
        public void Add_customer()
        {
            //Arrange
            _mockProviderService
                .Given("There is a service")
                .UponReceiving("A POST request to create a customer")
                .With(new ProviderServiceRequest
                {
                    Method = HttpVerb.Post,
                    Path = "/customers",
                    Query = "",
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8" }
                    },
                    Body = new Customer
                    {
                        Firstname = "richard",
                        Surname = "parkins"
                    }
                })
                .WillRespondWith(new ProviderServiceResponse
                {
                    Status = 201,
                    Headers = new Dictionary<string, object>
                    {
                        { "Content-Type", "application/json; charset=utf-8"}
                    },
                    Body = new Customer
                    {
                        Id = "richard-parkins",
                        Firstname = "richard",
                        Surname = "parkins"
                    }
                });

            var consumer = new PactConsumer(_mockProviderServiceBaseUri);

            //Act
            var result = consumer.CreateUser("richard", "parkins").Result;

            //Assert
            Assert.Equal("richard", result.Firstname);
            Assert.Equal("parkins", result.Surname);
            
            Assert.Equal("richard-parkins", result.Id);
            
            _mockProviderService.VerifyInteractions();
        }

        public void Dispose()
        {
            
        }
    }
}