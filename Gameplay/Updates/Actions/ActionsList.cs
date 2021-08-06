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

        public void AddApproachAction(Cube block, Cube target) => Actions.Add(new BlockAction(block, ActionType.ApproachBlock) { TargetBlock = target, MoveSpeed = block.Speed });


        public void AddApproachAction(Cube block, Tile target) => Actions.Add(new BlockAction(block, ActionType.ApproachTile) { TargetTile = target, MoveSpeed = block.Speed });

        public void AddMoveAction(Cube block, CardinalDirection direction) => Actions.Add(new BlockAction(block, ActionType.CardinalMovement) { CardinalDir = direction, MoveSpeed = block.Speed });
        public void AddMoveAction(Cube block, RelativeDirection direction) => Actions.Add(new BlockAction(block, ActionType.RelativeMovement) { RelativeDir = direction, MoveSpeed = block.Speed });
        
        public void AddRotationAction(Cube block, int rotation) => Actions.Add(new BlockAction(block, ActionType.Rotation) { Rotation = rotation });


        public void AddCreationAction(Cube block, CubeTemplate toCreate, CubeMode blockType, RelativeDirection direction)
        {
            var creationAction = new BlockAction(block, ActionType.RelativeCreation)
            {
                Template = toCreate,
                BlockType = blockType,
                RelativeDir = direction
            };

            Actions.Add(creationAction);
        }
        public void AddCreationAction(Cube block, CubeTemplate toCreate, CubeMode blockType, CardinalDirection direction)
        {
            var creationAction = new BlockAction(block, ActionType.CardinalCreation) 
            { 
                Template = toCreate,
                BlockType = blockType, 
                CardinalDir = direction 
            };

            Actions.Add(creationAction);
        }

        public void AddSapEnergyAction(Cube block, CubeMode blockType, CardinalDirection direction)
        {
            var sapEnergyAction = new BlockAction(block, ActionType.CardinalSapEnergy)
            {
                BlockType = blockType,
                CardinalDir = direction,
            };

            Actions.Add(sapEnergyAction);
        }
        public void AddSapEnergyAction(Cube block, CubeMode blockType, RelativeDirection direction)
        {
            var sapEnergyAction = new BlockAction(block, ActionType.RelativeSapEnergy)
            {
                BlockType = blockType,
                RelativeDir = direction,
            };

            Actions.Add(sapEnergyAction);
        }


        public void AddGiveEnergyAction(Cube block, CubeMode blockType, CardinalDirection direction,int amount)
        {
            var giveEnergyAction = new BlockAction(block, ActionType.CardinalGiveEnergy)
            {
                BlockType = blockType,
                CardinalDir = direction,
                EnergyAmount = amount
            };

            Actions.Add(giveEnergyAction);
        }
        public void AddGiveEnergyAction(Cube block, CubeMode blockType, RelativeDirection direction, int amount)
        {
            var giveEnergyAction = new BlockAction(block, ActionType.RelativeGiveEnergy)
            {
                BlockType = blockType,
                RelativeDir = direction,
                EnergyAmount = amount
            };

            Actions.Add(giveEnergyAction);
        }




        internal void AddZapAction(Cube actor, CubeMode blockMode)
        {
            var zapAction = new BlockAction(actor, ActionType.Zap)
            {
                BlockType = blockMode
            };

            Actions.Add(zapAction);
        }

    }
}
