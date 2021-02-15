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

        public Point Size;

        public bool IsCentered;

        public virtual void UpdateLocation()
        {
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
