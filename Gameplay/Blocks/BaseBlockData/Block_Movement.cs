using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public abstract partial class Block
    {
        public BlockMovementData MovementData;

        public bool IsMoving => MovementData.IsMoving;
        public bool IsInCentreOfBlock => MovementData.IsInCentreOfBlock;

        public bool TryMove(RelativeDirection movementDirection)
        {
            return TryMove(DirectionUtils.ToCardinal(Orientation, movementDirection));
        }
        public bool TryMove(CardinalDirection direction)
        {
            var destination = Location.Adjacent[direction];

            if (CanOccupyDestination(destination))
            {
                Move(destination);
                Energy--;
                return true;
            }

            return false;
        }

        protected abstract void Move(Tile destination);


        public bool CanStartMoving()
        {
            return (!IsMoving) & (Energy > 0);
        }

        public abstract bool CanOccupyDestination(Tile destination);

    }
}
