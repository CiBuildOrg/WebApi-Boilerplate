using System.Collections.Generic;
using System.Globalization;
using App.Dto.Response;
using App.Entities;
using AutoMapper;

namespace App.Services.Mappings.Profiles
{
    public class LogEntryProfile : Profile
    {
        public LogEntryProfile(IMemberValueResolver<object, object, ICollection<LogStep>, string> custom)
        {
            CreateMap<LogEntry, TraceViewModel>()
                .ForMember(destination => destination.Duration,
                    options => options.MapFrom(
                        source => (source.ResponseTimestamp.HasValue && source.RequestTimestamp.HasValue)
                            ? $"{(source.ResponseTimestamp.Value - source.RequestTimestamp.Value).TotalSeconds:#.##} seconds"
                            : "Duration not captured"))
                .ForMember(destination => destination.Timestamp,
                    options => options.MapFrom(
                        source => source.Timestamp.ToString(CultureInfo.InvariantCulture)))
                .ForMember(destination => destination.Uri, options => options.MapFrom(source => source.RequestUri))
                .ForMember(destination => destination.Workflow,
                    options => options.ResolveUsing(custom, x => x.Steps));
        }
    }
}
