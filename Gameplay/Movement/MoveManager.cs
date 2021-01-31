using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class MoveManager
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

        public void ProcessNewMoveRequest(Block block, MovementDirection direction)
        {
            var cardinal = DirectionUtils.ToCardinal(block.Orientation, direction);
            ProcessNewMoveRequest(block, cardinal);
        }
        public void ProcessNewMoveRequest(Block block,CardinalDirection direction)
        {
            _tryStartMoving(block, direction);
        }

        private void _tryStartMoving(Block block,CardinalDirection direction)
        {
            if (block.Location.DirectionIsValid(direction) & block.CanStartMoving())
            {
                _startMoving(block, direction);
            }
        }

        private void _processMovingBlock(Block block)
        {
            var data = block.MovementData;
            data.MovementPosition++;

            if (data.MovementPosition == 0 | (!data.PastMidpoint & !block.CanOccupyDestination(data.Destination)))
            {
                _resetBlockMovement(block);
            }
            else
            {
                if (data.MovementPosition == data.Midpoint)
                {
                    block.Move(block.MovementData.Direction);
                    data.PastMidpoint = true;
                    data.MovementPosition = (-data.MovementPosition)+(block.Speed%2);
                }
            }
        }

        private void _resetBlockMovement(Block block)
        {
            block.IsMoving = false;
        }

        private void _startMoving(Block block,CardinalDirection direction)
        {
            var movementData = new BlockMovementData();
            movementData.Direction = direction;
            movementData.Midpoint = ((block.Speed + 1) / 2);
            movementData.PastMidpoint = false;
            movementData.MovementPosition = 0;
            movementData.Destination = block.Location.Adjacent[direction];

            var offs = DirectionUtils.XYOffset[direction];
            movementData.XOffset = offs.Item1;
            movementData.YOffset = offs.Item2;

            block.MovementData = movementData;
            block.IsMoving = true;
            Moving.Add(block);
        }


        private void _removeFinishedBlocks()
        {
            Moving.RemoveAll(b => !b.IsMoving);
        }
    }
}
