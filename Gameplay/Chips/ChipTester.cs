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

        public static void Go()
        {
            MakeEnemyBlock();
            MakeFleeBlock();
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

            TestEnemyBlock = initialBlock;
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
    }
}
