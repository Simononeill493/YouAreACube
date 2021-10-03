using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    abstract partial class ScreenItem : IHasDrawLayer
    {
        public IntPoint ActualLocation { get; private set; }

        public int Scale => ScaleProvider.GetScale(this);
        public void MultiplyScale(float multiplier) => ScaleProvider.MultiplyManualScale(multiplier);

        public ScreenItemScaleProvider ScaleProvider = new ScreenItemScaleProviderParent();

        public (IntPoint loc, CoordinateMode mode, bool centered) _locationConfig;
        public void ScaleLocation(float factor)
        {
            _locationConfig.loc.X = (int)(_locationConfig.loc.X * factor);
            _locationConfig.loc.Y = (int)(_locationConfig.loc.Y * factor);
        }

        public void SetLocationConfig(int x, int y, CoordinateMode coordinateMode, bool centered = false) => SetLocationConfig(new IntPoint(x, y), coordinateMode, centered);
        public void SetLocationConfig(IntPoint location, CoordinateMode coordinateMode, bool centered = false) => _locationConfig = (location, coordinateMode, centered);
        public void OffsetLocationConfig(IntPoint offset) => _locationConfig.loc += offset;

        public void UpdateLocationCascade(ScreenItem parent) => UpdateLocationCascade(parent.ActualLocation, parent.GetCurrentSize());
        public void UpdateLocationCascade(IntPoint parentlocation, IntPoint parentSize)
        {
            UpdateLocation(parentlocation, parentSize);
            _updateChildLocations();
        }

        public ScreenItem VisualParent;
        
        public void UpdateLocation(IntPoint parentlocation, IntPoint parentSize)
        {
            IntPoint location = _locationConfig.loc;

            if (_locationConfig.mode == CoordinateMode.ParentPixel)
            {
                location = parentlocation + (location * Scale);
            }
            if (_locationConfig.mode == CoordinateMode.ParentAbsolute)
            {
                location = parentlocation + location;
            }
            if (_locationConfig.mode == CoordinateMode.VisualParentPixel)
            {
                location = VisualParent.ActualLocation + (location * Scale);
            }
            else if (_locationConfig.mode == CoordinateMode.ParentPercentage)
            {
                int widthPercent = (int)(parentSize.X * (location.X / 100.0));
                int heightPercent = (int)(parentSize.Y * (location.Y / 100.0));
                var percentageOffset = new IntPoint(widthPercent, heightPercent);

                location = parentlocation + percentageOffset;
            }

            if (_locationConfig.centered)
            {
                location -= (GetCurrentSize() / 2);
            }

            ActualLocation = location;
        }


        protected virtual void _updateChildLocations()
        {
            var size = GetCurrentSize();
            _children.ForEach(child => child.UpdateLocationCascade(ActualLocation, size));
        }

        public static int GenerateScaleFromMultiplier(float multiplier) => (int)(MenuScreen.Scale * multiplier);

        public IntPoint GetLocationOffset(IntPoint mousePos)=> mousePos - ActualLocation;
        public IntPoint GetCurrentSize() => GetBaseSize() * Scale;
        public abstract IntPoint GetBaseSize();


        public bool IsIntersectedWith(ScreenItem item) => IsIntersectedWith(item.GetItemRectangle());
        public bool IsIntersectedWith(Rectangle rect) => GetItemRectangle().Intersects(rect);

        public Rectangle GetItemRectangle()
        {
            var size = GetCurrentSize();
            return new Rectangle(ActualLocation.X, ActualLocation.Y, size.X, size.Y);
        }

    }
}