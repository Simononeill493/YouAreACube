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
        public Dictionary<Block,BlockMovementData> MovingBlocks;
        public List<(Block, BlockMovementData, Point)> MovedOutOfSector;

        private List<(Block, BlockMovementData, Point)> _toMoveFromSector;
        private List<Block> _toRemove;
        private Sector _sector;

        public MoveManager(Sector sector)
        {
            _sector = sector;
            MovingBlocks = new Dictionary<Block, BlockMovementData>();
            MovedOutOfSector = new List<(Block, BlockMovementData, Point)>();

            _toMoveFromSector = new List<(Block, BlockMovementData, Point)>();
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
                _tickBlock(moving.Key, moving.Value);
            }

            _removeBlocksQueuedForRemoval();
            _removeSectorEmmigrants();
        }

        private void _tickBlock(Block block, BlockMovementData movementData)
        {
            movementData.Tick();

            if(!movementData.PastMidpoint & !block.CanOccupyDestination(movementData.Destination))
            {
                movementData.Cancelled = true;
            }

            if(movementData.Cancelled)
            {
                _cancelMovement(block, movementData);
            }
            if (movementData.AtMidpoint)
            {
                _moveToDestination(block, movementData);
            }
            else if(movementData.Finished)
            {
                _completeMovement(block, movementData);
            }

            block.IsMovingThroughCentre = movementData.Finished;
            block.MovementOffsetPercentage = movementData.GetMovePercentage();
        }

        private void _startMovement(Block block, BlockMovementData movementData)
        {
            block.IsMoving = true;
            block.StartMovement(movementData);
            block.MovementOffset = movementData.MovementOffset;

            MovingBlocks[block] = movementData;
        }
        private void _moveToDestination(Block block, BlockMovementData movementData)
        {
            block.Move(movementData);
            if (!block.InSector(_sector))
            {
                _toMoveFromSector.Add((block, movementData,block.Location.SectorID));
            }

            movementData.PastMidpoint = true;
        }
        private void _completeMovement(Block block, BlockMovementData movementData)
        {
            block.IsMoving = false;
            block.EndMovement(movementData);
            _toRemove.Add(block);
        }
        private void _cancelMovement(Block block, BlockMovementData movementData)
        {
            _toRemove.Add(block);
            block.IsMoving = false; 
        }

        public void Destroy(Block block)
        {
            Remove(block);
        }
        public void Remove(Block block)
        {
            MovingBlocks.Remove(block);
        }
        public void AddFromOutOfSector(Block block, BlockMovementData data)
        {
            MovingBlocks[block] = data;
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
    }
}
