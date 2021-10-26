using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace IAmACube
{
    class ColorLookup
    {
        public static Dictionary<XnaColors, Color> ColorsDict;

        public static void Init()
        {
            ColorsDict = new Dictionary<XnaColors, Color>();
            ColorsDict[XnaColors.ClearColorMask] = Color.White;
            ColorsDict[XnaColors.DeadCubeColor] = new Color(128, 128, 128, 255);
            ColorsDict[XnaColors.TransparentPink] = new Color(255, 0, 255, 255);
            ColorsDict[XnaColors.DemoStageDarkGroundColor] = new Color(128, 128, 128, 255);
        }
    }

    [Serializable()]
    public enum XnaColors
    {
        ClearColorMask,
        DeadCubeColor,
        TransparentPink,
        DemoStageDarkGroundColor
    }
}
