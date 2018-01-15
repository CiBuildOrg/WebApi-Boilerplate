using System.Collections.Generic;
using System.Web.Http;
using App.Core.Contracts;

namespace App.Api.Controllers
{
    public class ValuesController : ApiController
    {
        public ValuesController()
        {
        }

        public IEnumerable<string> Get()
        {
            return new[] { "value1", "value2" };
        }
    }
}