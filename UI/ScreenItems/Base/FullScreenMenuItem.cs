using System;

namespace IAmACube
{
    abstract class FullScreenMenuItem : ContainerMenuItem
    {
        protected Func<IntPoint> _getScreenSize;
        public FullScreenMenuItem(IHasDrawLayer parent, Func<IntPoint> parentSizeProvider) : base(parent)
        {
            _getScreenSize = parentSizeProvider;
        }

        public override IntPoint GetBaseSize() => _getScreenSize();
    }
}
