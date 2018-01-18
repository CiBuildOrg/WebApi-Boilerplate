using System.Globalization;
using App.Dto.Response;
using App.Entities;
using AutoMapper;

namespace App.Services.Mappings.Profiles
{
    public class TraceProfile : Profile
    {
        public TraceProfile(IMemberValueResolver<object, object, Trace, string> custom)
        {
            CreateMap<Trace, TraceViewModel>()
                .ForMember(destination => destination.Duration,
                    options => options.MapFrom(
                        source => source.CallDuration))
                .ForMember(destination => destination.Timestamp,
                    options => options.MapFrom(
                        source => source.RequestTimestamp.ToString(CultureInfo.InvariantCulture)))
                .ForMember(destination => destination.Uri, options => options.MapFrom(source => source.Url))
                .ForMember(destination => destination.Workflow,
                    options => options.ResolveUsing(custom, x => x));
        }
    }
}
