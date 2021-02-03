using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ChipTester
    {
        public static ChipBlock TestEnemyBlock=> MakeEnemyBlock();
        public static ChipBlock TestFleeBlock => MakeFleeBlock();
        public static ChipBlock TestPlayerBlock => MakePlayerBlock();
        public static ChipBlock TestSpinBlock => MakeSpinBlock();
        public static ChipBlock TestBulletBlock => MakeBulletBlock();

        public static ChipBlock MakeEnemyBlock()
        {
            var getNeighboursChip = new GetNeighboursChip();
            var randDirChip = new RandomDirChip();

            var ifChip = new IfChip();

            var isListEmptyChip = new IsListEmptyChip<SurfaceBlock>();
            var firstOfListChip = new FirstOfListChip<SurfaceBlock>();
            var blockLocationChip = new BlockLocationChip();
            var stepToChip = new ApproachDirectionChip();
            var moveRandChip = new MoveCardinalChip();
            var moveToAdjChip = new MoveCardinalChip();

            getNeighboursChip.Targets.Add(isListEmptyChip);
            getNeighboursChip.Targets.Add(firstOfListChip);

            randDirChip.Targets.Add(moveRandChip);
            firstOfListChip.Targets.Add(blockLocationChip);
            blockLocationChip.Targets.Add(stepToChip);
            stepToChip.Targets.Add(moveToAdjChip);

            var initialBlock = new ChipBlock(getNeighboursChip, isListEmptyChip, ifChip);
            var randomWalkBlock = new ChipBlock(randDirChip, moveRandChip);
            var approachBlock = new ChipBlock(getNeighboursChip, firstOfListChip, blockLocationChip, stepToChip, moveToAdjChip);

            isListEmptyChip.Targets.Add(ifChip);
            ifChip.Yes = randomWalkBlock;
            ifChip.No = approachBlock;

            return initialBlock;
        }

        public static ChipBlock MakeFleeBlock()
        {
            var getNeighboursChip = new GetNeighboursChip();
            var randDirChip = new RandomDirChip();

            var ifChip = new IfChip();

            var isListEmptyChip = new IsListEmptyChip<SurfaceBlock>();
            var firstOfListChip = new FirstOfListChip<SurfaceBlock>();
            var blockLocationChip = new BlockLocationChip();
            var fleeChip = new FleeDirectionChip();
            var moveRandChip = new MoveCardinalChip();
            var moveToAdjChip = new MoveCardinalChip();

            getNeighboursChip.Targets.Add(isListEmptyChip);
            getNeighboursChip.Targets.Add(firstOfListChip);

            randDirChip.Targets.Add(moveRandChip);
            firstOfListChip.Targets.Add(blockLocationChip);
            blockLocationChip.Targets.Add(fleeChip);
            fleeChip.Targets.Add(moveToAdjChip);

            var initialBlock = new ChipBlock(getNeighboursChip, isListEmptyChip,ifChip);
            var randomWalkBlock = new ChipBlock(randDirChip, moveRandChip);
            var fleeBlock = new ChipBlock(getNeighboursChip, firstOfListChip, blockLocationChip, fleeChip, moveToAdjChip);

            isListEmptyChip.Targets.Add(ifChip);
            ifChip.Yes = randomWalkBlock;
            ifChip.No = fleeBlock;

            return initialBlock;
        }

        public static ChipBlock MakePlayerBlock()
        {
            var keySwitch = new KeySwitchChip();
            var bullet = Templates.BlockTemplates["Bullet"];

            var moveUp = new MoveCardinalChip() { ChipInput = CardinalDirection.North };
            var moveDown = new MoveCardinalChip() { ChipInput = CardinalDirection.South };
            var moveLeft = new MoveCardinalChip() { ChipInput = CardinalDirection.West };
            var moveRight = new MoveCardinalChip() { ChipInput = CardinalDirection.East };
            var createEnemyNorth = new CreateCardinalChip() { ChipInput = CardinalDirection.North, ChipInput2 = bullet, ChipInput3 = BlockType.Ephemeral };
            var createEnemySouth = new CreateCardinalChip() { ChipInput = CardinalDirection.South, ChipInput2 = bullet, ChipInput3 = BlockType.Ephemeral };
            var createEnemyWest = new CreateCardinalChip() { ChipInput = CardinalDirection.West, ChipInput2 = bullet, ChipInput3 = BlockType.Ephemeral };
            var createEnemyEast = new CreateCardinalChip() { ChipInput = CardinalDirection.East, ChipInput2 = bullet, ChipInput3 = BlockType.Ephemeral };

            keySwitch.AddKeyEffect(Keys.W, new ChipBlock(moveUp));
            keySwitch.AddKeyEffect(Keys.A, new ChipBlock(moveLeft));
            keySwitch.AddKeyEffect(Keys.S, new ChipBlock(moveDown));
            keySwitch.AddKeyEffect(Keys.D, new ChipBlock(moveRight));
            keySwitch.AddKeyEffect(Keys.Up, new ChipBlock(createEnemyNorth));
            keySwitch.AddKeyEffect(Keys.Down, new ChipBlock(createEnemySouth));
            keySwitch.AddKeyEffect(Keys.Left, new ChipBlock(createEnemyWest));
            keySwitch.AddKeyEffect(Keys.Right, new ChipBlock(createEnemyEast));

            return new ChipBlock(keySwitch);
        }

        public static ChipBlock MakeSpinBlock()
        {
            var getRotationAmountChip = new RandomNumChip();
            getRotationAmountChip.ChipInput = 3;

            var rotateRightChip = new RotationChip();
            getRotationAmountChip.Targets.Add(rotateRightChip);

            var moveForwardChip = new MoveRelativeChip();
            moveForwardChip.ChipInput = RelativeDirection.Forward;

            return new ChipBlock(getRotationAmountChip,rotateRightChip, moveForwardChip);
        }

        public static ChipBlock MakeBulletBlock()
        {
            var moveForwardChip = new MoveRelativeChip();
            moveForwardChip.ChipInput = RelativeDirection.Forward;

            return new ChipBlock(moveForwardChip);
        }
    }
}
