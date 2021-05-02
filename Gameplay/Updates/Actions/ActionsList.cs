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

        public void AddMoveAction(Block block, CardinalDirection direction) => Actions.Add(new BlockAction(block, ActionType.CardinalMovement) { CardinalDir = direction, MoveSpeed = block.Speed });
        public void AddMoveAction(Block block, RelativeDirection direction) => Actions.Add(new BlockAction(block, ActionType.RelativeMovement) { RelativeDir = direction, MoveSpeed = block.Speed });
        public void AddRotationAction(Block block, int rotation) => Actions.Add(new BlockAction(block, ActionType.Rotation) { Rotation = rotation });
        public void AddCreationAction(Block block, TemplateVersionDictionary toCreate, int version, BlockMode blockType,CardinalDirection direction)
        {
            var creationAction = new BlockAction(block, ActionType.CardinalCreation) 
            { 
                Template = toCreate,
                Version = version,
                BlockType = blockType, 
                CardinalDir = direction 
            };

            Actions.Add(creationAction);
        }
    }
}
