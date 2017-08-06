using System;
using System.Collections;
using System.Collections.Generic;
using System.IdentityModel.Tokens;
using App.Entities.Security;
using Microsoft.Owin.Security.DataHandler.Encoder;

namespace App.Security
{
    public abstract class AbstractAudience<T> : IEnumerable<T>
    {
        public Func<IEnumerable<Client>> Audiences { get; set; }

        public abstract IEnumerator<T> GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }
    }
    

    
}
