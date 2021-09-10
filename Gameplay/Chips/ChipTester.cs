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
        public static Chipset TestPlayerChipset => MakePlayerChipset();
        public static Chipset TestBulletChipset => MakeBulletChipset();
        public static Chipset TestBulletV2Chipset => MakeSpinBulletChipset();

        public static Chipset TestTrackerChipset => MakeTrackerChipset();

        public static Chipset TestEnemyChipset => MakeEnemyChipset();
        public static Chipset TestFleeChipset => MakeFleeChipset();

        public static Chipset TestShootChipset => MakeShootChipset();

        public static Chipset TestSpinChipset => MakeSpinChipset();
        public static Chipset TestMouseFollowChipset => MakeMouseFollowChipset();

        public static void SetTestChipsets(TemplateDatabase ChipsetTemplates)
        {
            ChipsetTemplates["BasicPlayer"][0].Chipset = TestPlayerChipset;
            ChipsetTemplates["Bullet"][0].Chipset = TestBulletChipset;
            ChipsetTemplates["BigBullet"][0].Chipset = TestBulletChipset;

            ChipsetTemplates["MouseFollower"][0].Chipset = TestMouseFollowChipset;

            ChipsetTemplates["ApproachEnemy"][0].Chipset = TestEnemyChipset;
            ChipsetTemplates["FleeEnemy"][0].Chipset = TestFleeChipset;
            ChipsetTemplates["ShootEnemy"][0].Chipset = TestShootChipset;
            ChipsetTemplates["TrackerEnemy"][0].Chipset = TestTrackerChipset;

            ChipsetTemplates["Spinner"][0].Chipset = TestSpinChipset;
            ChipsetTemplates["MiniBullet"][0].Chipset = TestBulletChipset;


            var bullet2 = ChipsetTemplates["Bullet"][0].Clone();
            bullet2.Chipset = TestBulletV2Chipset;
            ChipsetTemplates["Bullet"][1] = bullet2;

            var trackerTemplate = ChipsetTemplates["TrackerEnemy"][0];
            trackerTemplate.Variables.Dict[0] = new TemplateVariable(0, "target", InGameTypeUtils.InGameTypes[InGameTypeUtils.AnyCube]);
        } 



        public static Chipset MakeEnemyChipset()
        {
            var getNeighboursChip = new GetNeighbouringCubesOfTypeChip() { Name = "GetNeighbours_1", InputValue1 = CubeMode.Surface };
            var randDirChip = new RandomCardinalChip() { Name = "RandomDir_1" };

            var ifChip = new IfChip() { Name = "If_1" };

            var isListEmptyChip = new IsListEmptyChip<Cube>() { Name = "IsListEmpty_1" };
            var firstOfListChip = new FirstOfListChip<Cube>() { Name = "FirstOfList_1" };
            var ChipsetLocationChip = new CubeLocationChip<Cube>() { Name = "ChipsetLocation_1" };
            var stepToChip = new ApproachDirectionChip() { Name = "ApproachDirection_1" };
            var moveRandChip = new MoveCardinalChip() { Name = "MoveCardinal_1" };
            var moveToAdjChip = new MoveCardinalChip() { Name = "MoveCardinal_2" };

            isListEmptyChip.InputReference1 = getNeighboursChip;
            firstOfListChip.InputReference1 = getNeighboursChip;

            moveRandChip.InputReference1 = randDirChip;
            ChipsetLocationChip.InputReference1 = firstOfListChip;
            stepToChip.InputReference1 = ChipsetLocationChip;
            moveToAdjChip.InputReference1 = stepToChip;

            var initialChipset = new Chipset(getNeighboursChip, isListEmptyChip, ifChip) { Name = "_Initial" };
            var randomWalkChipset = new Chipset(randDirChip, moveRandChip) { Name = "RandomWalk" };
            var approachChipset = new Chipset(firstOfListChip, ChipsetLocationChip, stepToChip, moveToAdjChip) { Name = "Approach" };

            ifChip.InputReference1 = isListEmptyChip;
            ifChip.Yes = randomWalkChipset;
            ifChip.No = approachChipset;

            return initialChipset;
        }
        public static Chipset MakeFleeChipset()
        {
            var getNeighboursChip = new GetNeighbouringCubesOfTypeChip() { Name = "GetNeighbours_1", InputValue1 = CubeMode.Surface };
            var randDirChip = new RandomCardinalChip() { Name = "RandomDir_1" };

            var ifChip = new IfChip() { Name = "If_1" };

            var isListEmptyChip = new IsListEmptyChip<Cube>() { Name = "IsListEmpty_1" };
            var firstOfListChip = new FirstOfListChip<Cube>() { Name = "FirstOfList_1" };
            var ChipsetLocationChip = new CubeLocationChip<Cube>() { Name = "ChipsetLocation_1" };
            var fleeChip = new FleeDirectionChip() { Name = "FleeDirection_1" };
            var moveRandChip = new MoveCardinalChip() { Name = "MoveCardinal_1" };
            var moveToAdjChip = new MoveCardinalChip() { Name = "MoveCardinal_2" };


            isListEmptyChip.InputReference1 = getNeighboursChip;
            firstOfListChip.InputReference1 = getNeighboursChip;

            moveRandChip.InputReference1 = randDirChip;
            ChipsetLocationChip.InputReference1 = firstOfListChip;
            fleeChip.InputReference1 = ChipsetLocationChip;
            moveToAdjChip.InputReference1 = fleeChip;

            var initialChipset = new Chipset(getNeighboursChip, isListEmptyChip, ifChip) { Name = "_Initial" };
            var randomWalkChipset = new Chipset(randDirChip, moveRandChip) { Name = "RandomWalk" };
            var fleeChipset = new Chipset(firstOfListChip, ChipsetLocationChip, fleeChip, moveToAdjChip) { Name = "Flee" };

            ifChip.InputReference1 = isListEmptyChip;

            ifChip.Yes = randomWalkChipset;
            ifChip.No = fleeChipset;

            return initialChipset;
        }

        public static Chipset MakeShootChipset()
        {
            var randDirChip1 = new RandomCardinalChip() { Name = "RandomDir_1" };
            var moveRandChip1 = new MoveCardinalChip() { Name = "MoveCardinal_1" };
            moveRandChip1.InputReference1 = randDirChip1;
            var wanderChipset1 = new Chipset(randDirChip1, moveRandChip1) { Name = "Wander1" };

            var randDirChip2 = new RandomCardinalChip() { Name = "RandomDir_2" };
            var moveRandChip2 = new MoveCardinalChip() { Name = "MoveCardinal_2" };
            moveRandChip2.InputReference1 = randDirChip2;
            var wanderChipset2 = new Chipset(randDirChip2, moveRandChip2) { Name = "Wander2" };


            var getNeighboursChip = new GetNeighbouringCubesOfTypeChip() { Name = "GetNeighbours_1", InputValue1 = CubeMode.Surface };

            var ifNoChipsetExists = new IfChip() { Name = "If_1" };
            var ifChipsetActive = new IfChip() { Name = "If_2" };

            var isListEmptyChip = new IsListEmptyChip<Cube>() { Name = "IsListEmpty_1" };
            var firstOfListChip = new FirstOfListChip<Cube>() { Name = "FirstOfList_1" };
            var ChipsetLocationChip = new CubeLocationChip<Cube>() { Name = "ChipsetLocation_1" };
            var shootDirChip = new ApproachDirectionChip() { Name = "ApproachDirection_1" };
            var shootAdjchip = new CreateCardinalChip() { Name = "CreateEphemeralCardinal_1", InputValue2 = Templates.Database["BigBullet"][0], InputValue3 = CubeMode.Ephemeral };
            var isChipsetActiveChip = new IsCubeActiveChip<Cube> { Name = "IsChipsetActive_1" };

            isListEmptyChip.InputReference1 = getNeighboursChip;
            firstOfListChip.InputReference1 = getNeighboursChip;
            ChipsetLocationChip.InputReference1 = firstOfListChip;
            isChipsetActiveChip.InputReference1 = firstOfListChip;
            shootDirChip.InputReference1 = ChipsetLocationChip;
            shootAdjchip.InputReference1 = shootDirChip;
            ifChipsetActive.InputReference1 = isChipsetActiveChip;
            ifNoChipsetExists.InputReference1 = isListEmptyChip;

            var initialChipset = new Chipset(getNeighboursChip, isListEmptyChip, ifNoChipsetExists) { Name = "_Initial" };
            var ChipsetExists = new Chipset(firstOfListChip, isChipsetActiveChip, ifChipsetActive) { Name = "Shoot" };
            var shootInactiveChipset = new Chipset(ChipsetLocationChip, shootDirChip, shootAdjchip) { Name = "ShootActive" };

            ifNoChipsetExists.Yes = wanderChipset1;
            ifNoChipsetExists.No = ChipsetExists;

            ifChipsetActive.Yes = wanderChipset2;
            ifChipsetActive.No = shootInactiveChipset;

            return initialChipset;
        }

        public static Chipset MakeTrackerChipset()
        {
            var approachChip = new ApproachCubeChip<Cube>() { InputVariable1 = 0, Name="Approach" };
            var ifPercentageChip = new IfPercentageChip() { InputValue1 = 1, InputValue2 = 25, Name = "If" };
            var initialChipset = new Chipset(approachChip, ifPercentageChip) { Name = "_Initial" };

            var randomCubeChip = new GetActiveCubeInSectorChip() {Name="GetCube" };
            var setVariableChip = new SetVariableChip<Cube>() { InputValue1 = 0, InputReference2 = randomCubeChip, Name = "SetVar" };
            var changeVariableChipset = new Chipset(randomCubeChip, setVariableChip) { Name="ChangeVar" };

            ifPercentageChip.Yes = changeVariableChipset;
            ifPercentageChip.No = Chipset.NoAction;

            return initialChipset;
        }



        public static Chipset MakePlayerChipset()
        {
            var keySwitch = new KeySwitchChip() { Name = "KeySwitch" };
            var bullet = new CubeTemplateMainPlaceholder("Bullet");
            var bigBullet = new CubeTemplateMainPlaceholder("BigBullet");

            var moveUp = new MoveCardinalChip() { InputValue1 = CardinalDirection.North, Name = "MoveUp" };
            var moveDown = new MoveCardinalChip() { InputValue1 = CardinalDirection.South, Name = "MoveDown" };
            var moveLeft = new MoveCardinalChip() { InputValue1 = CardinalDirection.West, Name = "MoveLeft" };
            var moveRight = new MoveCardinalChip() { InputValue1 = CardinalDirection.East, Name = "MoveRight" };
            var createEnemyNorth = new CreateCardinalChip() { InputValue1 = CardinalDirection.North, InputValue2 = bigBullet, InputValue3 = CubeMode.Ephemeral, Name = "createEnemyNorth" };
            var createEnemySouth = new CreateCardinalChip() { InputValue1 = CardinalDirection.South, InputValue2 = bigBullet, InputValue3 = CubeMode.Ephemeral, Name = "createEnemySouth" };
            var createEnemyWest = new CreateCardinalChip() { InputValue1 = CardinalDirection.West, InputValue2 = bullet, InputValue3 = CubeMode.Ephemeral, Name = "createEnemyWest" };
            var createEnemyEast = new CreateCardinalChip() { InputValue1 = CardinalDirection.East, InputValue2 = bullet, InputValue3 = CubeMode.Ephemeral, Name = "createEnemyEast" };

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
            getRotationAmountChip.InputValue1 = 3;

            var rotateRightChip = new RotateChip() { Name = "Rotation_1" };
            rotateRightChip.InputReference1 = getRotationAmountChip;

            var moveForwardChip = new MoveRelativeChip() { Name = "MoveRelative_1" };
            moveForwardChip.InputValue1 = RelativeDirection.Forward;

            return new Chipset(getRotationAmountChip, rotateRightChip, moveForwardChip) { Name = "_Initial" };
        }
        public static Chipset MakeBulletChipset()
        {
            var zapSurfaceChip = new ZapChip() { Name = "ZapSurfaceChip", InputValue1 = CubeMode.Surface };

            var moveForwardChip = new MoveRelativeChip() { Name = "MoveRelative_1" };
            moveForwardChip.InputValue1 = RelativeDirection.Forward;

            return new Chipset(zapSurfaceChip, moveForwardChip) { Name = "_Initial" };
        }
        public static Chipset MakeSpinBulletChipset()
        {
            var zapSurfaceChip = new ZapChip() { Name = "ZapSurfaceChip", InputValue1 = CubeMode.Surface };

            var moveForwardChip = new MoveRelativeChip() { Name = "MoveRelative_1" };
            moveForwardChip.InputValue1 = RelativeDirection.Forward;

            var rotationChip = new RotateChip() { Name = "Rotate_1" };
            rotationChip.InputValue1 = 1;

            return new Chipset(zapSurfaceChip, moveForwardChip, rotationChip) { Name = "_Initial" };
        }
        public static Chipset MakeMouseFollowChipset()
        {
            var getMouseChip = new GetMouseHoverChip() { Name = "MouseHover_1" };
            var mouseDirectionChip = new ApproachDirectionChip() { Name = "ApproachMouse" };
            var giveEnergyChip = new GiveEnergyCardinalChip() { Name = "GiveEnergy", InputValue2 = CubeMode.Surface, InputValue3 = 5 };
            var moveChip = new MoveCardinalChip() { Name = "Move" };

            mouseDirectionChip.InputReference1 = getMouseChip;
            moveChip.InputReference1 = mouseDirectionChip;
            giveEnergyChip.InputReference1 = mouseDirectionChip;
            return new Chipset(getMouseChip, mouseDirectionChip, giveEnergyChip,moveChip) { Name = "_Initial" };
        
        }
    }
}
