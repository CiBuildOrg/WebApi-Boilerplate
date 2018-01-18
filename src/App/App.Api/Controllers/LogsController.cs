using System.Web.Mvc;
using App.Dto.Request;
using App.Services.Contracts;
using App.Core;

namespace App.Api.Controllers
{
    [Authorize(Roles = Roles.SuperAdmin)]
    public class LogsController : Controller
    {
        private readonly ILogService _logService;

        public LogsController(ILogService logService)
        {
            _logService = logService;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetTracesPaged(int offset, int limit, string search, string sort, string order)
        {
            var traces = _logService.GetTraces(new TraceSearchRequest
            {
                Limit = limit,
                Search = search,
                Offset = offset,
                Sort = sort,
                Order = order
            });

            return Json(new
            {
                total = traces.Item1,
                rows = traces.Item2
            }, JsonRequestBehavior.AllowGet);
        }
    }
}