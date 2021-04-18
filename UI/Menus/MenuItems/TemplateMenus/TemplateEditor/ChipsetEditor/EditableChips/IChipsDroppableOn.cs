﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public interface IChipsDroppableOn
    {
        void DropChipsOn(List<ChipTop> chips, UserInput input);
    }
}