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

        public static ChipBlock TestShootBlock => MakeShootBlock();

        public static ChipBlock TestPlayerBlock => MakePlayerBlock();
        public static ChipBlock TestSpinBlock => MakeSpinBlock();
        public static ChipBlock TestBulletBlock => MakeBulletBlock();
        public static ChipBlock TestBulletV2Block => MakeSpinBulletBlock();
        public static ChipBlock TestMouseFollowBlock => MakeMouseFollowBlock();

        public static void SetTestBlocks(TemplateDatabase blockTemplates)
        {
            blockTemplates["ApproachEnemy"][0].ChipBlock = TestEnemyBlock;
            blockTemplates["FleeEnemy"][0].ChipBlock = TestFleeBlock;
            blockTemplates["ShootEnemy"][0].ChipBlock = TestShootBlock;

            blockTemplates["Spinner"][0].ChipBlock = TestSpinBlock;
            blockTemplates["Bullet"][0].ChipBlock = TestBulletBlock;
            blockTemplates["MiniBullet"][0].ChipBlock = TestBulletBlock;
            blockTemplates["BigBullet"][0].ChipBlock = TestBulletBlock;

            blockTemplates["BasicPlayer"][0].ChipBlock = TestPlayerBlock;
            blockTemplates["MouseFollower"][0].ChipBlock = TestMouseFollowBlock;

            var bullet2 = blockTemplates["Bullet"][0].Clone();
            bullet2.ChipBlock = TestBulletV2Block;
            blockTemplates["Bullet"][1] = bullet2;
        }



        public static ChipBlock MakeEnemyBlock()
        {
            var getNeighboursChip = new GetNeighbouringBlocksOfTypeChip() { Name = "GetNeighbours_1", ChipInput1 = BlockMode.Surface };
            var randDirChip = new RandomCardinalChip() { Name = "RandomDir_1" };

            var ifChip = new IfChip() { Name = "If_1" };

            var isListEmptyChip = new IsListEmptyChip<Block>() { Name = "IsListEmpty_1" };
            var firstOfListChip = new FirstOfListChip<Block>() { Name = "FirstOfList_1" };
            var blockLocationChip = new BlockLocationChip<Block>() { Name = "BlockLocation_1" };
            var stepToChip = new ApproachDirectionChip() { Name = "ApproachDirection_1" };
            var moveRandChip = new MoveCardinalChip() { Name = "MoveCardinal_1" };
            var moveToAdjChip = new MoveCardinalChip() { Name = "MoveCardinal_2" };

            getNeighboursChip.Targets1.Add(isListEmptyChip);
            getNeighboursChip.Targets1.Add(firstOfListChip);

            randDirChip.Targets1.Add(moveRandChip);
            firstOfListChip.Targets1.Add(blockLocationChip);
            blockLocationChip.Targets1.Add(stepToChip);
            stepToChip.Targets1.Add(moveToAdjChip);

            var initialBlock = new ChipBlock(getNeighboursChip, isListEmptyChip, ifChip) { Name = "_Initial" };
            var randomWalkBlock = new ChipBlock(randDirChip, moveRandChip) { Name = "RandomWalk" };
            var approachBlock = new ChipBlock(firstOfListChip, blockLocationChip, stepToChip, moveToAdjChip) { Name = "Approach" };

            isListEmptyChip.Targets1.Add(ifChip);
            ifChip.Yes = randomWalkBlock;
            ifChip.No = approachBlock;

            return initialBlock;
        }
        public static ChipBlock MakeFleeBlock()
        {
            var getNeighboursChip = new GetNeighbouringBlocksOfTypeChip() { Name = "GetNeighbours_1", ChipInput1 = BlockMode.Surface };
            var randDirChip = new RandomCardinalChip() { Name = "RandomDir_1" };

            var ifChip = new IfChip() { Name = "If_1" };

            var isListEmptyChip = new IsListEmptyChip<Block>() { Name = "IsListEmpty_1" };
            var firstOfListChip = new FirstOfListChip<Block>() { Name = "FirstOfList_1" };
            var blockLocationChip = new BlockLocationChip<Block>() { Name = "BlockLocation_1" };
            var fleeChip = new FleeDirectionChip() { Name = "FleeDirection_1" };
            var moveRandChip = new MoveCardinalChip() { Name = "MoveCardinal_1" };
            var moveToAdjChip = new MoveCardinalChip() { Name = "MoveCardinal_2" };

            getNeighboursChip.Targets1.Add(isListEmptyChip);
            getNeighboursChip.Targets1.Add(firstOfListChip);

            randDirChip.Targets1.Add(moveRandChip);
            firstOfListChip.Targets1.Add(blockLocationChip);
            blockLocationChip.Targets1.Add(fleeChip);
            fleeChip.Targets1.Add(moveToAdjChip);

            var initialBlock = new ChipBlock(getNeighboursChip, isListEmptyChip, ifChip) { Name = "_Initial" };
            var randomWalkBlock = new ChipBlock(randDirChip, moveRandChip) { Name = "RandomWalk" };
            var fleeBlock = new ChipBlock(firstOfListChip, blockLocationChip, fleeChip, moveToAdjChip) { Name = "Flee" };

            isListEmptyChip.Targets1.Add(ifChip);
            ifChip.Yes = randomWalkBlock;
            ifChip.No = fleeBlock;

            return initialBlock;
        }

        public static ChipBlock MakeShootBlock()
        {
            var randDirChip1 = new RandomCardinalChip() { Name = "RandomDir_1" };
            var moveRandChip1 = new MoveCardinalChip() { Name = "MoveCardinal_1" }; 
            randDirChip1.Targets1.Add(moveRandChip1);
            var wanderBlock1 = new ChipBlock(randDirChip1, moveRandChip1) { Name = "Wander1" };

            var randDirChip2 = new RandomCardinalChip() { Name = "RandomDir_2" };
            var moveRandChip2 = new MoveCardinalChip() { Name = "MoveCardinal_2" };
            randDirChip2.Targets1.Add(moveRandChip1);
            var wanderBlock2 = new ChipBlock(randDirChip2, moveRandChip2) { Name = "Wander2" };


            var getNeighboursChip = new GetNeighbouringBlocksOfTypeChip() { Name = "GetNeighbours_1", ChipInput1 = BlockMode.Surface };

            var ifNoBlockExists = new IfChip() { Name = "If_1" };
            var ifBlockActive = new IfChip() { Name = "If_2" };

            var isListEmptyChip = new IsListEmptyChip<Block>() { Name = "IsListEmpty_1" };
            var firstOfListChip = new FirstOfListChip<Block>() { Name = "FirstOfList_1" };
            var blockLocationChip = new BlockLocationChip<Block>() { Name = "BlockLocation_1" };
            var shootDirChip = new ApproachDirectionChip() { Name = "ApproachDirection_1" };
            var shootAdjchip = new CreateEphemeralCardinalChip() { Name = "CreateEphemeralCardinal_1", ChipInput2 = Templates.Database["BigBullet"], ChipInput3 = 0 };
            var isBlockActiveChip = new IsBlockActiveChip<Block> { Name = "IsBlockActive_1" };

            getNeighboursChip.Targets1.Add(isListEmptyChip);
            getNeighboursChip.Targets1.Add(firstOfListChip);
            firstOfListChip.Targets1.Add(blockLocationChip);
            firstOfListChip.Targets1.Add(isBlockActiveChip);
            blockLocationChip.Targets1.Add(shootDirChip);
            shootDirChip.Targets1.Add(shootAdjchip);
            isBlockActiveChip.Targets1.Add(ifBlockActive);
            isListEmptyChip.Targets1.Add(ifNoBlockExists);

            var initialBlock = new ChipBlock(getNeighboursChip, isListEmptyChip, ifNoBlockExists) { Name = "_Initial" };
            var blockExists = new ChipBlock(firstOfListChip, isBlockActiveChip, ifBlockActive) { Name = "Shoot" };
            var shootInactiveBlock = new ChipBlock(blockLocationChip, shootDirChip, shootAdjchip) { Name = "ShootActive" };

            ifNoBlockExists.Yes = wanderBlock1;
            ifNoBlockExists.No = blockExists;

            ifBlockActive.Yes = wanderBlock2;
            ifBlockActive.No = shootInactiveBlock;

            return initialBlock;
        }


        public static ChipBlock MakePlayerBlock()
        {
            var keySwitch = new KeySwitchChip() { Name = "KeySwitch" };
            var bullet = Templates.Database["Bullet"];
            var bigBullet = Templates.Database["BigBullet"];

            var moveUp = new MoveCardinalChip() { ChipInput1 = CardinalDirection.North, Name = "MoveUp" };
            var moveDown = new MoveCardinalChip() { ChipInput1 = CardinalDirection.South, Name = "MoveDown" };
            var moveLeft = new MoveCardinalChip() { ChipInput1 = CardinalDirection.West, Name = "MoveLeft" };
            var moveRight = new MoveCardinalChip() { ChipInput1 = CardinalDirection.East, Name = "MoveRight" };
            var createEnemyNorth = new CreateEphemeralCardinalChip() { ChipInput1 = CardinalDirection.North, ChipInput2 = bigBullet, ChipInput3 = 0, Name = "createEnemyNorth" };
            var createEnemySouth = new CreateEphemeralCardinalChip() { ChipInput1 = CardinalDirection.South, ChipInput2 = bigBullet, ChipInput3 = 0, Name = "createEnemySouth" };
            var createEnemyWest = new CreateEphemeralCardinalChip() { ChipInput1 = CardinalDirection.West, ChipInput2 = bullet, ChipInput3 = 0, Name = "createEnemyWest" };
            var createEnemyEast = new CreateEphemeralCardinalChip() { ChipInput1 = CardinalDirection.East, ChipInput2 = bullet, ChipInput3 = 0, Name = "createEnemyEast" };

            keySwitch.AddKeyEffect(Keys.Up, new ChipBlock(createEnemyNorth) { Name = "UpBlock" });
            keySwitch.AddKeyEffect(Keys.Down, new ChipBlock(createEnemySouth) { Name = "DownBlock" });
            keySwitch.AddKeyEffect(Keys.Left, new ChipBlock(createEnemyWest) { Name = "LeftBlock" });
            keySwitch.AddKeyEffect(Keys.Right, new ChipBlock(createEnemyEast) { Name = "RightBlock" });
            keySwitch.AddKeyEffect(Keys.W, new ChipBlock(moveUp) { Name = "WBlock" });
            keySwitch.AddKeyEffect(Keys.A, new ChipBlock(moveLeft) { Name = "ABlock" });
            keySwitch.AddKeyEffect(Keys.S, new ChipBlock(moveDown) { Name = "SBlock" });
            keySwitch.AddKeyEffect(Keys.D, new ChipBlock(moveRight) { Name = "DBlock" });

            return new ChipBlock(keySwitch) { Name = "_Initial" };
        }
        public static ChipBlock MakeSpinBlock()
        {
            var getRotationAmountChip = new RandomNumChip() { Name = "RandomNum_1" };
            getRotationAmountChip.ChipInput1 = 3;

            var rotateRightChip = new RotateChip() { Name = "Rotation_1" };
            getRotationAmountChip.Targets1.Add(rotateRightChip);

            var moveForwardChip = new MoveRelativeChip() { Name = "MoveRelative_1" };
            moveForwardChip.ChipInput1 = RelativeDirection.Forward;

            return new ChipBlock(getRotationAmountChip, rotateRightChip, moveForwardChip) { Name = "_Initial" };
        }
        public static ChipBlock MakeBulletBlock()
        {
            var zapSurfaceChip = new ZapChip() { Name = "ZapSurfaceChip", ChipInput1 = BlockMode.Surface };

            var moveForwardChip = new MoveRelativeChip() { Name = "MoveRelative_1" };
            moveForwardChip.ChipInput1 = RelativeDirection.Forward;

            return new ChipBlock(zapSurfaceChip,moveForwardChip) { Name = "_Initial" };
        }
        public static ChipBlock MakeSpinBulletBlock()
        {
            var zapSurfaceChip = new ZapChip() { Name = "ZapSurfaceChip", ChipInput1 = BlockMode.Surface };

            var moveForwardChip = new MoveRelativeChip() { Name = "MoveRelative_1" };
            moveForwardChip.ChipInput1 = RelativeDirection.Forward;

            var rotationChip = new RotateChip() { Name = "Rotate_1" };
            rotationChip.ChipInput1 = 1;

            return new ChipBlock(zapSurfaceChip, moveForwardChip, rotationChip) { Name = "_Initial" };
        }
        public static ChipBlock MakeMouseFollowBlock()
        {
            var getMouseChip = new GetMouseHoverChip() { Name = "MouseHover_1" };
            var approachDirChip = new ApproachDirectionChip() { Name = "ApproachDir" };
            var moveCardinalChip = new MoveCardinalChip() { Name = "MoveCardinal_1" };

            getMouseChip.Targets1.Add(approachDirChip);
            approachDirChip.Targets1.Add(moveCardinalChip);

            return new ChipBlock(getMouseChip, approachDirChip, moveCardinalChip) { Name = "_Initial" };
        }
        public static ChipBlock MakeFirstOfListBlock()
        {
            return null;
            var getNeighboursChip = new GetNeighbouringBlocksChip() { Name = "GetNeighbours_1" };
            var firstOfListChip = new FirstOfListChip<SurfaceBlock>() { Name = "FirstOfList_1" };

        }

    }
}
