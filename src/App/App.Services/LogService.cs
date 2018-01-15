using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Linq.Expressions;
using App.Core;
using App.Core.Contracts;
using App.Core.Utils;
using App.Database;
using App.Dto.Request;
using App.Dto.Response;
using App.Dto.Traces;
using App.Entities;
using App.Services.Contracts;
using AutoMapper;
using LinqKit;

namespace App.Services
{
    public class LogService : ILogService
    {
        private const string LogsSortAscendingKey = "asc";
        private const string LogsSortDescendingKey = "desc";

        private const int LogLimit = 2 * 24; // 2 days

        private readonly DatabaseContext _context;
        private readonly INow _now;
        private readonly IConfiguration _helper;
        private readonly IMapper _mapper;

        public LogService(DatabaseContext context, INow now, IConfiguration helper, IMapper mapper)
        {
            _context = context;
            _now = now;
            _helper = helper;
            _mapper = mapper;
        }

        private bool ShouldLog => _helper.GetBool(ConfigurationKeys.ShouldTrace);

        public List<TraceViewModel> GetTraces(TraceSearchRequest request)
        {
            //// get logs newer than 7 days
            //var nowUtc = _now.UtcNow;

            //var traces =
            //    _context.AsQueryable<LogEntry>().Include(x => x.Steps)
            //        .Where(x => DbFunctions.AddHours(x.Timestamp, LogLimit) > nowUtc)
            //        .AsQueryable();

            //IOrderedQueryable<LogEntry> pagedTraces;

            //switch (request.Order)
            //{
            //    case LogsSortAscendingKey:
            //        pagedTraces = traces.OrderAscending(request.Sort);
            //        break;
            //    case LogsSortDescendingKey:
            //        pagedTraces = traces.OrderDescending(request.Sort);
            //        break;
            //    default:
            //        pagedTraces = traces.OrderDescending(request.Sort);
            //        break;
            //}

            //List<LogEntry> pagedTracesList;

            //if (string.IsNullOrEmpty(request.Search))
            //{
            //    pagedTracesList = pagedTraces
            //        .Skip(request.Offset / request.Limit * request.Limit).Take(request.Limit).ToList();
            //}
            //else
            //{
            //    var searchLikeExpression = $"%{request.Search}%";
            //    Expression<Func<LogEntry, bool>> searchExpression =
            //        entry => SqlFunctions.PatIndex(searchLikeExpression, entry.RequestUri) > 0 ||
            //                 entry.Steps.Any(x => SqlFunctions.PatIndex(searchLikeExpression, x.Frame) > 0 ||
            //                                      SqlFunctions.PatIndex(searchLikeExpression, x.Message) > 0 ||
            //                                      SqlFunctions.PatIndex(searchLikeExpression, x.Name) > 0 ||
            //                                      SqlFunctions.PatIndex(searchLikeExpression, x.Metadata) > 0);

            //    pagedTracesList = pagedTraces.AsExpandable().Where(searchExpression)
            //        .Skip(request.Offset / request.Limit * request.Limit).Take(request.Limit).ToList();
            //}

            //return _mapper.Map<List<TraceViewModel>>(pagedTracesList);

            return null;
        }

        public void SaveTrace(ApiLogEntry entry, List<TraceStep> traceSteps)
        {
            //if (!ShouldLog) return;

            //var logEntry = new LogEntry
            //{
            //    Id = Guid.NewGuid(),
            //    Timestamp = _now.UtcNow,
            //    RequestTimestamp = entry.RequestTimestamp,
            //    ResponseTimestamp = entry.ResponseTimestamp,
            //    RequestUri = entry.RequestUri,
            //    Steps = new List<LogStep>()
            //};

            //foreach (var logStep in traceSteps.OrderBy(x => x.Index).ToList().Select(step => new LogStep
            //{
            //    Id = Guid.NewGuid(),
            //    LogEntry = logEntry,
            //    LogEntryId = logEntry.Id,
            //    Index = step.Index,
            //    Metadata = step.Metadata,
            //    StepTimestamp = step.StepTimestamp,
            //    Type = step.Type,
            //    Frame = step.Frame,
            //    Name = step.Name,
            //    Message = step.Message,
            //    Source = step.Source,
            //}))
            //{
            //    logEntry.Steps.Add(logStep);
            //}

            //_context.Save(logEntry);
        }
    }
}