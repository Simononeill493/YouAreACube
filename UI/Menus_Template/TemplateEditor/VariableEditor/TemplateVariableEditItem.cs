using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace IAmACube
{
    class TemplateVariableEditItem : ContainerScreenItem
    {
        public bool VariableEnabled;

        private TextScreenItem _number;
        private TextBoxMenuItem _nameBox;
        private DropdownMenuItem<InGameType> _dataTypeDropdown;

        private SpriteScreenItem _buttonRemove;
        private SpriteScreenItem _buttonAdd;

        public int VariableNumber => _variable.VariableNumber;
        private string _formattedVariableNumber => (_variable.VariableNumber + 1).ToString() + ":";

        private TemplateVariable _variable;

        public TemplateVariableEditItem(IHasDrawLayer parent,int num) : base(parent)
        {
            _variable = new TemplateVariable(num, "", null);

            _number = new TextScreenItem(this, ()=>_formattedVariableNumber);
            _nameBox = new TextBoxMenuItem(this, () => _variable.VariableName, (v) => { _variable.VariableName = v; }) { Editable = true };
            _dataTypeDropdown = new DropdownMenuItem<InGameType>(this,()=>_variable.VariableType,(v)=> { _variable.VariableType = v; },()=> InGameTypeUtils.InGameTypes.Values.ToList());

            _buttonRemove = new SpriteScreenItem(this, MenuSprites.VariableMinusButton);
            _buttonRemove.OnMouseReleased +=(i) => DisableVariable();

            _buttonAdd = new SpriteScreenItem(this, MenuSprites.VariablePlusButton);
            _buttonAdd.OnMouseReleased += (i) => EnableVariable();

            _addItem(_number, 0, -SpriteManager.GetTextSize(_formattedVariableNumber).Y / 2, CoordinateMode.ParentPixel, false);
            _addItem(_nameBox, 20, -SpriteManager.GetSpriteSize(_nameBox.SpriteName).Y/2, CoordinateMode.ParentPixel, false);
            _addItem(_dataTypeDropdown, 130, -SpriteManager.GetSpriteSize(_dataTypeDropdown.SpriteName).Y / 2, CoordinateMode.ParentPixel, false);
            _addItem(_buttonRemove,208, -SpriteManager.GetSpriteSize(_buttonRemove.SpriteName).Y / 2,CoordinateMode.ParentPixel, false);
            _addItem(_buttonAdd, 208, -SpriteManager.GetSpriteSize(_buttonAdd.SpriteName).Y / 2, CoordinateMode.ParentPixel, false);

            DisableVariable();
        }

        public void SetData(TemplateVariable variable)
        {
            _variable.VariableName = variable.VariableName;
            _variable.VariableType = variable.VariableType;

            EnableVariable();
        }


        public void DisableVariable()
        {
            _number.SetConstantColor(Color.Gray);
            _nameBox.SetConstantColor(Color.Gray);
            _dataTypeDropdown.SetConstantColor(Color.Gray);

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
            _number.SetConstantColor(Color.Black);
            _nameBox.SetConstantColor(Color.White);
            _dataTypeDropdown.SetConstantColor(Color.White);

            _nameBox.Enabled = true;
            _dataTypeDropdown.Enabled = true;

            _buttonRemove.Enabled = true;
            _buttonRemove.Visible = true;

            _buttonAdd.Enabled = false;
            _buttonAdd.Visible = false;

            VariableEnabled = true;
        }

        public TemplateVariable GetVariable() => _variable;

        public override IntPoint GetBaseSize()
        {
            return _nameBox.GetBaseSize();
        }
    }
}
