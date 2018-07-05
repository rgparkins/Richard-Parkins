using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using domain;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace pact_consumer
{
    public interface IPactConsumer
    {
        Task<Customer> CreateUser(string firstname, string surname);

    }
    
    public class PactConsumer : IPactConsumer
    {
        private readonly string _host;
        
        public PactConsumer(string host)
        {
            _host = host;
        }

        public async Task<Customer> CreateUser(string firstname, string surname)
        {
            var client = new HttpClient
            {
                BaseAddress = new Uri(_host)
            };
            
            var customer = new Customer
            {
                Firstname = firstname,
                Surname = surname
            };

            var result = await client.PostAsync("/customers",
                new StringContent(JsonConvert.SerializeObject(customer, new JsonSerializerSettings
                {
                    ContractResolver = new CamelCasePropertyNamesContractResolver(),
                    DefaultValueHandling = DefaultValueHandling.Include,
                    NullValueHandling = NullValueHandling.Ignore
                }), Encoding.UTF8, "application/json"));

            if (result.IsSuccessStatusCode)
            {
                return JsonConvert.DeserializeObject<Customer>(await result.Content.ReadAsStringAsync());
            }

            throw new Exception(await result.Content.ReadAsStringAsync());
        }
    }
}