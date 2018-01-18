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
using App.Services.Contracts;
using AutoMapper;
using App.Entities;
using LinqKit;

namespace App.Services
{
    public class LogService : ILogService
    {
        private const string LogsSortAscendingKey = "asc";
        private const string LogsSortDescendingKey = "desc";

        private const int LogLimit = 2 * 24; // 2 days

        private readonly LogsDatabaseContext _context;
        private readonly INow _now;
        private readonly IConfiguration _helper;
        private readonly IMapper _mapper;

        public LogService(LogsDatabaseContext context, INow now, IConfiguration helper, IMapper mapper)
        {
            _context = context;
            _now = now;
            _helper = helper;
            _mapper = mapper;
        }

        private bool ShouldLog => _helper.GetBool(ConfigurationKeys.ShouldTrace);

        public Tuple<int, List<TraceViewModel>> GetTraces(TraceSearchRequest request)
        {
            // get logs newer than 7 days
            var nowUtc = _now.UtcNow;

            var traces =
                _context.AsQueryable<Trace>()
                    .Where(x => DbFunctions.AddHours(x.RequestTimestamp, LogLimit) > nowUtc)
                    .AsQueryable();

            IOrderedQueryable<Trace> pagedTraces;

            if (!string.IsNullOrEmpty(request.Search))
            {
                var searchLikeExpression = $"%{request.Search}%";
                Expression<Func<Trace, bool>> searchExpression =
                    entry => SqlFunctions.PatIndex(searchLikeExpression, entry.Url) > 0 ||
                             SqlFunctions.PatIndex(searchLikeExpression, entry.RequestPayload) > 0 ||
                             SqlFunctions.PatIndex(searchLikeExpression, entry.ResponsePayload) > 0;
                traces = traces.Where(searchExpression).AsQueryable();
            }

            switch (request.Order)
            {
                case LogsSortAscendingKey:
                    pagedTraces = traces.OrderAscending(request.Sort);
                    break;
                case LogsSortDescendingKey:
                    pagedTraces = traces.OrderDescending(request.Sort);
                    break;
                default:
                    pagedTraces = traces.OrderDescending(request.Sort);
                    break;
            }

            var totalTraces = pagedTraces.Count();
            var pagedTracesList = pagedTraces
                .Skip(request.Offset / request.Limit * request.Limit).Take(request.Limit).ToList();

            return new Tuple<int, List<TraceViewModel>>(totalTraces, _mapper.Map<List<TraceViewModel>>(pagedTracesList));
        }
    }
}