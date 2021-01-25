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

        public void ProcessMoves(List<Tuple<Block,Direction>> toMove)
        {
            foreach(var movingBlock in Moving)
            {
                _processMovingBlock(movingBlock);
            }

            _removeFinishedBlocks();

            foreach (var move in toMove)
            {
                _tryStartMoving(move.Item1, move.Item2);
            }
        }

        private void _tryStartMoving(Block block,Direction direction)
        {
            if (block.Location.Adjacent.ContainsKey(direction) & !block.IsMoving)
            {
                _startMoving(block, direction);
            }
        }

        private void _processMovingBlock(Block block)
        {
            var data = block.MovementData;
            data.MovementPosition++;

            if (data.MovementPosition == 0 | (!data.PastMidpoint & data.Destination.Contents!=null))
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
            block.MovementData = null;
        }

        private void _startMoving(Block block,Direction direction)
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
