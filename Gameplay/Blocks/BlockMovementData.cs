using Microsoft.Xna.Framework;
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
        public bool IsMoving;

        public CardinalDirection Direction;
        public Tile Destination;
        public int MoveSpeed;

        public int MovementPosition;
        public bool IsInCentreOfBlock => (MovementPosition == 0);

        public Point Offset;

        public int Midpoint;
        public bool PastMidpoint;
        public bool AtMidpoint => (MovementPosition == Midpoint);
        public bool MovementComplete => (PastMidpoint & (MovementPosition == 0));

        public void StartMoving(Block block, CardinalDirection direction, int moveSpeed)
        {
            IsMoving = true;
            Direction = direction;
            PastMidpoint = false;
            MovementPosition = 0;
            MoveSpeed = moveSpeed;

            Destination = block.Location.Adjacent[direction];
            Offset = direction.XYOffset();

            Midpoint = ((moveSpeed + 1) / 2);
        }
        public void StopMoving()
        {
            IsMoving = false;
            Destination = null;
            MovementPosition = 0;
        }

        public void MovePastMidpoint()
        {
            PastMidpoint = true;
            MovementPosition = (-MovementPosition) + (MoveSpeed % 2);
        }
    }
}
