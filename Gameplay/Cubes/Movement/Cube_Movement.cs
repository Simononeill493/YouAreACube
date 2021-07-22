using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public abstract partial class Cube
    {
        public bool IsMoving;
        public bool IsMovingThroughCentre;
        public bool IsMovingBetweenSectors;
        public bool IsInCentreOfTile => (!IsMoving) | (IsMovingThroughCentre);

        public CubeMovementData MovementData { get; private set; }

        public void StartMovement(CubeMovementData movementData) 
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
        public void MoveToCurrentDestination() => EnterLocation(MovementData.Destination);
        public abstract void EnterLocation(Tile destination);

        public bool TryGetAdjacent(CardinalDirection cardinalDirection, out Tile destination) => Location.TryGetNeighbour(cardinalDirection, out destination);

        public bool ShouldAbortMovement() => ToBeDeleted() | (!MovementData.Moved & (!CanOccupyDestination(MovementData.Destination)));
        public bool CanMoveTo(Tile destination) => CanStartMoving() & CanOccupyDestination(destination);
        public bool CanStartMoving() => (!IsMoving) & Energy > 0;
        public abstract bool CanOccupyDestination(Tile destination);
    }
}
