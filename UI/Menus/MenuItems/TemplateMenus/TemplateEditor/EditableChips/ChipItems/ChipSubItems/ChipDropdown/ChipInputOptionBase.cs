using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    public class ChipInputOptionBase : ChipInputOption
    {
        public object BaseObject;
        public override string ToString()
        {
            return BaseObject.ToString();
        }
    }
}
