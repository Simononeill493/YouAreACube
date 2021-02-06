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
        public Sector _sector;

        public List<Block> Moving = new List<Block>();
        public List<(Block,Point)> _toMoveFromSector = new List<(Block, Point)>();
        public List<(Block, Point)> MovedOutOfSector = new List<(Block, Point)>();

        public MoveManager(Sector sector)
        {
            _sector = sector;
        }

        public void TickCurrentMoves()
        {
            foreach (var movingBlock in Moving)
            {
                _processMovingBlock(movingBlock);
            }

            _moveBlocksFromSector();
            _removeFinishedBlocks();
        }

        public void AddMovingBlockFromOtherSector(Block block)
        {
            Moving.Add(block);
        }

        public void _moveBlocksFromSector()
        {
            foreach(var toMove in _toMoveFromSector)
            {
                _sector.RemoveFromSectorLists(toMove.Item1);

                Moving.Remove(toMove.Item1);
                MovedOutOfSector.Add(toMove);
            }

            _toMoveFromSector.Clear();
        }

        public void TryStartMoving(Block block, RelativeDirection direction,int moveSpeed)
        {
            var cardinal = DirectionUtils.ToCardinal(block.Orientation, direction);
            TryStartMoving(block, cardinal,moveSpeed);
        }
        public void TryStartMoving(Block block,CardinalDirection direction, int moveSpeed)
        {
            if (block.Location.DirectionIsValid(direction) & block.CanStartMoving())
            {
                _startMovement(block, direction,moveSpeed);
            }
        }

        public void ManuallyCancelMovement(Block block)
        {
            if(block.IsMoving)
            {
                Moving.Remove(block);
            }
        }

        private void _processMovingBlock(Block block)
        {
            var data = block.MovementData;
            data.MovementPosition++;

            if (_shouldCancelMovementEarly(block) | data.MovementComplete)
            {
                _endMovement(block);
            }
            else if (data.AtMidpoint)
            {
                if(block.TryMove(block.MovementData.Direction))
                {
                    data.MovePastMidpoint();

                    if(!block.InSector(_sector))
                    {
                        _toMoveFromSector.Add((block,block.Location.SectorID));
                    }
                }
                else
                {
                    _endMovement(block);
                }
            }
        }


        private void _startMovement(Block block, CardinalDirection direction, int moveSpeed)
        {
            block.IsMoving = true;

            block.MovementData = new BlockMovementData(block, direction, moveSpeed);
            Moving.Add(block);
        }
        private void _endMovement(Block block)
        {
            block.IsMoving = false;
        }

        private bool _shouldCancelMovementEarly(Block block)
        {
            return !block.MovementData.PastMidpoint & !block.CanOccupyDestination(block.MovementData.Destination);
        }
        private void _removeFinishedBlocks()
        {
            Moving.RemoveAll(b => !b.IsMoving);
        }
    }
}
