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
        public List<(Block, IntPoint)> MovedOutOfSector;
        private List<(Block, IntPoint)> _toMoveFromSector;

        private List<Block> _movingBlocks;
        private HashSet<Block> _toRemove;

        private Sector _sector;

        public MoveManager(Sector sector)
        {
            _sector = sector;
            _movingBlocks = new List<Block>();
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

            if(_movingBlocks.Contains(block))
            {
                throw new Exception("Started moving block, but it's already moving");
            }

            _movingBlocks.Add(block);
        }

        public void AddMovingBlock(Block block) => _movingBlocks.Add(block);
        public void RemoveMovingBlock(Block block)
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

            //Console.WriteLine(block._id + " moved to tile " + block.MovementData.Destination.AbsoluteLocation);
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
        private void _checkIfEmmigrated(Block block)
        {
            if (!block.InSector(_sector))
            {
                block.IsMovingBetweenSectors = true;
                _toMoveFromSector.Add((block, block.Location.SectorID));
            }
        }
    }
}
