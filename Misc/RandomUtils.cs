using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class RandomUtils
    {
        public static Random testSeed;

        public const int NumDirections = 8;
        public static Direction[] Directions;

        public static void Init(int seed)
        {
            InitTestSeed(seed);

            Directions = typeof(Direction).GetEnumValues().Cast<Direction>().ToArray();
        }
        public static void InitTestSeed(int seed)
        {
            testSeed = new Random(seed);
        }

        public static Direction RandomTestDir()
        {
            return (Direction)testSeed.Next(0, NumDirections);
        }
    }
}
