using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class Block
    {
        public Tile Location;

        public string Sprite => _template.Sprite;
        public bool Active => _template.Active;
        public int Speed => _template.Speed;
        private BlockTemplate _template;
        
        public Block(BlockTemplate template)
        {
            _template = template;
        }

        public void Update(UserInput input)
        {
            _template.RootChip.Process(this, input);
        }
    }
}
