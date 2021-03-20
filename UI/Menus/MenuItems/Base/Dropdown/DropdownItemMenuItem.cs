using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class DropdownItemMenuItem<T> : RectangleMenuItem
    {
        public T Item;
        public Color TextColor;

        private TextMenuItem _text;

        public DropdownItemMenuItem(IHasDrawLayer parentDrawLayer,T item,Action<T> itemSelected) : base(parentDrawLayer)
        {
            DrawLayer = DrawLayers.MenuDropdownLayer;

            Color = Color.White;
            TextColor = Color.Black;

            Item = item;

            _text = new TextMenuItem(this) { Text = item.ToString() };
            _text.SetLocationConfig(5, 20, CoordinateMode.ParentPercentageOffset, false);
            AddChild(_text);

            OnMouseReleased += (i) =>
            {
                if(Visible)
                {
                    itemSelected(Item);
                }
            };

            OnMouseStartHover += (i) => 
            { 
                Color = Color.Navy;
                _text.Color = Color.White;
            };

            OnMouseEndHover += (i) => 
            { 
                Color = Color.White;
                _text.Color = Color.Black;
            };
        }

        public void RefreshText()
        {
            _text.Text = Item.ToString();
        }
    }
}
