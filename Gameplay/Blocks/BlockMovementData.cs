using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class BlockMovementData
    {
        public Direction Direction;
        public Tile Destination;

        public int Midpoint;
        public int MovementPosition;
        public int XOffset;
        public int YOffset;
        public bool PastMidpoint;
    }
}
