using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using App.Core.Contracts;

namespace App.Api.Controllers
{
    public class ValuesController : ApiController
    {
        private readonly ITraceStepper _traceStepper;

        public ValuesController( ITraceStepper traceStepper)
        {
            _traceStepper = traceStepper;
        }

        public IEnumerable<string> Get()
        {
            try
            {
                _traceStepper.WriteOperation("Entered get", "Index", "No metadata");
                return new[] { "value1", "value2" };
            }
            finally
            {
                _traceStepper.WriteOperation("Exited get", "Index", "No metadata");
            }
        }
    }
}