using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using domain;

namespace pact_service
{
    public interface ICustomerStore
    {
        Task<Customer> Add(Customer customer);
        Customer GetById(string id);
    }

    public class CustomerStore : ICustomerStore
    {
        readonly Dictionary<string, Customer> _customers = new Dictionary<string, Customer>();
        
        public Task<Customer> Add(Customer customer)
        {
            var key = Key.Generate(customer);
            
            if (_customers.ContainsKey(key))
            {
                throw new DuplicateNameException($"{key} already exists");
            }
            
            _customers.Add(key, customer);

            return Task.FromResult(customer);
        }

        public Customer GetById(string id)
        {
            return _customers.ContainsKey(id) ? _customers[id] : null;
        }

        class Key
        {
            public static string Generate(Customer customer)
            {
                return $"{customer.Firstname}-${customer.Surname}";
            }
        }
    }
}