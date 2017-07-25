using System.Globalization;
using System.Web.Mvc;
using App.Core.Contracts;

namespace App.Api.Controllers
{
    public class HomeController : Controller
    {
        private readonly INow _now;
        private readonly ITraceStepper _traceStepper;

        public HomeController(INow now, ITraceStepper traceStepper)
        {
            _now = now;
            _traceStepper = traceStepper;
        }

        // GET: Home
        public ActionResult Index()
        {
            try
            {
                _traceStepper.WriteOperation("Entered index", "Index", "No metadata");

                return new ContentResult
                {
                    Content = "hello" + _now.Now.ToString(CultureInfo.InvariantCulture)
                };
            }
            finally
            {
                _traceStepper.WriteOperation("Exited index", "Index", "No metadata");
            }
        }
    }
}