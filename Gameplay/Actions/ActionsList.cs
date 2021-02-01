using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ActionsList
    {
        public List<Action> Actions = new List<Action>();

        public void StartMove(Block block,CardinalDirection direction)
        {
            var moveCardinalAction = new Action(block, ActionType.CardinalMovement) { CardinalDir = direction, MoveSpeed = block.Speed };
            Actions.Add(moveCardinalAction);
        }

        public void StartMove(Block block, RelativeDirection direction)
        {
            var moveRelativeAction = new Action(block, ActionType.RelativeMovement) { RelativeDir = direction, MoveSpeed = block.Speed };
            Actions.Add(moveRelativeAction);
        }

        public void StartRotation(Block block, int rotation)
        {
            var rotationAction = new Action(block, ActionType.Rotation) { Rotation = rotation };
            Actions.Add(rotationAction);
        }

        public void CreateBlock(Block block, BlockTemplate toCreate, BlockType blockType,CardinalDirection direction)
        {
            var creationAction = new Action(block, ActionType.CardinalCreation) { BlockTemplate = toCreate, BlockType = blockType, CardinalDir = direction };
            Actions.Add(creationAction);
        }
    }
}
