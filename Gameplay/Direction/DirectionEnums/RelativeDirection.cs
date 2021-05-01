using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public enum RelativeDirection
    {
        Forward = 0,
        ForwardRight = 1,
        Right = 2,
        BackwardRight = 3,
        Backward = 4,
        BackwardLeft = 5,
        Left = 6,
        ForwardLeft = 7
    }
}
