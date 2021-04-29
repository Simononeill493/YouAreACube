using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class RandomUtils
    {
        public static Random R;

        private const int _numDirections = 8;

        public static void Init()=> R = new Random();
        public static void Init(int seed)=>R = new Random(seed);
        
        public static CardinalDirection RandomDirection()=>(CardinalDirection)R.Next(0, _numDirections);
        public static int RandomNumber(int upperBoundExclusive)=>R.Next(0, upperBoundExclusive);
        public static List<T> Shuffle<T>(List<T> toShuffle)=> toShuffle.OrderBy(x => R.Next()).ToList();
    }
}
