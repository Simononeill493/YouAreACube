using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    static class RandomUtils
    {
        public static Random R { get; private set; }
        public static void Init() => R = new Random();
        public static void Init(int seed = 1) => R = new Random(seed);

        public static CardinalDirection RandomCardinal() => (CardinalDirection)R.Next(0, DirectionUtils.NumCardinalDirections);
        public static RelativeDirection RandomRelative() => (RelativeDirection)R.Next(0, DirectionUtils.NumRelativeDirections);

        public static int RandomNumber(int upperBoundExclusive) => R.Next(0, upperBoundExclusive);
        public static List<T> GetShuffledList<T>(List<T> toShuffle) => toShuffle.OrderBy(x => R.Next()).ToList();
        public static List<T> GetShuffledList<T>(List<T> toShuffle, Random r) => toShuffle.OrderBy(x => r.Next()).ToList();

        public static T GetRandom<T>(this List<T> list) => list[R.Next(0,list.Count)];
    }
}
