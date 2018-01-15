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
    //public class LogEntryWorkflowMemberValueResolver : IMemberValueResolver<object, object, ICollection<LogStep>, string>
    //{
    //    private const string XmlHeader8 = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";
    //    private const string XmlHeader16 = "<?xml version=\"1.0\" encoding=\"utf-8\"?>";

    //    public string Resolve(object source, object destination, ICollection<LogStep> sourceMember, string destMember,
    //        ResolutionContext context)
    //    {
    //        var traceSteps = sourceMember.OrderBy(x => x.Index).ToList();

    //        var builder = new StringBuilder();
    //        builder.Append("<p style=\"white-space: nowrap;\">Start request </p>");

    //        foreach (var tracestep in traceSteps)
    //        {
    //            builder.Append(
    //                $"<p style=\"white-space: nowrap;\">{string.Format($"From {tracestep.Source} method located in frame {string.Format($"<pre class=\"prettyprint lang-cs\">{tracestep.Frame}</pre>", tracestep.Frame)} {(!string.IsNullOrEmpty(tracestep.Name) ? $" (which processes {tracestep.Name}) " : "")} {(!string.IsNullOrEmpty(tracestep.Message) ? $" (with message {tracestep.Message}) " : "")} \r\n", tracestep.Source, $"<pre class=\"prettyprint lang-cs\">{tracestep.Frame}</pre>", (!string.IsNullOrEmpty(tracestep.Name) ? $" (which processes {tracestep.Name}) " : ""), (!string.IsNullOrEmpty(tracestep.Message) ? $" (with message {tracestep.Message}) " : ""))}</p>");

    //            if (string.IsNullOrEmpty(tracestep.Metadata)) continue;

    //            builder.Append("<p style=\"white-space: nowrap;\">With metadata: </p>");

    //            string beautified;
    //            if (XmlUtils.IsValidXml(tracestep.Metadata))
    //            {
    //                // xml 
    //                // operation metadata is xml in our case
    //                beautified = XmlPrettifyHelper.Prettify(tracestep.Metadata.Replace(XmlHeader8, "").Replace(XmlHeader16, ""));
    //            }
    //            else if (JsonUtils.IsValidJson(tracestep.Metadata))
    //            {
    //                beautified =
    //                    $"<pre class=\"prettyprint lang-json\">{JsonPrettifier.BeautifyJson(tracestep.Metadata)}</pre>";
    //            }
    //            else
    //            {
    //                beautified = tracestep.Metadata;
    //            }

    //            builder.Append(beautified);
    //        }

    //        builder.Append("<p style=\"white-space: nowrap;\">End request </p>");

    //        var traceString = HttpUtility.HtmlEncode(builder.ToString());

    //        return traceString;
    //    }
    //}
}