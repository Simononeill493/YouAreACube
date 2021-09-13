using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class CheckBoxMenuItem: RectangleMenuItem
    {
        public bool Checked;
        private TextMenuItem X;

        public CheckBoxMenuItem(IHasDrawLayer parentDrawLayer) : base(parentDrawLayer) 
        {
            this.Size = new IntPoint(10, 10);
            this.Color = Microsoft.Xna.Framework.Color.White;
            OnMouseReleased += (i) => { Set(!this.Checked); };

            X = _addStaticTextItem("X", 50, 50, CoordinateMode.ParentPercentageOffset, centered: true);
            Set(true);
        }

        public void Set(bool toSetTo)
        {
            Checked = toSetTo;
            X.Visible = toSetTo;
        }
    }
}
