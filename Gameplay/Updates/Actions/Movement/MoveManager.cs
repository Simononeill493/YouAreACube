using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class MoveManager
    {
        public List<(Cube, IntPoint)> MovedOutOfSector;
        private List<(Cube, IntPoint)> _toMoveFromSector;

        private List<Cube> _movingBlocks;
        private HashSet<Cube> _toRemove;

        private Sector _sector;

        public MoveManager(Sector sector)
        {
            _sector = sector;
            _movingBlocks = new List<Cube>();
            MovedOutOfSector = new List<(Cube, IntPoint)>();

            _toMoveFromSector = new List<(Cube, IntPoint)>();
            _toRemove = new HashSet<Cube>();
        }

        public void TryStartMovement(Cube block, CardinalDirection direction, int moveTotalTicks)
        {
            if (block.TryGetAdjacent(direction, out Tile destination))
            {
                if (block.CanMoveTo(destination))
                {
                    _startMovement(block, new CubeMovementData(block, destination, moveTotalTicks, direction));
                }
            }
        }
        private void _startMovement(Cube block, CubeMovementData movementData)
        {
            block.StartMovement(movementData);

            if(_movingBlocks.Contains(block))
            {
                throw new Exception("Started moving block, but it's already moving");
            }

            _movingBlocks.Add(block);
        }

        public void AddMovingBlock(Cube block) => _movingBlocks.Add(block);
        public void RemoveMovingBlock(Cube block)
        {
            var removed = _movingBlocks.Remove(block);
            if (!removed)
            {
                throw new Exception("Tried to remove a moving block, but it wasn't in the moving list!");
            }
        }

        public void Tick()
        {
            foreach (var moving in _movingBlocks)
            {
                _tickBlock(moving, moving.MovementData);
            }

            _extractSectorEmmigrants();
            _removeBlocksQueuedForRemoval();
        }
        private void _tickBlock(Cube block, CubeMovementData movementData)
        {
            if (block.ShouldAbortMovement())
            {
                _abortMovement(block);
                return;
            }

            movementData.Tick();

            if (movementData.AtMidpoint)
            {
                _moveBlockToDestination(block);
            }
            if (movementData.Finished)
            {
                _completeMovement(block);
            }

            block.IsMovingThroughCentre = movementData.Finished;
        }
        private void _moveBlockToDestination(Cube block)
        {
            block.MoveToCurrentDestination();
            _checkIfEmmigrated(block);

            //Console.WriteLine(block._id + " moved to tile " + block.MovementData.Destination.AbsoluteLocation);
        }
        private void _completeMovement(Cube block)
        {
            block.CompleteMovement();
            _toRemove.Add(block);
        }
        private void _abortMovement(Cube block)
        {
            block.AbortMovement();
            _toRemove.Add(block);
        }

        private void _removeBlocksQueuedForRemoval()
        {
            foreach (var toRemove in _toRemove)
            {
                RemoveMovingBlock(toRemove);
            }
            _toRemove.Clear();
        }
        private void _extractSectorEmmigrants()
        {
            MovedOutOfSector.AddRange(_toMoveFromSector);
            _toMoveFromSector.Clear();
        }
        private void _checkIfEmmigrated(Cube block)
        {
            if (!block.InSector(_sector))
            {
                block.IsMovingBetweenSectors = true;
                _toMoveFromSector.Add((block, block.Location.SectorID));
            }
        }
    }
}
