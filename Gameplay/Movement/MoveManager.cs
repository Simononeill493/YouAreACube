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
                if(!move.Item1.IsMoving)
                {
                    _startMoving(move.Item1, move.Item2);
                }
            }
        }

        private void _removeFinishedBlocks()
        {
            Moving.RemoveAll(b => !b.IsMoving);
        }

        private void _processMovingBlock(Block block)
        {
            var data = block.MovementData;
            data.MovementPosition++;
            data.TestLength++;

            if (data.MovementPosition == 0)
            {
                block.IsMoving = false;
                Console.WriteLine(data.TestLength);
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

        private void _startMoving(Block block,Direction direction)
        {
            var movementData = new BlockMovementData();
            movementData.Direction = direction;
            movementData.Midpoint = ((block.Speed + 1) / 2);
            movementData.PastMidpoint = false;
            movementData.MovementPosition = 0;

            var offs = DirectionUtils.XYOffset[direction];
            movementData.XOffset = offs.Item1;
            movementData.YOffset = offs.Item2;

            block.MovementData = movementData;
            block.IsMoving = true;
            Moving.Add(block);
        }
    }
}
