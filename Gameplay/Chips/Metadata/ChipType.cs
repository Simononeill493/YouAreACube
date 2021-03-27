using Microsoft.Xna.Framework;
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
        General
    }

    public static class ChipTypeMetadata
    {
        public static Color GetColor(this ChipType chiptype)
        {
            switch (chiptype)
            {
                case ChipType.Action:
                    return Microsoft.Xna.Framework.Color.DarkRed;
                case ChipType.Control:
                    return Microsoft.Xna.Framework.Color.DarkGreen;
                case ChipType.Sense:
                    return Microsoft.Xna.Framework.Color.DarkBlue;
                case ChipType.General:
                    return Microsoft.Xna.Framework.Color.DimGray;
                default:
                    throw new Exception();
            }
        }
    }
}