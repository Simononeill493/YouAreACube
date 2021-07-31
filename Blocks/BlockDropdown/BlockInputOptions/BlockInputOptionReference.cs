using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    public class BlockInputOptionReference : BlockInputOption
    {
        public BlockTopWithOutput BlockReference { get; }
        public override string BaseType => BlockReference.OutputTypeCurrent;


        public BlockInputOptionReference(BlockTopWithOutput blockReference) : base(InputOptionType.Reference)
        {
            BlockReference = blockReference;
        }

        public override string ToString() => BlockReference.OutputName;
    }
}
