﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    public class ChipInputOptionReference : ChipInputOption
    {
        public ChipTopWithOutput ChipReference;

        public ChipInputOptionReference(ChipTopWithOutput chipReference) : base(InputOptionType.Reference)
        {
            ChipReference = chipReference;
        }

        public override string ToString() => ChipReference.OutputName;
    }
}
