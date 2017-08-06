using System;
using System.Collections;
using System.Collections.Generic;
using App.Entities.Security;

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
