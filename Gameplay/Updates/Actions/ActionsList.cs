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

        public void StartMove(Block block, CardinalDirection direction) => Actions.Add(new BlockAction(block, ActionType.CardinalMovement) { CardinalDir = direction, MoveTotalTicks = block.Speed });
        public void StartMove(Block block, RelativeDirection direction) => Actions.Add(new BlockAction(block, ActionType.RelativeMovement) { RelativeDir = direction, MoveTotalTicks = block.Speed });
        public void StartRotation(Block block, int rotation) => Actions.Add(new BlockAction(block, ActionType.Rotation) { Rotation = rotation });

        public void CreateBlock(Block block, TemplateVersionDictionary toCreate, int version, BlockMode blockType,CardinalDirection direction)
        {
            var creationAction = new BlockAction(block, ActionType.CardinalCreation) 
            { 
                BlockTemplate = toCreate,
                TemplateVersion = version,
                BlockType = blockType, 
                CardinalDir = direction 
            };

            Actions.Add(creationAction);
        }
    }
}
