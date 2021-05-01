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
        private Sector _sector;

        public List<Block> MovingBlocks;
        public List<(Block, IntPoint)> MovedOutOfSector;

        private List<(Block, IntPoint)> _toMoveFromSector;
        private HashSet<Block> _toRemove;

        public MoveManager(Sector sector)
        {
            _sector = sector;
            MovingBlocks = new List<Block>();
            MovedOutOfSector = new List<(Block, IntPoint)>();

            _toMoveFromSector = new List<(Block, IntPoint)>();
            _toRemove = new HashSet<Block>();
        }

        public void TryStartMovement(Block block, CardinalDirection direction, int moveTotalTicks)
        {
            if (block.TryGetAdjacent(direction, out Tile destination))
            {
                if (block.CanMoveTo(destination))
                {
                    _startMovement(block, new BlockMovementData(block, destination, moveTotalTicks, direction));
                }
            }
        }
        private void _startMovement(Block block, BlockMovementData movementData)
        {
            block.StartMovement(movementData);

            if(MovingBlocks.Contains(block))
            {
                throw new Exception("Started moving block, but it's already moving");
            }

            MovingBlocks.Add(block);
        }


        public void Tick()
        {
            foreach (var moving in MovingBlocks)
            {
                _tickBlock(moving, moving.MovementData);
            }

            _extractSectorEmmigrants();
            _removeBlocksQueuedForRemoval();
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
        private void _tickBlock(Block block, BlockMovementData movementData)
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

        private void _moveBlockToDestination(Block block)
        {
            block.MoveToCurrentDestination();
            _checkIfEmmigrated(block);
        }
        private void _completeMovement(Block block)
        {
            block.CompleteMovement();
            _toRemove.Add(block);
        }
        private void _abortMovement(Block block)
        {
            block.AbortMovement();
            _toRemove.Add(block);
        }
        private void _checkIfEmmigrated(Block block)
        {
            if (!block.InSector(_sector))
            {
                block.IsMovingBetweenSectors = true;
                _toMoveFromSector.Add((block, block.Location.SectorID));
            }
        }

        public void RemoveMovingBlock(Block block)
        {
            var removed = MovingBlocks.Remove(block);
            if (!removed)
            {
                throw new Exception("Tried to remove a moving block, but it wasn't in the moving list!");
            }
        }

        public void AddMovingBlock(Block block) => MovingBlocks.Add(block);

    }
}
