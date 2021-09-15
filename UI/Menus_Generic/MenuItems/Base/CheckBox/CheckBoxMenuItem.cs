using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class CheckBoxMenuItem: RectangleMenuItem
    {
        private Func<bool> _checkedProvider;
        private Action<bool> _onSet;

        public CheckBoxMenuItem(IHasDrawLayer parentDrawLayer,Func<bool> checkedProvider,Action<bool> onSet) : base(parentDrawLayer) 
        {
            _checkedProvider = checkedProvider;
            _onSet = onSet;

            this.Size = new IntPoint(10, 10);
            this.Color = Microsoft.Xna.Framework.Color.White;
            OnMouseReleased += (i) => { _onSet(!checkedProvider()); };

            _addTextItem(_getCheckedString, 50, 50, CoordinateMode.ParentPercentage, centered: true);
        }

        private string _getCheckedString()
        {
            if(_checkedProvider())
            {
                return "X";
            }

            return "";
        }
    }
}
