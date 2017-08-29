using System;
using System.Collections.Generic;
using System.Linq;

namespace App.Dto.Request
{
    public class FormData
    {
        private List<ValueFile> _files;
        private List<ValueString> _fields;

        public List<ValueFile> Files
        {
            get => _files ?? (_files = new List<ValueFile>());
            set => _files = value;
        }

        public List<ValueString> Fields
        {
            get => _fields ?? (_fields = new List<ValueString>());
            set => _fields = value;
        }

        public IEnumerable<string> AllKeys()
        {
            return Fields.Select(m => m.Name).Union(Files.Select(m => m.Name));
        }

        public void Add(string name, string value)
        {
            Fields.Add(new ValueString() { Name = name, Value = value});
        }

        public void Add(string name, HttpFile value)
        {
            Files.Add(new ValueFile() { Name = name, Value = value });
        }

        public bool TryGetValue(string name, out string value)
        {
            var field = Fields.FirstOrDefault(m => String.Equals(m.Name, name, StringComparison.CurrentCultureIgnoreCase));
            if (field != null)
            {
                value = field.Value;
                return true;
            }
            value = null;
            return false;
        }

        public bool TryGetValue(string name, out HttpFile value)
        {
            var field = Files.FirstOrDefault(m => String.Equals(m.Name, name, StringComparison.CurrentCultureIgnoreCase));
            if (field != null)
            {
                value = field.Value;
                return true;
            }
            value = null;
            return false;
        }
    }
}
