using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class MenuItemDimensions
    {
        public Point ActualLocation;
        protected (Point location, CoordinateMode mode) LocationConfig;

        public bool IsCentered;

        public int Scale = 1;

        public virtual void Update(int scale)
        {
            Scale = scale;

            switch (LocationConfig.mode)
            {
                case CoordinateMode.Absolute:
                    ActualLocation = LocationConfig.location;
                    break;
                case CoordinateMode.Relative:
                    ActualLocation = DrawUtils.ScreenPercentageToCoords(LocationConfig.location);
                    break;
            }
        }

        public void SetLocationConfig(int x, int y, CoordinateMode positioningMode)
        {
            SetLocationConfig(new Point(x, y), positioningMode);
        }
        public void SetLocationConfig(Point pos, CoordinateMode positioningMode)
        {
            LocationConfig = (pos, positioningMode);
        }

        public MenuItemDimensions ShallowCopy() => (MenuItemDimensions)this.MemberwiseClone();
    }
}
