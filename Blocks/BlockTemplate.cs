using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class BlockTemplate
    {
        public string Name;
        private Block _template;

        public BlockTemplate(string name,Block template)
        {
            Name = name;
            _template = template;
        }

        public Block Generate()
        {
            return _template.Clone();
        }
    }
}
