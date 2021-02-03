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
        public CardinalDirection Direction;
        public Tile Destination;
        public int MoveSpeed;

        public int MovementPosition;
        public bool IsInCentreOfBlock => (MovementPosition == 0);

        public (int X, int Y) Offset;

        public int Midpoint;
        public bool PastMidpoint;
        public bool AtMidpoint => (MovementPosition == Midpoint);
        public bool MovementComplete => (PastMidpoint & (MovementPosition == 0));

        public BlockMovementData() { }
        public BlockMovementData(Block block,CardinalDirection direction,int moveSpeed)
        {
            Direction = direction;
            PastMidpoint = false;
            MovementPosition = 0;
            MoveSpeed = moveSpeed;

            Destination = block.Location.Adjacent[direction];
            Offset = direction.XYOffset();

            Midpoint = ((moveSpeed + 1) / 2);
        }

        public void MovePastMidpoint()
        {
            PastMidpoint = true;
            MovementPosition = (-MovementPosition) + (MoveSpeed % 2);
        }
    }
}
