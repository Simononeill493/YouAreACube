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
        public static Chipset TestEnemyChipset => MakeEnemyChipset();
        public static Chipset TestFleeChipset => MakeFleeChipset();

        public static Chipset TestShootChipset => MakeShootChipset();

        public static Chipset TestPlayerChipset => MakePlayerChipset();
        public static Chipset TestSpinChipset => MakeSpinChipset();
        public static Chipset TestBulletChipset => MakeBulletChipset();
        public static Chipset TestBulletV2Chipset => MakeSpinBulletChipset();
        public static Chipset TestMouseFollowChipset => MakeMouseFollowChipset();

        public static void SetTestChipsets(TemplateDatabase ChipsetTemplates)
        {
            ChipsetTemplates["ApproachEnemy"][0].Chipset = TestEnemyChipset;
            ChipsetTemplates["FleeEnemy"][0].Chipset = TestFleeChipset;
            ChipsetTemplates["ShootEnemy"][0].Chipset = TestShootChipset;

            ChipsetTemplates["Spinner"][0].Chipset = TestSpinChipset;
            ChipsetTemplates["Bullet"][0].Chipset = TestBulletChipset;
            ChipsetTemplates["MiniBullet"][0].Chipset = TestBulletChipset;
            ChipsetTemplates["BigBullet"][0].Chipset = TestBulletChipset;

            ChipsetTemplates["BasicPlayer"][0].Chipset = TestPlayerChipset;
            ChipsetTemplates["MouseFollower"][0].Chipset = TestMouseFollowChipset;

            var bullet2 = ChipsetTemplates["Bullet"][0].Clone();
            bullet2.Chipset = TestBulletV2Chipset;
            ChipsetTemplates["Bullet"][1] = bullet2;
        }



        public static Chipset MakeEnemyChipset()
        {
            var getNeighboursChip = new GetNeighbouringCubesOfTypeChip() { Name = "GetNeighbours_1", ChipInput1 = CubeMode.Surface };
            var randDirChip = new RandomCardinalChip() { Name = "RandomDir_1" };

            var ifChip = new IfChip() { Name = "If_1" };

            var isListEmptyChip = new IsListEmptyChip<Cube>() { Name = "IsListEmpty_1" };
            var firstOfListChip = new FirstOfListChip<Cube>() { Name = "FirstOfList_1" };
            var ChipsetLocationChip = new CubeLocationChip<Cube>() { Name = "ChipsetLocation_1" };
            var stepToChip = new ApproachDirectionChip() { Name = "ApproachDirection_1" };
            var moveRandChip = new MoveCardinalChip() { Name = "MoveCardinal_1" };
            var moveToAdjChip = new MoveCardinalChip() { Name = "MoveCardinal_2" };

            getNeighboursChip.Targets1.Add(isListEmptyChip);
            getNeighboursChip.Targets1.Add(firstOfListChip);

            randDirChip.Targets1.Add(moveRandChip);
            firstOfListChip.Targets1.Add(ChipsetLocationChip);
            ChipsetLocationChip.Targets1.Add(stepToChip);
            stepToChip.Targets1.Add(moveToAdjChip);

            var initialChipset = new Chipset(getNeighboursChip, isListEmptyChip, ifChip) { Name = "_Initial" };
            var randomWalkChipset = new Chipset(randDirChip, moveRandChip) { Name = "RandomWalk" };
            var approachChipset = new Chipset(firstOfListChip, ChipsetLocationChip, stepToChip, moveToAdjChip) { Name = "Approach" };

            isListEmptyChip.Targets1.Add(ifChip);
            ifChip.Yes = randomWalkChipset;
            ifChip.No = approachChipset;

            return initialChipset;
        }
        public static Chipset MakeFleeChipset()
        {
            var getNeighboursChip = new GetNeighbouringCubesOfTypeChip() { Name = "GetNeighbours_1", ChipInput1 = CubeMode.Surface };
            var randDirChip = new RandomCardinalChip() { Name = "RandomDir_1" };

            var ifChip = new IfChip() { Name = "If_1" };

            var isListEmptyChip = new IsListEmptyChip<Cube>() { Name = "IsListEmpty_1" };
            var firstOfListChip = new FirstOfListChip<Cube>() { Name = "FirstOfList_1" };
            var ChipsetLocationChip = new CubeLocationChip<Cube>() { Name = "ChipsetLocation_1" };
            var fleeChip = new FleeDirectionChip() { Name = "FleeDirection_1" };
            var moveRandChip = new MoveCardinalChip() { Name = "MoveCardinal_1" };
            var moveToAdjChip = new MoveCardinalChip() { Name = "MoveCardinal_2" };

            getNeighboursChip.Targets1.Add(isListEmptyChip);
            getNeighboursChip.Targets1.Add(firstOfListChip);

            randDirChip.Targets1.Add(moveRandChip);
            firstOfListChip.Targets1.Add(ChipsetLocationChip);
            ChipsetLocationChip.Targets1.Add(fleeChip);
            fleeChip.Targets1.Add(moveToAdjChip);

            var initialChipset = new Chipset(getNeighboursChip, isListEmptyChip, ifChip) { Name = "_Initial" };
            var randomWalkChipset = new Chipset(randDirChip, moveRandChip) { Name = "RandomWalk" };
            var fleeChipset = new Chipset(firstOfListChip, ChipsetLocationChip, fleeChip, moveToAdjChip) { Name = "Flee" };

            isListEmptyChip.Targets1.Add(ifChip);
            ifChip.Yes = randomWalkChipset;
            ifChip.No = fleeChipset;

            return initialChipset;
        }

        public static Chipset MakeShootChipset()
        {
            var randDirChip1 = new RandomCardinalChip() { Name = "RandomDir_1" };
            var moveRandChip1 = new MoveCardinalChip() { Name = "MoveCardinal_1" };
            randDirChip1.Targets1.Add(moveRandChip1);
            var wanderChipset1 = new Chipset(randDirChip1, moveRandChip1) { Name = "Wander1" };

            var randDirChip2 = new RandomCardinalChip() { Name = "RandomDir_2" };
            var moveRandChip2 = new MoveCardinalChip() { Name = "MoveCardinal_2" };
            randDirChip2.Targets1.Add(moveRandChip2);
            var wanderChipset2 = new Chipset(randDirChip2, moveRandChip2) { Name = "Wander2" };


            var getNeighboursChip = new GetNeighbouringCubesOfTypeChip() { Name = "GetNeighbours_1", ChipInput1 = CubeMode.Surface };

            var ifNoChipsetExists = new IfChip() { Name = "If_1" };
            var ifChipsetActive = new IfChip() { Name = "If_2" };

            var isListEmptyChip = new IsListEmptyChip<Cube>() { Name = "IsListEmpty_1" };
            var firstOfListChip = new FirstOfListChip<Cube>() { Name = "FirstOfList_1" };
            var ChipsetLocationChip = new CubeLocationChip<Cube>() { Name = "ChipsetLocation_1" };
            var shootDirChip = new ApproachDirectionChip() { Name = "ApproachDirection_1" };
            var shootAdjchip = new CreateCardinalChip() { Name = "CreateEphemeralCardinal_1", ChipInput2 = Templates.Database["BigBullet"][0], ChipInput3 = CubeMode.Ephemeral };
            var isChipsetActiveChip = new IsCubeActiveChip<Cube> { Name = "IsChipsetActive_1" };

            getNeighboursChip.Targets1.Add(isListEmptyChip);
            getNeighboursChip.Targets1.Add(firstOfListChip);
            firstOfListChip.Targets1.Add(ChipsetLocationChip);
            firstOfListChip.Targets1.Add(isChipsetActiveChip);
            ChipsetLocationChip.Targets1.Add(shootDirChip);
            shootDirChip.Targets1.Add(shootAdjchip);
            isChipsetActiveChip.Targets1.Add(ifChipsetActive);
            isListEmptyChip.Targets1.Add(ifNoChipsetExists);

            var initialChipset = new Chipset(getNeighboursChip, isListEmptyChip, ifNoChipsetExists) { Name = "_Initial" };
            var ChipsetExists = new Chipset(firstOfListChip, isChipsetActiveChip, ifChipsetActive) { Name = "Shoot" };
            var shootInactiveChipset = new Chipset(ChipsetLocationChip, shootDirChip, shootAdjchip) { Name = "ShootActive" };

            ifNoChipsetExists.Yes = wanderChipset1;
            ifNoChipsetExists.No = ChipsetExists;

            ifChipsetActive.Yes = wanderChipset2;
            ifChipsetActive.No = shootInactiveChipset;

            return initialChipset;
        }


        public static Chipset MakePlayerChipset()
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

            keySwitch.AddKeyEffect(Keys.Up, new Chipset(createEnemyNorth) { Name = "UpChipset" });
            keySwitch.AddKeyEffect(Keys.Down, new Chipset(createEnemySouth) { Name = "DownChipset" });
            keySwitch.AddKeyEffect(Keys.Left, new Chipset(createEnemyWest) { Name = "LeftChipset" });
            keySwitch.AddKeyEffect(Keys.Right, new Chipset(createEnemyEast) { Name = "RightChipset" });
            keySwitch.AddKeyEffect(Keys.W, new Chipset(moveUp) { Name = "WChipset" });
            keySwitch.AddKeyEffect(Keys.A, new Chipset(moveLeft) { Name = "AChipset" });
            keySwitch.AddKeyEffect(Keys.S, new Chipset(moveDown) { Name = "SChipset" });
            keySwitch.AddKeyEffect(Keys.D, new Chipset(moveRight) { Name = "DChipset" });

            return new Chipset(keySwitch) { Name = "_Initial" };
        }
        public static Chipset MakeSpinChipset()
        {
            var getRotationAmountChip = new RandomNumChip() { Name = "RandomNum_1" };
            getRotationAmountChip.ChipInput1 = 3;

            var rotateRightChip = new RotateChip() { Name = "Rotation_1" };
            getRotationAmountChip.Targets1.Add(rotateRightChip);

            var moveForwardChip = new MoveRelativeChip() { Name = "MoveRelative_1" };
            moveForwardChip.ChipInput1 = RelativeDirection.Forward;

            return new Chipset(getRotationAmountChip, rotateRightChip, moveForwardChip) { Name = "_Initial" };
        }
        public static Chipset MakeBulletChipset()
        {
            var zapSurfaceChip = new ZapChip() { Name = "ZapSurfaceChip", ChipInput1 = CubeMode.Surface };

            var moveForwardChip = new MoveRelativeChip() { Name = "MoveRelative_1" };
            moveForwardChip.ChipInput1 = RelativeDirection.Forward;

            return new Chipset(zapSurfaceChip, moveForwardChip) { Name = "_Initial" };
        }
        public static Chipset MakeSpinBulletChipset()
        {
            var zapSurfaceChip = new ZapChip() { Name = "ZapSurfaceChip", ChipInput1 = CubeMode.Surface };

            var moveForwardChip = new MoveRelativeChip() { Name = "MoveRelative_1" };
            moveForwardChip.ChipInput1 = RelativeDirection.Forward;

            var rotationChip = new RotateChip() { Name = "Rotate_1" };
            rotationChip.ChipInput1 = 1;

            return new Chipset(zapSurfaceChip, moveForwardChip, rotationChip) { Name = "_Initial" };
        }
        public static Chipset MakeMouseFollowChipset()
        {
            var getMouseChip = new GetMouseHoverChip() { Name = "MouseHover_1" };
            var approachChip = new ApproachTileChip() { Name = "ApproachMouse" };

            getMouseChip.Targets1.Add(approachChip);

            return new Chipset(getMouseChip, approachChip) { Name = "_Initial" };
        }
        public static Chipset MakeFirstOfListChipset()
        {
            return null;
            var getNeighboursChip = new GetNeighbouringCubesChip() { Name = "GetNeighbours_1" };
            var firstOfListChip = new FirstOfListChip<SurfaceCube>() { Name = "FirstOfList_1" };

        }

    }
}
