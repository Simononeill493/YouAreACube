using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class CubeMovementData
    {
        public readonly Tile Origin;
        public readonly Tile Destination;
        public readonly IntPoint MovementOffset;
        private readonly int _totalTicks;

        public float MovementOffsetPercentage { get; private set; }
        public bool AtMidpoint { get; private set; }
        public bool Moved { get; private set; }
        public bool Finished { get; private set; }

        private int _currentTick;
        private int _midpoint;
        private bool _passedMidpoint;

        public CubeMovementData(Cube mover,Tile destination,int moveTotalTicks,CardinalDirection direction)
        {
            Origin = mover.Location;
            Destination = destination;
            MovementOffset = direction.XYOffset();

            _totalTicks = moveTotalTicks;
            _currentTick = 0;

            _midpoint = _totalTicks / 2;
        }

        public void Tick()
        {
            if(Finished)
            {
                throw new Exception("Finished block is ticking");
            }

            _currentTick++;
            AtMidpoint = (_currentTick >= _midpoint) & (!_passedMidpoint);

            _passedMidpoint |= AtMidpoint;
            Moved |= AtMidpoint;

            Finished = (_currentTick == _totalTicks);

            MovementOffsetPercentage = GetMovePercentage(_currentTick);
        }
        public float GetMovePercentage(int tick)
        {
            var percent = tick / (float)_totalTicks;

            if(Moved)
            {
                percent--;
            }

            return percent;
        }
    }
}
