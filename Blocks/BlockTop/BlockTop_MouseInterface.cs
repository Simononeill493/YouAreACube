using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    partial class BlockTop
    {
        public bool IsMouseOverAnySection() => MouseHovering | _isMouseOverLowerSection();
        public virtual bool IsMouseOverBottomSection() => (InputSections.Count == 0) || InputSections.Last().MouseHovering;
        protected virtual bool _isMouseOverLowerSection() => InputSections.Select(s => s.MouseHovering).Any(h => h);

        private void _onDragHandler(UserInput input)
        {
            if (!_isMouseOverLowerSection())
            {
                Callbacks.BlockLifted(this, input);
            }
        }
    }
}
