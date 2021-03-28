using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class ActionsList
    {
        public List<BlockAction> Actions = new List<BlockAction>();

        public void StartMove(Block block,CardinalDirection direction)
        {
            var moveCardinalAction = new BlockAction(block, ActionType.CardinalMovement) { CardinalDir = direction, MoveTotalTicks = block.Speed };
            Actions.Add(moveCardinalAction);
        }

        public void StartMove(Block block, RelativeDirection direction)
        {
            var moveRelativeAction = new BlockAction(block, ActionType.RelativeMovement) { RelativeDir = direction, MoveTotalTicks = block.Speed };
            Actions.Add(moveRelativeAction);
        }

        public void StartRotation(Block block, int rotation)
        {
            var rotationAction = new BlockAction(block, ActionType.Rotation) { Rotation = rotation };
            Actions.Add(rotationAction);
        }

        public void CreateBlock(Block block, BlockTemplate toCreate, BlockType blockType,CardinalDirection direction)
        {
            var creationAction = new BlockAction(block, ActionType.CardinalCreation) { BlockTemplate = toCreate, BlockType = blockType, CardinalDir = direction };
            Actions.Add(creationAction);
        }
    }
}
