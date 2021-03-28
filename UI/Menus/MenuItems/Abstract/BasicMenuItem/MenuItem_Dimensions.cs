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
        public Point ActualLocation { get; private set; }
        public int Scale { get; private set; } = MenuScreen.Scale;
        public float ScaleMultiplier { get; set; } = 1;

        protected (Point loc, CoordinateMode mode, bool centered) _locationConfig;
        public void ScaleLocation(float factor)
        {
            _locationConfig.loc.X = (int)(_locationConfig.loc.X * factor);
            _locationConfig.loc.Y = (int)(_locationConfig.loc.Y * factor);
        }

        public void SetLocationConfig(int x, int y, CoordinateMode coordinateMode, bool centered = false) => SetLocationConfig(new Point(x, y), coordinateMode, centered);
        public void SetLocationConfig(Point location, CoordinateMode coordinateMode, bool centered = false) => _locationConfig = (location, coordinateMode, centered);

        public void UpdateDimensionsCascade(Point parentlocation, Point parentSize)
        {
            UpdateDimensions(parentlocation, parentSize);
            _updateChildDimensions();
        }
        public virtual void UpdateDimensions(Point parentlocation, Point parentSize)
        {
            _updateScale();
            _updateLocation(parentlocation, parentSize);
        }

        public void MultiplyScaleCascade(float multiplier)
        {
            MultiplyScale(multiplier);
            foreach (var child in _children)
            {
                child.MultiplyScaleCascade(multiplier);
            }
        }
        public void MultiplyScale(float multiplier) => ScaleMultiplier *= multiplier;
        

        protected void _updateLocation(Point parentlocation, Point parentSize)
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
        protected void _updateScale() => Scale = GenerateScaleFromMultiplier(ScaleMultiplier);


        protected virtual void _updateChildDimensions()
        {
            var size = GetCurrentSize();
            foreach (var child in _children)
            {
                child.UpdateDimensionsCascade(ActualLocation, size);
            }
        }

        public static int GenerateScaleFromMultiplier(float multiplier) => (int)(MenuScreen.Scale * multiplier);

        public Point GetLocationOffset(Point mousePos)=> mousePos - ActualLocation;
        public Point GetCurrentSize() => GetBaseSize() * Scale;
        public abstract Point GetBaseSize();


        public bool IsIntersectedWith(MenuItem item) => IsIntersectedWith(item.GetItemRectangle());
        public bool IsIntersectedWith(Rectangle rect) => GetItemRectangle().Intersects(rect);

        public Rectangle GetItemRectangle()
        {
            var size = GetCurrentSize();
            return new Rectangle(ActualLocation.X, ActualLocation.Y, size.X, size.Y);
        }

    }
}