using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public abstract partial class Block
    {
        public bool IsMoving;
        public bool IsMovingThroughCentre;
        public float MovementOffsetPercentage;
        public Point MovementOffset;

        public bool IsInCentreOfBlock => (!IsMoving) | (IsMovingThroughCentre);

        public abstract void Move(BlockMovementData movementData);
        public virtual void StartMovement(BlockMovementData movementData) { TakeEnergy(1); }
        public virtual void EndMovement(BlockMovementData movementData) { }

        public bool CanMoveTo(Tile destination) => CanStartMoving() & CanOccupyDestination(destination);
        public bool CanStartMoving() => (!IsMoving) & Energy > 0;
        public virtual bool CanOccupyDestination(Tile destination) => true;
    }
}
