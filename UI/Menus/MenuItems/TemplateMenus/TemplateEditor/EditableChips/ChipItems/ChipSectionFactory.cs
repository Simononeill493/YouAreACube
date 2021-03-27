﻿using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ChipSectionFactory
    {
        public static ChipMiddleSection Create(ChipTop parent, int sectionIndex)
        {
            var parentDrawLayer = ManualDrawLayer.InFrontOf(parent, sectionIndex);
            var dataType = parent.Chip.GetInputType(sectionIndex);

            var section = new ChipMiddleSection(parentDrawLayer, dataType, parent.ColorMask);
            return section;
        }
    }
}
