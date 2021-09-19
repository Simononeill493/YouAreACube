using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ScreenItemScaleProviderStatic :ScreenItemScaleProvider
    {
        private int _staticScale;
        public ScreenItemScaleProviderStatic(int scale)
        {
            _staticScale = scale;
        }

        public override int GetScale(ScreenItem item) => _staticScale;
    }
}
