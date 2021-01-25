using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ChipTester
    {
        public static ChipBlock TestEnemyBlock;
        public static ChipBlock TestFleeBlock;
        public static ChipBlock TestPlayerBlock;

        public static void Go()
        {
            MakeEnemyBlock();
            MakeFleeBlock();
            MakePlayerBlock();
        }

        public static void MakeEnemyBlock()
        {
            var getNeighboursChip = new GetNeighboursChip();
            var randDirChip = new RandDirChip();

            var ifChip = new IfChip();

            var isListEmptyChip = new IsListEmptyChip<SurfaceBlock>();
            var firstOfListChip = new FirstOfListChip<SurfaceBlock>();
            var blockLocationChip = new BlockLocationChip();
            var stepToChip = new ApproachDirectionChip();
            var moveRandChip = new MoveChip();
            var moveToAdjChip = new MoveChip();

            getNeighboursChip.Targets.Add(isListEmptyChip);
            getNeighboursChip.Targets.Add(firstOfListChip);

            randDirChip.Targets.Add(moveRandChip);
            firstOfListChip.Targets.Add(blockLocationChip);
            blockLocationChip.Targets.Add(stepToChip);
            stepToChip.Targets.Add(moveToAdjChip);

            var initialBlock = new ChipBlock(new List<IChip> { getNeighboursChip, isListEmptyChip, ifChip });
            var randomWalkBlock = new ChipBlock(new List<IChip> { randDirChip, moveRandChip });
            var approachBlock = new ChipBlock(new List<IChip> { getNeighboursChip, firstOfListChip, blockLocationChip, stepToChip, moveToAdjChip });

            isListEmptyChip.Targets.Add(ifChip);
            ifChip.Yes = randomWalkBlock;
            ifChip.No = approachBlock;

            TestEnemyBlock = randomWalkBlock;
        }

        public static void MakeFleeBlock()
        {
            var getNeighboursChip = new GetNeighboursChip();
            var randDirChip = new RandDirChip();

            var ifChip = new IfChip();

            var isListEmptyChip = new IsListEmptyChip<SurfaceBlock>();
            var firstOfListChip = new FirstOfListChip<SurfaceBlock>();
            var blockLocationChip = new BlockLocationChip();
            var fleeChip = new FleeDirectionChip();
            var moveRandChip = new MoveChip();
            var moveToAdjChip = new MoveChip();

            getNeighboursChip.Targets.Add(isListEmptyChip);
            getNeighboursChip.Targets.Add(firstOfListChip);

            randDirChip.Targets.Add(moveRandChip);
            firstOfListChip.Targets.Add(blockLocationChip);
            blockLocationChip.Targets.Add(fleeChip);
            fleeChip.Targets.Add(moveToAdjChip);

            var initialBlock = new ChipBlock(new List<IChip> { getNeighboursChip, isListEmptyChip, ifChip });
            var randomWalkBlock = new ChipBlock(new List<IChip> { randDirChip, moveRandChip });
            var fleeBlock = new ChipBlock(new List<IChip> { getNeighboursChip, firstOfListChip, blockLocationChip, fleeChip, moveToAdjChip });

            isListEmptyChip.Targets.Add(ifChip);
            ifChip.Yes = randomWalkBlock;
            ifChip.No = fleeBlock;

            TestFleeBlock = initialBlock;
        }

        public static void MakePlayerBlock()
        {
            var ifWPressed = new IfChip();
            var ifAPressed = new IfChip();
            var ifSPressed = new IfChip();
            var ifDPressed = new IfChip();

            var isWPressedChip = new IsKeyPressedChip();
            var isAPressedChip = new IsKeyPressedChip();
            var isSPressedChip = new IsKeyPressedChip();
            var isDPressedChip = new IsKeyPressedChip();

            var moveUp = new MoveChip();
            var moveDown = new MoveChip();
            var moveLeft = new MoveChip();
            var moveRight = new MoveChip();

            isWPressedChip.ChipInput = Microsoft.Xna.Framework.Input.Keys.W;
            isAPressedChip.ChipInput = Microsoft.Xna.Framework.Input.Keys.A;
            isSPressedChip.ChipInput = Microsoft.Xna.Framework.Input.Keys.S;
            isDPressedChip.ChipInput = Microsoft.Xna.Framework.Input.Keys.D;

            moveUp.ChipInput = Direction.Top;
            moveDown.ChipInput = Direction.Bottom;
            moveLeft.ChipInput = Direction.Left;
            moveRight.ChipInput = Direction.Right;

            isWPressedChip.Targets.Add(ifWPressed);
            isAPressedChip.Targets.Add(ifAPressed);
            isSPressedChip.Targets.Add(ifSPressed);
            isDPressedChip.Targets.Add(ifDPressed);

            ifWPressed.Yes = new ChipBlock(moveUp);
            ifAPressed.Yes = new ChipBlock(moveLeft);
            ifSPressed.Yes = new ChipBlock(moveDown);
            ifDPressed.Yes = new ChipBlock(moveRight);

            ifWPressed.No = new ChipBlock(isAPressedChip, ifAPressed);
            ifAPressed.No = new ChipBlock(isSPressedChip, ifSPressed);
            ifSPressed.No = new ChipBlock(isDPressedChip, ifDPressed);
            ifDPressed.No = ChipBlock.NoAction;

            TestPlayerBlock = new ChipBlock(isWPressedChip, ifWPressed);
        }
    }
}
