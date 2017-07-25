using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Globalization;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using App.Core.Contracts;
using App.Core.Utils;
using App.Dto;
using App.Entities;
using App.Infrastructure.Tracing.Resources;
using LinqKit;

namespace App.Api.Controllers
{
    public class LogsController : Controller
    {
        private const string XmlHeader8 = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
        private const string XmlHeader16 = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

        private const string LogsSortAscendingKey = "asc";
        private const string LogsSortDescendingKey = "desc";

        private const int LogLimit = 2 * 24; // 2 days

        private readonly IDatabaseContext _context;
        private readonly INow _now;

        public LogsController(IDatabaseContext context, INow now)
        {
            _context = context;
            _now = now;
        }

        public ActionResult Index()
        {
            return View();
        }

        public JsonResult GetTracesPaged(int offset, int limit, string search, string sort, string order)
        {
            // get logs newer than 7 days
            var nowUtc = _now.UtcNow;

            var traces =
                _context.AsQueryable<LogEntry>().Include(x => x.Steps)
                    .Where(x => DbFunctions.AddHours(x.Timestamp, LogLimit) > nowUtc)
                    .AsQueryable();

            IOrderedQueryable<LogEntry> pagedTraces;

            switch (order)
            {
                case LogsSortAscendingKey:
                    pagedTraces = traces.OrderAscending(sort);
                    break;
                case LogsSortDescendingKey:
                    pagedTraces = traces.OrderDescending(sort);
                    break;
                default:
                    pagedTraces = traces.OrderDescending(sort);
                    break;
            }

            List<LogEntry> pagedTracesList;

            if (string.IsNullOrEmpty(search))
            {
                pagedTracesList = pagedTraces
                    .Skip((offset / limit) * limit)
                    .Take(limit).ToList();
            }
            else
            {
                string searchLikeExpression = $"%{search}%";
                Expression<Func<LogEntry, bool>> searchExpression =
                    entry => SqlFunctions.PatIndex(searchLikeExpression, entry.RequestUri) > 0 ||
                             entry.Steps.Any(x => SqlFunctions.PatIndex(searchLikeExpression, x.Frame) > 0 ||
                                                  SqlFunctions.PatIndex(searchLikeExpression, x.Message) > 0 ||
                                                  SqlFunctions.PatIndex(searchLikeExpression, x.Name) > 0 ||
                                                  SqlFunctions.PatIndex(searchLikeExpression, x.Metadata) > 0);

                pagedTracesList = pagedTraces.AsExpandable().Where(searchExpression).Skip((offset / limit) * limit).Take(limit).ToList();
            }

            var tracesVms = new List<TraceViewModel>();

            foreach (var trace in pagedTracesList)
            {
                var traceSteps = trace.Steps.OrderBy(x => x.Index).ToList();

                var builder = new StringBuilder();
                builder.Append("<p style=\"white-space: nowrap;\">Start request </p>");

                foreach (var tracestep in traceSteps)
                {
                    builder.Append(string.Format("<p style=\"white-space: nowrap;\">{0}</p>", string.Format("From {0} method located in frame {1} {2} {3} \r\n", tracestep.Source,
                        string.Format("<pre class=\"prettyprint lang-cs\">{0}</pre>", tracestep.Frame),
                        (!string.IsNullOrEmpty(tracestep.Name) ? string.Format(" (which processes {0}) ", tracestep.Name) : ""),
                        (!string.IsNullOrEmpty(tracestep.Message) ? string.Format(" (with message {0}) ", tracestep.Message) : ""))));

                    if (string.IsNullOrEmpty(tracestep.Metadata)) continue;

                    builder.Append("<p style=\"white-space: nowrap;\">With metadata: </p>");

                    string beautified;
                    if (XmlUtils.IsValidXml(tracestep.Metadata))
                    {
                        // xml 
                        // operation metadata is xml in our case
                        beautified = XmlPrettifyHelper.Prettify(tracestep.Metadata.Replace(XmlHeader8, "").Replace(XmlHeader16, ""));
                    }
                    else if (JsonUtils.IsValidJson(tracestep.Metadata))
                    {
                        beautified = string.Format("<pre class=\"prettyprint lang-json\">{0}</pre>",
                            JsonPrettifier.BeautifyJson(tracestep.Metadata));
                    }
                    else
                    {
                        beautified = tracestep.Metadata;
                    }

                    builder.Append(beautified);
                }

                builder.Append("<p style=\"white-space: nowrap;\">End request </p>");

                var traceString = HttpUtility.HtmlEncode(builder.ToString());

                var captureDuration = trace.ResponseTimestamp.HasValue && trace.RequestTimestamp.HasValue;

                var item = new TraceViewModel
                {

                    Duration = captureDuration ? string.Format("{0} seconds",
                        (trace.ResponseTimestamp.Value - trace.RequestTimestamp.Value).TotalSeconds.ToString("#.##")) : "Duration not captured",
                    Timestamp = trace.Timestamp.ToString(CultureInfo.InvariantCulture),
                    Uri = trace.RequestUri,
                    Workflow = new HtmlString(HttpUtility.HtmlDecode(traceString)).ToHtmlString()
                };

                tracesVms.Add(item);
            }

            var model = new
            {
                total = traces.Count(),
                rows = tracesVms
            };

            return Json(model, JsonRequestBehavior.AllowGet);
        }
    }
}