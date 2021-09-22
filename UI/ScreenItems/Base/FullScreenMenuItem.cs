using System;
using System.Collections.Generic;

namespace IAmACube
{
    abstract class FullScreenMenuItem : ContainerScreenItem
    {
        protected Func<IntPoint> _getFullScreenItemBaseSize;
        public FullScreenMenuItem(IHasDrawLayer parent, Func<IntPoint> parentSizeProvider) : base(parent)
        {
            _getFullScreenItemBaseSize = parentSizeProvider;
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
            var curScreenSize = _getFullScreenItemBaseSize() * Scale;
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


        public override IntPoint GetBaseSize() => _getFullScreenItemBaseSize();

    }
}
