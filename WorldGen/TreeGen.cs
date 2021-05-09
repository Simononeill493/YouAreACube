using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TreeGen
    {
        public static int NumberOfPoints = 50;
        public static void Test()
        {
            var r = new Random();

            var points = new List<DataPoint>();
            for(int i=0;i<NumberOfPoints;i++)
            {
                var newPoint = new DataPoint(r.Next(1, 50), r.Next(1, 50), r.Next(0, 2));
                points.Add(newPoint);
            }


        }

        public class DataPoint
        {
            public int x;
            public int y;
            public int colorNumber;

            public DataPoint(int x, int y, int colorNumber)
            {
                this.x = x;
                this.y = y;
                this.colorNumber = colorNumber;
            }
        }
    }
}
