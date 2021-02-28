using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public abstract partial class MenuItem : IHasDrawLayer
    {
        public bool HalfScaled = false;

        public Point ActualLocation { get; private set; }
        public int Scale => (MenuScreen.Scale + ScaleOffset) / (HalfScaled ? 2 : 1);
        public int ScaleOffset { get; private set; }

        protected (Point loc, CoordinateMode mode, bool centered) _locationConfig;
        public void DownscaleLocation(int factor) => _locationConfig.loc /= factor;
        public void UpscaleLocation(int factor) => _locationConfig.loc *= factor;

        public void SetLocationConfig(int x, int y, CoordinateMode coordinateMode, bool centered = false) => SetLocationConfig(new Point(x, y), coordinateMode, centered);
        public void SetLocationConfig(Point location, CoordinateMode coordinateMode, bool centered = false)
        {
            _locationConfig = (location, coordinateMode, centered);
        }
       
        public virtual void UpdateThisAndChildLocations(Point parentlocation, Point parentSize)
        {
            UpdateLocation(parentlocation, parentSize);
            _updateChildLocations();
        }

        public virtual void UpdateScaleOffsetCascade(int offset)
        {
            ScaleOffset += offset;
            foreach(var child in _children)
            {
                child.UpdateScaleOffsetCascade(offset);
            }
        }

        public void UpdateLocation(Point parentlocation, Point parentSize)
        {
            Point location = _locationConfig.loc;

            if (_locationConfig.mode == CoordinateMode.ParentPixelOffset)
            {
                location = parentlocation + (location * Scale);
            }
            else if (_locationConfig.mode == CoordinateMode.ParentPercentageOffset)
            {
                int widthPercent = (int)(parentSize.X * (location.X / 100.0));
                int heightPercent = (int)(parentSize.Y * (location.Y / 100.0));
                var percentageOffset = new Point(widthPercent, heightPercent);

                location = parentlocation + percentageOffset;
            }

            if (_locationConfig.centered)
            {
                location = location - (GetCurrentSize() / 2);
            }

            ActualLocation = location;
        }

        
        public Point GetLocationOffset(Point mousePos)
        {
            return mousePos - ActualLocation;
        }
        public Point GetCurrentSize() => GetBaseSize() * Scale;
        public abstract Point GetBaseSize();
    }
}