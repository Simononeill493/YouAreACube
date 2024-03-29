﻿using System;
using System.Collections.Generic;

namespace IAmACube
{
    abstract class FullScreenMenuItem : ContainerScreenItem
    {
        public Func<IntPoint> _getFullScreenBaseSize;
        public FullScreenMenuItem(Screen parent) : base(parent)
        {
            _getFullScreenBaseSize = () => (parent._currentScreenDimensions / Scale);
        }

        public override void Update(UserInput input)
        {
            base.Update(input);
            _removeOffscreenChildren();
        }

        private IntPoint _offscreenPadding = new IntPoint(100, 100);

        protected virtual List<ScreenItem> _removeOffscreenChildren()
        {
            var toRemove = new List<ScreenItem>();
            var curScreenSize = _getFullScreenBaseSize() * Scale;
            foreach (var child in _children)
            {
                var floaterSize = child.GetCurrentSize();
                //if (child.ActualLocation.X < -(floaterSize.X) | child.ActualLocation.Y < -(floaterSize.Y) | child.ActualLocation.X > curScreenSize.X + floaterSize.X | child.ActualLocation.Y > curScreenSize.Y + floaterSize.Y)
                if (child.ActualLocation.X < -(floaterSize.X + _offscreenPadding.X) | child.ActualLocation.Y < -(floaterSize.Y + _offscreenPadding.Y) | child.ActualLocation.X > curScreenSize.X + floaterSize.X + _offscreenPadding.X | child.ActualLocation.Y > curScreenSize.Y + floaterSize.Y + _offscreenPadding.Y)
                {
                        toRemove.Add(child);
                }
            }

            RemoveChildrenAfterUpdate(toRemove);
            return toRemove;
        }


        public override IntPoint GetBaseSize() => _getFullScreenBaseSize();

    }
}
