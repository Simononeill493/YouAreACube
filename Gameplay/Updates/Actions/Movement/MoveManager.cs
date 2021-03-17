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
        public List<(Block, Point)> MovedOutOfSector;

        private List<(Block, Point)> _toMoveFromSector;
        private List<Block> _toRemove;

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
            if (block.TryGetAdjacent(direction, out destination))
            {
                if (block.CanMoveTo(destination))
                {
                    _startMovement(block, new BlockMovementData(block, destination, moveTotalTicks, direction));
                }
            }
        }

        public void Tick()
        {
            foreach (var moving in MovingBlocks)
            {
                _tickBlock(moving, moving.MovementData);
            }

            _removeBlocksQueuedForRemoval();
            _extractSectorEmmigrants();
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
                _moveToDestination(block);
                _checkIfEmmigrated(block);
            }
            else if (movementData.Finished)
            {
                _completeMovement(block);
            }

            block.IsMovingThroughCentre = movementData.Finished;
        }

        private void _startMovement(Block block, BlockMovementData movementData)
        {
            block.StartMovement(movementData);
            MovingBlocks.Add(block);
        }
        private void _moveToDestination(Block block) => block.MoveToCurrentDestination();
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
                _toMoveFromSector.Add((block, block.Location.SectorID));
            }
        }

        private void _removeFromMovingList(Block block) => MovingBlocks.Remove(block);
        private void _removeBlocksQueuedForRemoval()
        {
            foreach (var toRemove in _toRemove)
            {
                _removeFromMovingList(toRemove);
            }
            _toRemove.Clear();
        }
        private void _extractSectorEmmigrants()
        {
            foreach (var toMove in _toMoveFromSector)
            {
                _removeFromMovingList(toMove.Item1);
                MovedOutOfSector.Add(toMove);
            }

            _toMoveFromSector.Clear();
        }

        public void DestroyBlock(Block block) => _removeFromMovingList(block);
        public void AddFromOutOfSector(Block block) => MovingBlocks.Add(block);
    }
}
