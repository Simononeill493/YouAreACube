using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class DropdownItemMenuItem<T> : ShapeMenuItem
    {
        public T Item;
        public Color TextColor;

        public DropdownItemMenuItem(IHasDrawLayer parentDrawLayer,T item,Action<T> itemSelected) : base(parentDrawLayer)
        {
            DrawLayer = DrawLayers.MenuDropdownLayer;

            Color = Color.White;
            TextColor = Color.Black;

            Item = item;

            var text = new TextMenuItem(this) { Text = item.ToString() };
            text.SetLocationConfig(5, 20, CoordinateMode.ParentPercentageOffset, false);
            AddChild(text);

            OnClick += () =>
            {
                if(Visible)
                {
                    itemSelected(Item);
                }
            };

            OnMouseStartHover += () => 
            { 
                Color = Color.Navy;
                text.Color = Color.White;
            };

            OnMouseEndHover += () => 
            { 
                Color = Color.White;
                text.Color = Color.Black;
            };
        }
    }
}
