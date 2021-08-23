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

        public List<TemplateVariable> GetVariables() => _items.Where(i => i.VariableEnabled).Select(ie => ie.MakeVariable()).ToList();
    }

    public interface IVariableProvider
    {
        List<TemplateVariable> GetVariables();
    }
}
