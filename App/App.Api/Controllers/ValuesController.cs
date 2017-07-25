using System.Collections.Generic;
using System.Web.Http;
using App.Core.Contracts;

namespace App.Api.Controllers
{
    public class ValuesController : ApiController
    {
        private readonly ITraceProvider _traceProvider;

        public ValuesController( ITraceProvider traceProvider)
        {
            _traceProvider = traceProvider;
        }

        public IEnumerable<string> Get()
        {
            try
            {
                _traceProvider.WriteOperation("Entered get", "Index", "No metadata");
                return new[] { "value1", "value2" };
            }
            finally
            {
                _traceProvider.WriteOperation("Exited get", "Index", "No metadata");
            }
        }
    }
}