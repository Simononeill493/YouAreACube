﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public abstract class MenuItemScaleProvider
    {
        public abstract int GetScale(MenuItem item);

        public virtual float Multiplier => _manualMultiplier;
        private float _manualMultiplier = 1.0f;

        public void MultiplyManualScale(float multiplier)
        {
            if (Math.Abs(multiplier) < 0.0001f)
            {
                throw new Exception("Scale should never be mutiplied by zero");

            }
            _manualMultiplier *= multiplier;
        }
    }

    public class MenuItemScaleProviderParent : MenuItemScaleProvider
    {
        public override int GetScale(MenuItem item) => (item._parent == null) ? 1 : (int)(item._parent.Scale * Multiplier);
    }

    public class MenuItemScaleProviderParent_WithMultiplierFetcher : MenuItemScaleProviderParent
    {
        private Func<float> _fetchMultiplier;
        public MenuItemScaleProviderParent_WithMultiplierFetcher(Func<float> fetchMultiplier)
        {
            _fetchMultiplier = fetchMultiplier;
        }

        public override float Multiplier => _fetchMultiplier();
    }

    public class MenuItemScaleProviderMenuScreen : MenuItemScaleProvider
    {
        public override int GetScale(MenuItem item) => (int)(MenuScreen.Scale * Multiplier);
    }



    public abstract partial class MenuItem : IHasDrawLayer
    {
        public IntPoint ActualLocation { get; private set; }

        public int Scale => ScaleProvider.GetScale(this);
        public void MultiplyScale(float multiplier) => ScaleProvider.MultiplyManualScale(multiplier);

        public MenuItemScaleProvider ScaleProvider = new MenuItemScaleProviderParent();

        protected (IntPoint loc, CoordinateMode mode, bool centered) _locationConfig;
        public void ScaleLocation(float factor)
        {
            _locationConfig.loc.X = (int)(_locationConfig.loc.X * factor);
            _locationConfig.loc.Y = (int)(_locationConfig.loc.Y * factor);
        }

        public void SetLocationConfig(int x, int y, CoordinateMode coordinateMode, bool centered = false) => SetLocationConfig(new IntPoint(x, y), coordinateMode, centered);
        public void SetLocationConfig(IntPoint location, CoordinateMode coordinateMode, bool centered = false) => _locationConfig = (location, coordinateMode, centered);
        public void OffsetLocationConfig(IntPoint offset) => _locationConfig.loc += offset;

        public void UpdateLocationCascade(MenuItem parent) => UpdateLocationCascade(parent.ActualLocation, parent.GetCurrentSize());
        public void UpdateLocationCascade(IntPoint parentlocation, IntPoint parentSize)
        {
            UpdateLocation(parentlocation, parentSize);
            _updateChildLocations();
        }

        
        public void UpdateLocation(IntPoint parentlocation, IntPoint parentSize)
        {
            IntPoint location = _locationConfig.loc;

            if (_locationConfig.mode == CoordinateMode.ParentPixelOffset)
            {
                location = parentlocation + (location * Scale);
            }
            else if (_locationConfig.mode == CoordinateMode.ParentPercentageOffset)
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


        public bool IsIntersectedWith(MenuItem item) => IsIntersectedWith(item.GetItemRectangle());
        public bool IsIntersectedWith(Rectangle rect) => GetItemRectangle().Intersects(rect);

        public Rectangle GetItemRectangle()
        {
            var size = GetCurrentSize();
            return new Rectangle(ActualLocation.X, ActualLocation.Y, size.X, size.Y);
        }

    }
}