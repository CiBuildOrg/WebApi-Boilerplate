using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Mvc;
using App.Core.Contracts;

namespace App.Api.Controllers
{
    public class HomeController : Controller
    {
        private readonly INow _now;

        public HomeController(INow now)
        {
            _now = now;
        }

        // GET: Home
        public ActionResult Index()
        {
            return new ContentResult
            {
                Content = "hello" + _now.Now.ToString(CultureInfo.InvariantCulture)
            };
        }
    }
}