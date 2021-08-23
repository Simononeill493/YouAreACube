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
        public bool VariableEnabled;
        public int VariableNumber;

        private TextMenuItem _number;
        private TextBoxMenuItem _nameBox;
        private DropdownMenuItem<InGameType> _dataTypeDropdown;

        private SpriteMenuItem _buttonRemove;
        private SpriteMenuItem _buttonAdd;

        public TemplateVariableEditItem(IHasDrawLayer parent,int num) : base(parent)
        {
            VariableNumber = num;

            _number = new TextMenuItem(this, (num+1).ToString() + ":");
            _nameBox = new TextBoxMenuItem(this) { Editable = true };
            _dataTypeDropdown = new DropdownMenuItem<InGameType>(this, InGameTypeUtils.InGameTypes);

            _buttonRemove = new SpriteMenuItem(this, BuiltInMenuSprites.VariableMinusButton);
            _buttonRemove.OnMouseReleased +=(i) => DisableVariable();

            _buttonAdd = new SpriteMenuItem(this, BuiltInMenuSprites.VariablePlusButton);
            _buttonAdd.OnMouseReleased += (i) => EnableVariable();

            AddToContainer(_number, 0, -SpriteManager.GetTextSize(_number.Text).Y / 2, CoordinateMode.ParentPixelOffset, false);
            AddToContainer(_nameBox, 20, -SpriteManager.GetSpriteSize(_nameBox.SpriteName).Y/2, CoordinateMode.ParentPixelOffset, false);
            AddToContainer(_dataTypeDropdown, 130, -SpriteManager.GetSpriteSize(_dataTypeDropdown.SpriteName).Y / 2, CoordinateMode.ParentPixelOffset, false);
            AddToContainer(_buttonRemove,208, -SpriteManager.GetSpriteSize(_buttonRemove.SpriteName).Y / 2,CoordinateMode.ParentPixelOffset, false);
            AddToContainer(_buttonAdd, 208, -SpriteManager.GetSpriteSize(_buttonAdd.SpriteName).Y / 2, CoordinateMode.ParentPixelOffset, false);

            DisableVariable();
        }

        public void DisableVariable()
        {
            _number.Color = Color.Gray;
            _nameBox.ColorMask = Color.Gray;
            _dataTypeDropdown.ColorMask = Color.Gray;

            _nameBox.Enabled = false;
            _dataTypeDropdown.Enabled = false;

            _buttonRemove.Enabled = false;
            _buttonRemove.Visible = false;

            _buttonAdd.Enabled = true;
            _buttonAdd.Visible = true;

            VariableEnabled = false;
        }

        public void EnableVariable()
        {
            _number.Color = Color.Black;
            _nameBox.ColorMask = Color.White;
            _dataTypeDropdown.ColorMask = Color.White;

            _nameBox.Enabled = true;
            _dataTypeDropdown.Enabled = true;

            _buttonRemove.Enabled = true;
            _buttonRemove.Visible = true;

            _buttonAdd.Enabled = false;
            _buttonAdd.Visible = false;

            VariableEnabled = true;
        }

        public TemplateVariable MakeVariable() => new TemplateVariable(VariableNumber, _nameBox.Text, _dataTypeDropdown.SelectedItem);

        public override IntPoint GetBaseSize()
        {
            return _nameBox.GetBaseSize();
        }
    }
}
