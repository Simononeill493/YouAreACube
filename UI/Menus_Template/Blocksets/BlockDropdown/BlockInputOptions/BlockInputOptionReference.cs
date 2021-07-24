using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    public class BlockInputOptionReference : BlockInputOption
    {
        public BlockTopWithOutput ChipReference { get; }
        public override string BaseType => ChipReference.OutputTypeCurrent;


        public BlockInputOptionReference(BlockTopWithOutput chipReference) : base(InputOptionType.Reference)
        {
            ChipReference = chipReference;
        }

        public override string ToString() => ChipReference.OutputName;
    }
}
