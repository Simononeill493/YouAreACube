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
        public string Sprite;
        public bool Active;
        public int Speed;
        public IChip RootChip;

        public BlockTemplate(string name)
        {
            Name = name;
        }

        public Block Generate()
        {
            var block = new Block(this);
            //todo set any dynamic data here
            return block;
        }
    }
}
