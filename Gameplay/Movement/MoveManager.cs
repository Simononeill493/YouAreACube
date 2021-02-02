using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class MoveManager
    {
        public List<Block> Moving = new List<Block>();

        public void TickCurrentMoves()
        {
            foreach (var movingBlock in Moving)
            {
                _processMovingBlock(movingBlock);
            }

            _removeFinishedBlocks();
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
