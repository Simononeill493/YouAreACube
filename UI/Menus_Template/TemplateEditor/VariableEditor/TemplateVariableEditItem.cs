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

        private string _variableName = "";
        private InGameType _selectedType;
        private string _formattedVariableNumber => (VariableNumber + 1).ToString() + ":";

        public TemplateVariableEditItem(IHasDrawLayer parent,int num) : base(parent)
        {
            VariableNumber = num;

            _number = new TextMenuItem(this, ()=>_formattedVariableNumber);
            _nameBox = new TextBoxMenuItem(this, () => _variableName, (v) => { _variableName = v; }) { Editable = true };
            _dataTypeDropdown = new DropdownMenuItem<InGameType>(this,()=>_selectedType,(v)=> { _selectedType = v; },()=> InGameTypeUtils.InGameTypes.Values.ToList());

            _buttonRemove = new SpriteMenuItem(this, BuiltInMenuSprites.VariableMinusButton);
            _buttonRemove.OnMouseReleased +=(i) => DisableVariable();

            _buttonAdd = new SpriteMenuItem(this, BuiltInMenuSprites.VariablePlusButton);
            _buttonAdd.OnMouseReleased += (i) => EnableVariable();

            _addItem(_number, 0, -SpriteManager.GetTextSize(_formattedVariableNumber).Y / 2, CoordinateMode.ParentPixelOffset, false);
            _addItem(_nameBox, 20, -SpriteManager.GetSpriteSize(_nameBox.SpriteName).Y/2, CoordinateMode.ParentPixelOffset, false);
            _addItem(_dataTypeDropdown, 130, -SpriteManager.GetSpriteSize(_dataTypeDropdown.SpriteName).Y / 2, CoordinateMode.ParentPixelOffset, false);
            _addItem(_buttonRemove,208, -SpriteManager.GetSpriteSize(_buttonRemove.SpriteName).Y / 2,CoordinateMode.ParentPixelOffset, false);
            _addItem(_buttonAdd, 208, -SpriteManager.GetSpriteSize(_buttonAdd.SpriteName).Y / 2, CoordinateMode.ParentPixelOffset, false);

            DisableVariable();
        }

        public void SetData(TemplateVariable variable)
        {
            _variableName = variable.VariableName;
            _selectedType = variable.VariableType;

            EnableVariable();
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

        public TemplateVariable MakeVariable() => new TemplateVariable(VariableNumber, _variableName, _selectedType);

        public override IntPoint GetBaseSize()
        {
            return _nameBox.GetBaseSize();
        }
    }
}
