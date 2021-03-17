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
        public Tile Origin;
        public Tile Destination;

        public Point MovementOffset;
        public float MovementOffsetPercentage;

        public int TotalTicks;
        public int CurrentTick;

        public bool AtMidpoint;
        public bool Moved;

        public bool Finished;
        public bool Cancelled;

        private int _midpoint;

        public BlockMovementData(Block mover,Tile destination,int moveTotalTicks,CardinalDirection direction)
        {
            Origin = mover.Location;
            Destination = destination;

            MovementOffset = direction.XYOffset();
            TotalTicks = moveTotalTicks;
            CurrentTick = 0;

            _midpoint = TotalTicks / 2;
        }

        public void Tick()
        {
            CurrentTick++;
            AtMidpoint = (CurrentTick == _midpoint);
            Moved = (Moved | AtMidpoint);
            Finished = (CurrentTick == TotalTicks);

            MovementOffsetPercentage = GetMovePercentage();
        }

        private float GetMovePercentage()
        {
            var percent = ((CurrentTick) / (float)(TotalTicks));

            if(Moved)
            {
                percent = -(1-percent);
            }

            return percent;
        }
    }
}
