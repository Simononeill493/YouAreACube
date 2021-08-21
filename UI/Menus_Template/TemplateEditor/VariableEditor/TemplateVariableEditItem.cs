using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateVariableEditItem : ContainerMenuItem
    {
        private TextBoxMenuItem _nameBox;

        public TemplateVariableEditItem(IHasDrawLayer parent,int num) : base(parent)
        {
            var number = new TextMenuItem(this, (num+1).ToString() + ":");
            _nameBox = new TextBoxMenuItem(this) { Editable = true };
            var dataType = new DropdownMenuItem<InGameType>(this);
            dataType.AddItems(InGameTypeUtils.InGameTypes);

            AddToContainer(number, 0, -SpriteManager.GetTextSize(number.Text).Y / 2, CoordinateMode.ParentPixelOffset, false);
            AddToContainer(_nameBox, 20, -SpriteManager.GetSpriteSize(_nameBox.SpriteName).Y/2, CoordinateMode.ParentPixelOffset, false);
            AddToContainer(dataType, 130, -SpriteManager.GetSpriteSize(dataType.SpriteName).Y / 2, CoordinateMode.ParentPixelOffset, false);
        }

        public override IntPoint GetBaseSize()
        {
            return _nameBox.GetBaseSize();
        }
    }
}
