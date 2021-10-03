using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class FadeInAnimation : Animation<VisualScreenItem>
    {
        private float _increment;
        public FadeInAnimation(Trigger trigger, Ticker ticker,float increment) : base(trigger, ticker)
        {
            _increment = increment;
        }

        public override void Begin(VisualScreenItem item)
        {
            item.Alpha = 0;
        }

        protected override void _do(VisualScreenItem item)
        {
            var newAlpha = item.Alpha + _increment;
            if(newAlpha>=1)
            {
                _completed = true;
                item.Alpha = 1;
                return;
            }

            item.Alpha = newAlpha;
        }
    }
}
