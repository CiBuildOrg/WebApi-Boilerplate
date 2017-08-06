using System.Collections.Generic;
using System.Linq;
using App.Entities;
using App.Security;

namespace App.Api.Security
{
    public class ValidIssuers : AbstractAudience<string>
    {
        public override IEnumerator<string> GetEnumerator()
        {
            return Audiences().Select(issuer => issuer.Secret).GetEnumerator();
        }
    }
}