using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using App.Core.Utils;
using App.Entities;
using App.Services.Resources;
using AutoMapper;

namespace App.Services.Mappings.ValueResolvers
{
    public class LogEntryWorkflowMemberValueResolver : IMemberValueResolver<object, object, Trace, string>
    {
        private const string XmlHeader8 = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
        private const string XmlHeader16 = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

        public string Resolve(object source, object destination, Trace sourceMember, string destMember,
            ResolutionContext context)
        {
            var builder = new StringBuilder();
            builder.Append("<p style=\"white-space: nowrap;\">Start request </p>");
            builder.Append($"<p style=\"white-space: nowrap;\">Identity {sourceMember.CallerIdentity}, Url {sourceMember.Url}, Verb {sourceMember.Verb}, Status {sourceMember.StatusCode}, Duration {sourceMember.CallDuration}</p>");
            builder.Append($"<p style=\"white-space: nowrap;\">With request headers {sourceMember.RequestHeaders}</p>");
            if (!string.IsNullOrEmpty(sourceMember.RequestPayload))
            {

                builder.Append($"<p style=\"white-space: nowrap;\">With request payload: </p>");
                string beautified = (JsonUtils.IsValidJson(sourceMember.RequestPayload)
                    ? $"<pre class=\"prettyprint lang-json\">{JsonPrettifier.BeautifyJson(sourceMember.RequestPayload)}</pre>"
                    : sourceMember.RequestPayload);

                builder.Append(beautified);
            }

            builder.Append($"<p style=\"white-space: nowrap;\">With response headers {sourceMember.ResponseHeaders}</p>");

            if (!string.IsNullOrEmpty(sourceMember.ResponsePayload))
            {

                builder.Append($"<p style=\"white-space: nowrap;\">With request payload: </p>");
                string beautified = (JsonUtils.IsValidJson(sourceMember.ResponsePayload)
                    ? $"<pre class=\"prettyprint lang-json\">{JsonPrettifier.BeautifyJson(sourceMember.ResponsePayload)}</pre>"
                    : sourceMember.ResponsePayload);

                builder.Append(beautified);
            }

            builder.Append("<p style=\"white-space: nowrap;\">End request </p>");
            return builder.ToString();
        }
    }
}