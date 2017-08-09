using App.Core.Contracts;

namespace App.Core.Implementations
{
    public class TraceStepUtil : ITraceStepUtil
    {
        private readonly IResolver _resolver;

        public TraceStepUtil(IResolver resolver)
        {
            _resolver = resolver;
        }

        public ITraceProvider Get()
        {
            return _resolver.Resolve<ITraceProvider>();
        }
    }
}