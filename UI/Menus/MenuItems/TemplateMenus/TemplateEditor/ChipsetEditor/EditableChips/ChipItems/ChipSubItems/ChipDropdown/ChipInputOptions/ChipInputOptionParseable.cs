using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ChipInputOptionParseable : ChipInputOption
    {
        public string StringRepresentation;
        public Type BaseType;

        public ChipInputOptionParseable(string stringRepresentation,Type baseType) : base(InputOptionType.Parseable)
        {
            StringRepresentation = stringRepresentation;
            BaseType = baseType;
        }

        public override string ToString()
        {
            return StringRepresentation;
        }

    }
}
