using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace IAmACube
{
    class TemplateVariableEditItem : ContainerMenuItem
    {
        private TextMenuItem _number;
        private TextBoxMenuItem _nameBox;
        private DropdownMenuItem<InGameType> _dataTypeDropdown;

        private SpriteMenuItem _removeButton;
        private SpriteMenuItem _addButton;

        public TemplateVariableEditItem(IHasDrawLayer parent,int num) : base(parent)
        {
            _number = new TextMenuItem(this, (num+1).ToString() + ":");
            _nameBox = new TextBoxMenuItem(this) { Editable = true };
            _dataTypeDropdown = new DropdownMenuItem<InGameType>(this, InGameTypeUtils.InGameTypes);

            _removeButton = new SpriteMenuItem(this, BuiltInMenuSprites.VariableMinusButton);
            _removeButton.OnMouseReleased +=(i) => Disable();

            _addButton = new SpriteMenuItem(this, BuiltInMenuSprites.VariablePlusButton);
            _addButton.OnMouseReleased += (i) => Enable();

            AddToContainer(_number, 0, -SpriteManager.GetTextSize(_number.Text).Y / 2, CoordinateMode.ParentPixelOffset, false);
            AddToContainer(_nameBox, 20, -SpriteManager.GetSpriteSize(_nameBox.SpriteName).Y/2, CoordinateMode.ParentPixelOffset, false);
            AddToContainer(_dataTypeDropdown, 130, -SpriteManager.GetSpriteSize(_dataTypeDropdown.SpriteName).Y / 2, CoordinateMode.ParentPixelOffset, false);
            AddToContainer(_removeButton,208, -SpriteManager.GetSpriteSize(_removeButton.SpriteName).Y / 2,CoordinateMode.ParentPixelOffset, false);
            AddToContainer(_addButton, 208, -SpriteManager.GetSpriteSize(_addButton.SpriteName).Y / 2, CoordinateMode.ParentPixelOffset, false);

            Disable();
        }

        public void Disable()
        {
            _number.Color = Color.Gray;
            _nameBox.ColorMask = Color.Gray;
            _dataTypeDropdown.ColorMask = Color.Gray;

            _nameBox.Enabled = false;
            _dataTypeDropdown.Enabled = false;

            _removeButton.Enabled = false;
            _removeButton.Visible = false;

            _addButton.Enabled = true;
            _addButton.Visible = true;
        }

        public void Enable()
        {
            _number.Color = Color.Black;
            _nameBox.ColorMask = Color.White;
            _dataTypeDropdown.ColorMask = Color.White;

            _nameBox.Enabled = true;
            _dataTypeDropdown.Enabled = true;

            _removeButton.Enabled = true;
            _removeButton.Visible = true;

            _addButton.Enabled = false;
            _addButton.Visible = false;
        }


        public override IntPoint GetBaseSize()
        {
            return _nameBox.GetBaseSize();
        }
    }
}
