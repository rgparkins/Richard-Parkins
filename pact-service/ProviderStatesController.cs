using System;
using System.Collections.Generic;
using domain;
using Microsoft.AspNetCore.Mvc;

namespace pact_service
{
    public class ProviderState
    {
        public string Consumer { get; set; }
        public string State { get; set; }
    }
    
    [Route("provider-states")]
    public class ProviderStatesController : Controller
    {
        private const string ConsumerName = "Consumer";
        Dictionary<string, Action> _providerStates;
        
        private void AddTesterIfItDoesntExist()
        {
            //Add code to go an inject or insert the tester data
        }

        public ProviderStatesController()
        {
            _providerStates = new Dictionary<string, Action>
            {
                {
                    "There is a service",
                    AddTesterIfItDoesntExist
                }
            };
        }
        
        [HttpPost]
        public IActionResult Validate(ProviderState state)
        {
            //A null or empty provider state key must be handled
            if (state != null &&
                !string.IsNullOrEmpty(state.State) &&
                state.Consumer == ConsumerName)
            {
                _providerStates[state.State].Invoke();
            }

            return Content("");
        }
    }
}