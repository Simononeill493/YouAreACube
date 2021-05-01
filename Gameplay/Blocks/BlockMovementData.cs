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

        public IntPoint MovementOffset;
        public FloatPoint SingleStepOffset;
        public float MovementOffsetPercentage;

        public int TotalTicks;
        public int CurrentTick;

        public bool AtMidpoint;
        public bool PassedMidpoint;
        public bool Moved;

        public bool Finished;
        public bool Cancelled;

        private int _midpoint;

        public BlockMovementData(Block mover,Tile destination,int moveTotalTicks,CardinalDirection direction)
        {
            Origin = mover.Location;
            Destination = destination;
            TotalTicks = moveTotalTicks;

            MovementOffset = direction.XYOffset();
            SingleStepOffset = (MovementOffset * (GetMovePercentage(1)));

            CurrentTick = 0;

            _midpoint = TotalTicks / 2;
        }

        public void Tick()
        {
            if(Finished)
            {
                throw new Exception("Finished block is ticking");
            }

            CurrentTick++;
            AtMidpoint = (CurrentTick >= _midpoint) & (!PassedMidpoint);

            PassedMidpoint |= AtMidpoint;
            Moved |= AtMidpoint;

            Finished = (CurrentTick == TotalTicks);

            MovementOffsetPercentage = GetMovePercentage(CurrentTick);
        }

        public float GetMovePercentage(int tick)
        {
            var percent = tick / (float)TotalTicks;

            if(Moved)
            {
                percent--;
            }

            return percent;
        }
    }
}
