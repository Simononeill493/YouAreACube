using System;
using System.Collections.Generic;

namespace IAmACube
{
    [Serializable()]
    public class TemplateVersionList
    {
        public string Name;
        public BlockTemplate Main;

        public TemplateVersionList(string name) : base()
        {
            _dict = new Dictionary<int, BlockTemplate>();
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }

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
                // set the instance value at index
            }
        }
        private Dictionary<int, BlockTemplate> _dict;
    }
}
