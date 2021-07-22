﻿using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ChipTester
    {
        public static Chipset TestEnemyBlock=> MakeEnemyBlock();
        public static Chipset TestFleeBlock => MakeFleeBlock();

        public static Chipset TestShootBlock => MakeShootBlock();

        public static Chipset TestPlayerBlock => MakePlayerBlock();
        public static Chipset TestSpinBlock => MakeSpinBlock();
        public static Chipset TestBulletBlock => MakeBulletBlock();
        public static Chipset TestBulletV2Block => MakeSpinBulletBlock();
        public static Chipset TestMouseFollowBlock => MakeMouseFollowBlock();

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



        public static Chipset MakeEnemyBlock()
        {
            var getNeighboursChip = new GetNeighbouringCubesOfTypeChip() { Name = "GetNeighbours_1", ChipInput1 = CubeMode.Surface };
            var randDirChip = new RandomCardinalChip() { Name = "RandomDir_1" };

            var ifChip = new IfChip() { Name = "If_1" };

            var isListEmptyChip = new IsListEmptyChip<Cube>() { Name = "IsListEmpty_1" };
            var firstOfListChip = new FirstOfListChip<Cube>() { Name = "FirstOfList_1" };
            var blockLocationChip = new CubeLocationChip<Cube>() { Name = "BlockLocation_1" };
            var stepToChip = new ApproachDirectionChip() { Name = "ApproachDirection_1" };
            var moveRandChip = new MoveCardinalChip() { Name = "MoveCardinal_1" };
            var moveToAdjChip = new MoveCardinalChip() { Name = "MoveCardinal_2" };

            getNeighboursChip.Targets1.Add(isListEmptyChip);
            getNeighboursChip.Targets1.Add(firstOfListChip);

            randDirChip.Targets1.Add(moveRandChip);
            firstOfListChip.Targets1.Add(blockLocationChip);
            blockLocationChip.Targets1.Add(stepToChip);
            stepToChip.Targets1.Add(moveToAdjChip);

            var initialBlock = new Chipset(getNeighboursChip, isListEmptyChip, ifChip) { Name = "_Initial" };
            var randomWalkBlock = new Chipset(randDirChip, moveRandChip) { Name = "RandomWalk" };
            var approachBlock = new Chipset(firstOfListChip, blockLocationChip, stepToChip, moveToAdjChip) { Name = "Approach" };

            isListEmptyChip.Targets1.Add(ifChip);
            ifChip.Yes = randomWalkBlock;
            ifChip.No = approachBlock;

            return initialBlock;
        }
        public static Chipset MakeFleeBlock()
        {
            var getNeighboursChip = new GetNeighbouringCubesOfTypeChip() { Name = "GetNeighbours_1", ChipInput1 = CubeMode.Surface };
            var randDirChip = new RandomCardinalChip() { Name = "RandomDir_1" };

            var ifChip = new IfChip() { Name = "If_1" };

            var isListEmptyChip = new IsListEmptyChip<Cube>() { Name = "IsListEmpty_1" };
            var firstOfListChip = new FirstOfListChip<Cube>() { Name = "FirstOfList_1" };
            var blockLocationChip = new CubeLocationChip<Cube>() { Name = "BlockLocation_1" };
            var fleeChip = new FleeDirectionChip() { Name = "FleeDirection_1" };
            var moveRandChip = new MoveCardinalChip() { Name = "MoveCardinal_1" };
            var moveToAdjChip = new MoveCardinalChip() { Name = "MoveCardinal_2" };

            getNeighboursChip.Targets1.Add(isListEmptyChip);
            getNeighboursChip.Targets1.Add(firstOfListChip);

            randDirChip.Targets1.Add(moveRandChip);
            firstOfListChip.Targets1.Add(blockLocationChip);
            blockLocationChip.Targets1.Add(fleeChip);
            fleeChip.Targets1.Add(moveToAdjChip);

            var initialBlock = new Chipset(getNeighboursChip, isListEmptyChip, ifChip) { Name = "_Initial" };
            var randomWalkBlock = new Chipset(randDirChip, moveRandChip) { Name = "RandomWalk" };
            var fleeBlock = new Chipset(firstOfListChip, blockLocationChip, fleeChip, moveToAdjChip) { Name = "Flee" };

            isListEmptyChip.Targets1.Add(ifChip);
            ifChip.Yes = randomWalkBlock;
            ifChip.No = fleeBlock;

            return initialBlock;
        }

        public static Chipset MakeShootBlock()
        {
            var randDirChip1 = new RandomCardinalChip() { Name = "RandomDir_1" };
            var moveRandChip1 = new MoveCardinalChip() { Name = "MoveCardinal_1" }; 
            randDirChip1.Targets1.Add(moveRandChip1);
            var wanderBlock1 = new Chipset(randDirChip1, moveRandChip1) { Name = "Wander1" };

            var randDirChip2 = new RandomCardinalChip() { Name = "RandomDir_2" };
            var moveRandChip2 = new MoveCardinalChip() { Name = "MoveCardinal_2" };
            randDirChip2.Targets1.Add(moveRandChip2);
            var wanderBlock2 = new Chipset(randDirChip2, moveRandChip2) { Name = "Wander2" };


            var getNeighboursChip = new GetNeighbouringCubesOfTypeChip() { Name = "GetNeighbours_1", ChipInput1 = CubeMode.Surface };

            var ifNoBlockExists = new IfChip() { Name = "If_1" };
            var ifBlockActive = new IfChip() { Name = "If_2" };

            var isListEmptyChip = new IsListEmptyChip<Cube>() { Name = "IsListEmpty_1" };
            var firstOfListChip = new FirstOfListChip<Cube>() { Name = "FirstOfList_1" };
            var blockLocationChip = new CubeLocationChip<Cube>() { Name = "BlockLocation_1" };
            var shootDirChip = new ApproachDirectionChip() { Name = "ApproachDirection_1" };
            var shootAdjchip = new CreateCardinalChip() { Name = "CreateEphemeralCardinal_1", ChipInput2 = Templates.Database["BigBullet"][0], ChipInput3 = CubeMode.Ephemeral };
            var isBlockActiveChip = new IsCubeActiveChip<Cube> { Name = "IsBlockActive_1" };

            getNeighboursChip.Targets1.Add(isListEmptyChip);
            getNeighboursChip.Targets1.Add(firstOfListChip);
            firstOfListChip.Targets1.Add(blockLocationChip);
            firstOfListChip.Targets1.Add(isBlockActiveChip);
            blockLocationChip.Targets1.Add(shootDirChip);
            shootDirChip.Targets1.Add(shootAdjchip);
            isBlockActiveChip.Targets1.Add(ifBlockActive);
            isListEmptyChip.Targets1.Add(ifNoBlockExists);

            var initialBlock = new Chipset(getNeighboursChip, isListEmptyChip, ifNoBlockExists) { Name = "_Initial" };
            var blockExists = new Chipset(firstOfListChip, isBlockActiveChip, ifBlockActive) { Name = "Shoot" };
            var shootInactiveBlock = new Chipset(blockLocationChip, shootDirChip, shootAdjchip) { Name = "ShootActive" };

            ifNoBlockExists.Yes = wanderBlock1;
            ifNoBlockExists.No = blockExists;

            ifBlockActive.Yes = wanderBlock2;
            ifBlockActive.No = shootInactiveBlock;

            return initialBlock;
        }


        public static Chipset MakePlayerBlock()
        {
            var keySwitch = new KeySwitchChip() { Name = "KeySwitch" };
            var bullet = Templates.Database["Bullet"][0];
            var bigBullet = Templates.Database["BigBullet"][0];

            var moveUp = new MoveCardinalChip() { ChipInput1 = CardinalDirection.North, Name = "MoveUp" };
            var moveDown = new MoveCardinalChip() { ChipInput1 = CardinalDirection.South, Name = "MoveDown" };
            var moveLeft = new MoveCardinalChip() { ChipInput1 = CardinalDirection.West, Name = "MoveLeft" };
            var moveRight = new MoveCardinalChip() { ChipInput1 = CardinalDirection.East, Name = "MoveRight" };
            var createEnemyNorth = new CreateCardinalChip() { ChipInput1 = CardinalDirection.North, ChipInput2 = bigBullet, ChipInput3 = CubeMode.Ephemeral, Name = "createEnemyNorth" };
            var createEnemySouth = new CreateCardinalChip() { ChipInput1 = CardinalDirection.South, ChipInput2 = bigBullet, ChipInput3 = CubeMode.Ephemeral, Name = "createEnemySouth" };
            var createEnemyWest = new CreateCardinalChip() { ChipInput1 = CardinalDirection.West, ChipInput2 = bullet, ChipInput3 = CubeMode.Ephemeral, Name = "createEnemyWest" };
            var createEnemyEast = new CreateCardinalChip() { ChipInput1 = CardinalDirection.East, ChipInput2 = bullet, ChipInput3 = CubeMode.Ephemeral, Name = "createEnemyEast" };

            keySwitch.AddKeyEffect(Keys.Up, new Chipset(createEnemyNorth) { Name = "UpBlock" });
            keySwitch.AddKeyEffect(Keys.Down, new Chipset(createEnemySouth) { Name = "DownBlock" });
            keySwitch.AddKeyEffect(Keys.Left, new Chipset(createEnemyWest) { Name = "LeftBlock" });
            keySwitch.AddKeyEffect(Keys.Right, new Chipset(createEnemyEast) { Name = "RightBlock" });
            keySwitch.AddKeyEffect(Keys.W, new Chipset(moveUp) { Name = "WBlock" });
            keySwitch.AddKeyEffect(Keys.A, new Chipset(moveLeft) { Name = "ABlock" });
            keySwitch.AddKeyEffect(Keys.S, new Chipset(moveDown) { Name = "SBlock" });
            keySwitch.AddKeyEffect(Keys.D, new Chipset(moveRight) { Name = "DBlock" });

            return new Chipset(keySwitch) { Name = "_Initial" };
        }
        public static Chipset MakeSpinBlock()
        {
            var getRotationAmountChip = new RandomNumChip() { Name = "RandomNum_1" };
            getRotationAmountChip.ChipInput1 = 3;

            var rotateRightChip = new RotateChip() { Name = "Rotation_1" };
            getRotationAmountChip.Targets1.Add(rotateRightChip);

            var moveForwardChip = new MoveRelativeChip() { Name = "MoveRelative_1" };
            moveForwardChip.ChipInput1 = RelativeDirection.Forward;

            return new Chipset(getRotationAmountChip, rotateRightChip, moveForwardChip) { Name = "_Initial" };
        }
        public static Chipset MakeBulletBlock()
        {
            var zapSurfaceChip = new ZapChip() { Name = "ZapSurfaceChip", ChipInput1 = CubeMode.Surface };

            var moveForwardChip = new MoveRelativeChip() { Name = "MoveRelative_1" };
            moveForwardChip.ChipInput1 = RelativeDirection.Forward;

            return new Chipset(zapSurfaceChip,moveForwardChip) { Name = "_Initial" };
        }
        public static Chipset MakeSpinBulletBlock()
        {
            var zapSurfaceChip = new ZapChip() { Name = "ZapSurfaceChip", ChipInput1 = CubeMode.Surface };

            var moveForwardChip = new MoveRelativeChip() { Name = "MoveRelative_1" };
            moveForwardChip.ChipInput1 = RelativeDirection.Forward;

            var rotationChip = new RotateChip() { Name = "Rotate_1" };
            rotationChip.ChipInput1 = 1;

            return new Chipset(zapSurfaceChip, moveForwardChip, rotationChip) { Name = "_Initial" };
        }
        public static Chipset MakeMouseFollowBlock()
        {
            var getMouseChip = new GetMouseHoverChip() { Name = "MouseHover_1" };
            var approachChip = new ApproachTileChip() { Name = "ApproachMouse" };

            getMouseChip.Targets1.Add(approachChip);

            return new Chipset(getMouseChip, approachChip) { Name = "_Initial" };
        }
        public static Chipset MakeFirstOfListBlock()
        {
            return null;
            var getNeighboursChip = new GetNeighbouringCubesChip() { Name = "GetNeighbours_1" };
            var firstOfListChip = new FirstOfListChip<SurfaceCube>() { Name = "FirstOfList_1" };

        }

    }
}
