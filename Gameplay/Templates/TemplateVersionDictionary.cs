using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    [Serializable()]
    public class TemplateVersionDictionary
    {
        public string Name { get; private set; }
        public CubeTemplate Main { get; set; }
        public List<CubeTemplate> Versions => _dict.Values.ToList();

        public TemplateVersionDictionary(string name, CubeTemplate initial)
        {
            Name = name;
            _dict = new Dictionary<int, CubeTemplate>();

            this[0] = initial;
            this.Main = initial;
        }

        public TemplateVersionDictionary GetRuntimeVersion()
        {
            Templates.Database.TryGetValue(Name,out TemplateVersionDictionary value);
            return value;
        }

        public int GetNewVersionNumber()
        {
            return _dict.Keys.Max() + 1;
        }

        public override string ToString() => Name;

        #region dictionary
        public CubeTemplate this[int index]
        {
            get => _dict[index];
            set
            {
                _dict[index] = value;
                value.Version = index;
                value.Versions = this;
            }
        }
        private Dictionary<int, CubeTemplate> _dict;

        public bool ContainsKey(int key) => _dict.ContainsKey(key);
        #endregion
    }
}
