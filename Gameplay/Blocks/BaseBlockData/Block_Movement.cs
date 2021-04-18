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

        public bool IsInCentreOfBlock => (!IsMoving) | (IsMovingThroughCentre);

        public BlockMovementData MovementData { get; private set; }

        public void StartMovement(BlockMovementData movementData) 
        {
            MovementData = movementData;
            IsMoving = true;

            TakeEnergy(1);
        }
        public void CompleteMovement() 
        {
            MovementData = null;
            IsMoving = false;
        }
        public void AbortMovement()
        {
            MovementData = null;
            IsMoving = false;
            IsMovingThroughCentre = false;
        }

        public bool TryGetAdjacent(CardinalDirection cardinalDirection, out Tile destination) => Location.Adjacent.TryGetValue(cardinalDirection, out destination);

        public bool ShouldAbortMovement() => (!MovementData.Moved & (!CanOccupyDestination(MovementData.Destination) | MovementData.Cancelled));

        public bool CanMoveTo(Tile destination) => CanStartMoving() & CanOccupyDestination(destination);
        public bool CanStartMoving() => (!IsMoving) & Energy > 0;
        public virtual bool CanOccupyDestination(Tile destination) => true;
        public abstract void EnterLocation(Tile destination);

        public void MoveToCurrentDestination() => EnterLocation(MovementData.Destination);
    }
}
