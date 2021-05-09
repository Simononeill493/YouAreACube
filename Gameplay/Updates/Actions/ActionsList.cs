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
        public List<BlockAction> Actions;

        public ActionsList()
        {
            Actions = new List<BlockAction>();
        }

        public void AddMoveAction(Block block, CardinalDirection direction) => Actions.Add(new BlockAction(block, ActionType.CardinalMovement) { CardinalDir = direction, MoveSpeed = block.Speed });
        public void AddMoveAction(Block block, RelativeDirection direction) => Actions.Add(new BlockAction(block, ActionType.RelativeMovement) { RelativeDir = direction, MoveSpeed = block.Speed });
        public void AddRotationAction(Block block, int rotation) => Actions.Add(new BlockAction(block, ActionType.Rotation) { Rotation = rotation });


        public void AddCreationAction(Block block, TemplateVersionDictionary toCreate, int version, BlockMode blockType, RelativeDirection direction)
        {
            var creationAction = new BlockAction(block, ActionType.RelativeCreation)
            {
                Template = toCreate,
                Version = version,
                BlockType = blockType,
                RelativeDir = direction
            };

            Actions.Add(creationAction);
        }
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



        public void AddGiveEnergyAction(Block block, BlockMode blockType, CardinalDirection direction,int amount)
        {
            var giveEnergyAction = new BlockAction(block, ActionType.CardinalGiveEnergy)
            {
                BlockType = blockType,
                CardinalDir = direction,
                EnergyAmount = amount
            };

            Actions.Add(giveEnergyAction);
        }
        public void AddGiveEnergyAction(Block block, BlockMode blockType, RelativeDirection direction, int amount)
        {
            var giveEnergyAction = new BlockAction(block, ActionType.RelativeGiveEnergy)
            {
                BlockType = blockType,
                RelativeDir = direction,
                EnergyAmount = amount
            };

            Actions.Add(giveEnergyAction);
        }
    }
}
