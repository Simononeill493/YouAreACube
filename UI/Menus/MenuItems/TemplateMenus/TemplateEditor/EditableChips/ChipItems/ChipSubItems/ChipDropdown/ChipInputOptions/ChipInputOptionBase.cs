using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    public class ChipInputOptionBase : ChipInputOption
    {
        public object BaseObject;

        public ChipInputOptionBase() : base(InputOptionType.Base) {  }

        public override string ToString() => BaseObject.ToString();
    }
}
