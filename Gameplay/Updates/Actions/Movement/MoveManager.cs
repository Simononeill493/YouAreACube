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
        public List<Block> MovingBlocks;
        public List<(Block, Point)> MovedOutOfSector;

        private List<(Block, Point)> _toMoveFromSector;
        private List<Block> _toRemove;
        private Sector _sector;

        public MoveManager(Sector sector)
        {
            _sector = sector;
            MovingBlocks = new List<Block>();
            MovedOutOfSector = new List<(Block, Point)>();

            _toMoveFromSector = new List<(Block, Point)>();
            _toRemove = new List<Block>();
        }

        public void TryStartMovement(Block block, CardinalDirection direction, int moveTotalTicks)
        {
            Tile destination;
            if(block.Location.Adjacent.TryGetValue(direction,out destination))
            {
                if (block.CanMoveTo(destination))
                {
                    _startMovement(block, new BlockMovementData(block, destination, moveTotalTicks, direction));
                }
            }
        }
        public void Tick()
        {
            foreach(var moving in MovingBlocks)
            {
                _tickBlock(moving, moving.MovementData);
            }

            _removeBlocksQueuedForRemoval();
            _removeSectorEmmigrants();
        }

        private void _tickBlock(Block block,BlockMovementData movementData)
        {
            if(_blockCannotOccupyDestination(block,movementData) | movementData.Cancelled)
            {
                _cancelMovement(block, movementData);
                return;
            }

            movementData.Tick();

            if (movementData.AtMidpoint)
            {
                _moveToDestination(block, movementData);
                movementData.ManualMovedFlag = true;
            }
            else if(movementData.Finished)
            {
                _completeMovement(block, movementData);
            }

            block.IsMovingThroughCentre = movementData.Finished;
        }

        private void _startMovement(Block block, BlockMovementData movementData)
        {
            block.StartMovement(movementData);
            MovingBlocks.Add(block);
        }
        private void _moveToDestination(Block block, BlockMovementData movementData)
        {
            block.MoveToCurrentDestination();

            if (!block.InSector(_sector))
            {
                _toMoveFromSector.Add((block,block.Location.SectorID));
            }
        }
        private void _completeMovement(Block block, BlockMovementData movementData)
        {
            block.CompleteMovement();
            _toRemove.Add(block);
        }
        private void _cancelMovement(Block block, BlockMovementData movementData)
        {
            block.CancelMovement();
            _toRemove.Add(block);
        }

        public void Destroy(Block block)
        {
            Remove(block);
        }
        public void Remove(Block block)
        {
            MovingBlocks.Remove(block);
        }
        public void AddFromOutOfSector(Block block)
        {
            MovingBlocks.Add(block);
        }

        private void _removeBlocksQueuedForRemoval()
        {
            foreach (var toRemove in _toRemove)
            {
                Remove(toRemove);
            }
            _toRemove.Clear();
        }
        private void _removeSectorEmmigrants()
        {
            foreach (var toMove in _toMoveFromSector)
            {
                Remove(toMove.Item1);
                MovedOutOfSector.Add(toMove);
            }

            _toMoveFromSector.Clear();
        }

        private bool _blockCannotOccupyDestination(Block block, BlockMovementData movementData)
        {
            return !movementData.Moved & !block.CanOccupyDestination(movementData.Destination);
        }
    }
}
