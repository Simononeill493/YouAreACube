using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class MoveToPixelOffsetAnimation : Animation
    {
        private IntPoint _currentOffset = IntPoint.Zero;
        private IntPoint _targetOffset;

        public MoveToPixelOffsetAnimation(Trigger trigger, Ticker ticker,IntPoint targetOffset) : base(trigger,ticker)
        {
            _targetOffset = targetOffset;
        }

        protected override void _do(ScreenItem item)
        {
            var vector = (_targetOffset - _currentOffset).ToOnes();

            item.OffsetLocationConfig(vector);
            _currentOffset += vector;

            if(_currentOffset == _targetOffset)
            {
                _completed = true;
            }
        }
    }

    class MoveToPixelOffsetSmoothAnimation : Animation
    //This is dodgy as hell, don't actually use it
    //it's just really hacky and has stuff vibrating and whizzing around the screen when you change scale
    {
        private FloatPoint _currentOffset = FloatPoint.Zero;
        private IntPoint _targetOffset;

        private float _pixelPercentage;

        public MoveToPixelOffsetSmoothAnimation(Trigger trigger, Ticker ticker, IntPoint targetOffset,float pixelPercentage) : base(trigger, ticker)
        {
            _targetOffset = targetOffset;
            _pixelPercentage = pixelPercentage;
        }

        protected override void _do(ScreenItem item)
        {
            var currentTarget = _targetOffset * item.Scale;

            var vector = ((currentTarget - _currentOffset).ToOnes() * (item.Scale * _pixelPercentage)).Round;

            item.OffsetLocationConfig(vector);
            _currentOffset += vector;

            var dif = _currentOffset.Round - currentTarget;
            if(dif.X <= vector.X & dif.Y<vector.Y)
            {
                _completed = true;
                item.OffsetLocationConfig(-dif);

                item._locationConfig.loc /= item.Scale;
                item._locationConfig.mode = CoordinateMode.ParentPixel;
                return;
            }

            if (_currentOffset.Round == currentTarget)
            {
                _completed = true;

                item._locationConfig.loc /= item.Scale;
                item._locationConfig.mode = CoordinateMode.ParentPixel;
            }
        }
    }
}
