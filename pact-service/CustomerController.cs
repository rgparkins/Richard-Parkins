using System;
using System.Threading.Tasks;
using domain;
using Microsoft.AspNetCore.Mvc;

namespace pact_service
{
    [Route("customers")]
    public class CustomerController : Controller
    {
        private readonly ICustomerStore _customers;

        public CustomerController(ICustomerStore customers)
        {
            _customers = customers;
        }

        [Route("{id}", Name ="Get")]
        [HttpGet]
        public IActionResult Get(string id)
        {
            try
            {
                var saved = _customers.GetById(id);
            
                return CreatedAtRoute("Get", new { id = saved.Id }, saved);
            }
            catch (Exception ex)
            {
                return BadRequest("Customer already exists");
            }
        }
        
        [HttpPost]
        public async Task<IActionResult> Create([FromBody]Customer customer)
        {
            try
            {
                var saved = await _customers.Add(customer);

                return CreatedAtRoute("Get", new {id = saved.Id}, saved);
            }
            catch (Exception ex)
            {
                return BadRequest("Customer already exists");
            }
        }
    }
}