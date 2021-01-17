using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class Tile
    {
        public Dictionary<Direction, Tile> Adjacent;
        public GroundBlock Ground;

        public Tile()
        {
            Adjacent = new Dictionary<Direction, Tile>();
        }
    }
}
