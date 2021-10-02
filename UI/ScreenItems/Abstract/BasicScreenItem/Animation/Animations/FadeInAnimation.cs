using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TextBoxFadeInAnimation : Animation<CorneredBox>
    {
        private float _increment;
        public TextBoxFadeInAnimation(Trigger trigger, Ticker ticker,float increment) : base(trigger, ticker)
        {
            _increment = increment;
        }

        protected override void _do(CorneredBox item)
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
