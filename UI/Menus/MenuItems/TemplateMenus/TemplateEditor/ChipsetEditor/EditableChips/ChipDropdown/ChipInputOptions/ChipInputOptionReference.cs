using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    public class ChipInputOptionReference : ChipInputOption
    {
        public ChipTopWithOutput ChipReference { get; }
        public override string BaseType => ChipReference.OutputTypeCurrent;


        public ChipInputOptionReference(ChipTopWithOutput chipReference) : base(InputOptionType.Reference)
        {
            ChipReference = chipReference;
        }

        public override string ToString() => ChipReference.OutputName;
    }
}
