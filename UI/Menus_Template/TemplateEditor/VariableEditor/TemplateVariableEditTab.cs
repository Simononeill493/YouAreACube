using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateVariableEditTab : SpriteMenuItem, IVariableProvider
    {
        public const int ItemBaseXOffset = 10;
        public const int ItemBaseYOffset = 15;
        public const int VariablesToAdd = 7;

        private List<TemplateVariableEditItem> _items;

        public TemplateVariableEditTab(IHasDrawLayer parent, CubeTemplate baseTemplate) : base(parent, BuiltInMenuSprites.LargeMenuRectangle)
        {
            _items = new List<TemplateVariableEditItem>();
            _addVariableItems();

            LoadVariablesForEditing(baseTemplate);
        }

        private void _addVariableItems()
        {
            for (int i = 0; i < VariablesToAdd; i++)
            {
                _addVariableItem();
            }
        }
        private void _addVariableItem()
        {
            var editItem = new TemplateVariableEditItem(this,_items.Count());
            editItem.SetLocationConfig(ItemBaseXOffset, ItemBaseYOffset + (_items.Count*(editItem.GetBaseSize().Y+2)), CoordinateMode.ParentPixelOffset, false);
            AddChild(editItem);
            _items.Add(editItem);
        }

        public void LoadVariablesForEditing(CubeTemplate template)
        {
            foreach(var variable in template.Variables.Dict)
            {
                var item = _items[variable.Key];
                item.SetData(variable.Value);
            }
        }

        public void AddEditedVariablesToTemplate(CubeTemplate template)
        {
            template.Variables = GetVariables();
        }

        public TemplateVariableSet GetVariables() 
        {
            var variableSet = new TemplateVariableSet();
            foreach(var item in _items)
            {
                if(item.VariableEnabled)
                {
                    variableSet.Dict[item.VariableNumber] = item.MakeVariable();
                }
            }

            return variableSet;
        } 
    }

    public interface IVariableProvider
    {
        TemplateVariableSet GetVariables();
    }
}
