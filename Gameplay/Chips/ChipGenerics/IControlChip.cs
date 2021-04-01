﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public interface IControlChip
    {
        List<ChipBlock> GetSubBlocks();
        void ExecuteOutput(Block actor, UserInput input, ActionsList actions);
    }
}
