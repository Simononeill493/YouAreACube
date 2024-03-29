﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public enum ChipType
    {
        Action,
        Control,
        Sense,
        General,
        Animation,
        Chip
    }

    public static class ChipTypeUtils
    {
        public static Color GetColor(this ChipType chiptype)
        {
            switch (chiptype)
            {
                case ChipType.Action:
                    return Color.DarkRed;
                case ChipType.Control:
                    return Color.DarkGreen;
                case ChipType.Sense:
                    return Color.DarkBlue;
                case ChipType.General:
                    return Color.DimGray;
                case ChipType.Animation:
                    return Color.DarkGoldenrod;
                case ChipType.Chip:
                    return Color.DarkCyan;
                default:
                    throw new Exception("Tried to get a chip color for the chip type '" + chiptype.ToString() + "', which does not have a set color.");
            }
        }
    }
}