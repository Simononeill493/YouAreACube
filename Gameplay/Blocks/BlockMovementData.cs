﻿using Microsoft.Xna.Framework;
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

        public int TotalTicks;
        public int CurrentTick;

        public bool AtMidpoint;
        public bool PastMidpoint;

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
            Finished = (CurrentTick == TotalTicks);
        }

        public float GetMovePercentage()
        {
            var percent = ((CurrentTick) / (float)(TotalTicks));

            if(PastMidpoint)
            {
                percent = -(1-percent);
            }

            return percent;
        }
    }
}
