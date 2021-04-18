using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    [Serializable()]
    public class TemplateAllVersions
    {
        public string Name;
        public BlockTemplate Main;

        public static TemplateAllVersions Create(string name,BlockTemplate initial)
        {
            var created = new TemplateAllVersions(name);
            created[0] = initial;
            created.Main = initial;
            return created;
        }

        private TemplateAllVersions(string name) : base()
        {
            _dict = new Dictionary<int, BlockTemplate>();
            Name = name;
        }



        public override string ToString()
        {
            return Name;
        }

        public int GetNewVersionNumber() => _dict.Keys.Max() + 1;
        public Dictionary<int, BlockTemplate>.ValueCollection Values => _dict.Values;
        public BlockTemplate this[int index]
        {
            get
            {
                return _dict[index];
                // get the instance value from index
            }
            set
            {
                _dict[index] = value;
                value.Version = index;
                value.Versions = this;
                // set the instance value at index
            }
        }
        private Dictionary<int, BlockTemplate> _dict;


    }
}
