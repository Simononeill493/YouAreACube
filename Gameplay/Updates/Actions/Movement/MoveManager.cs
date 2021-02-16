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
        public List<(Block, Point)> MovedOutOfSector = new List<(Block, Point)>();
        private List<(Block, Point)> _toMoveFromSector = new List<(Block, Point)>();

        private List<Block> _moving = new List<Block>();
        private Sector _sector;

        public MoveManager(Sector sector)
        {
            _sector = sector;
        }

        public void Tick()
        {
            foreach (var movingBlock in _moving)
            {
                _processMovingBlock(movingBlock);
            }

            _moveBlocksFromSector();
            _removeFinishedBlocks();
        }

        public void TryStartMoving(Block block, RelativeDirection direction, int moveSpeed)
        {
            var cardinal = DirectionUtils.ToCardinal(block.Orientation, direction);
            TryStartMoving(block, cardinal, moveSpeed);
        }
        public void TryStartMoving(Block block, CardinalDirection direction, int moveSpeed)
        {
            if (block.Location.DirectionIsValid(direction) & block.CanStartMoving())
            {
                _startMoving(block, direction, moveSpeed);
            }
        }

        public void ManuallyCancelMovement(Block block)
        {
            if (block.IsMoving)
            {
                _moving.Remove(block);
            }
        }
        public void ManuallyAddMovingBlock(Block block)
        {
            _moving.Add(block);
        }

        private void _moveBlocksFromSector()
        {
            foreach(var toMove in _toMoveFromSector)
            {
                _sector.RemoveFromSectorLists(toMove.Item1);

                _moving.Remove(toMove.Item1);
                MovedOutOfSector.Add(toMove);
            }

            _toMoveFromSector.Clear();
        }
        private void _processMovingBlock(Block block)
        {
            var data = block.MovementData;
            data.MovementPosition++;

            if (data.MovementComplete | _shouldCancelMovementEarly(block))
            {
                _stopMoving(block);
            }
            else if (data.AtMidpoint)
            {
                _tryMoveBlockToNewTile(block, data);
            }
        }
        private void _tryMoveBlockToNewTile(Block block,BlockMovementData data)
        {
            if (block.TryMove(data.Direction))
            {
                data.MovePastMidpoint();

                if (!block.InSector(_sector))
                {
                    _toMoveFromSector.Add((block, block.Location.SectorID));
                }
            }
            else
            {
                _stopMoving(block);
            }
        }

        private void _startMoving(Block block, CardinalDirection direction, int moveSpeed)
        {
            block.MovementData.StartMoving(block, direction, moveSpeed);
            _moving.Add(block);
        }
        private void _stopMoving(Block block)
        {
            block.MovementData.StopMoving();
        }

        private bool _shouldCancelMovementEarly(Block block)
        {
            return !block.MovementData.PastMidpoint & !block.CanOccupyDestination(block.MovementData.Destination);
        }
        private void _removeFinishedBlocks()
        {
            _moving.RemoveAll(b => !b.IsMoving);
        }
    }
}
