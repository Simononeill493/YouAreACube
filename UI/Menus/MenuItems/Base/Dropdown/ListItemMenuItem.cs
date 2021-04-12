using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ListItemMenuItem<T> : RectangleMenuItem
    {
        public bool Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                if(_selected)
                {
                    Highlight();
                }
                else
                {
                    ResetColors();
                }
            }
        }
        private bool _selected = false;

        public bool Hoverable;

        public T Item;
        public Color BaseTextColor;

        public Color HighlightColor;
        public Color HighlightTextColor;

        private TextMenuItem _text;
        private Action<T> _selectedCallback;

        public ListItemMenuItem(IHasDrawLayer parentDrawLayer,T item,Action<T> itemSelected) : base(parentDrawLayer)
        {
            _selectedCallback = itemSelected;

            Color = Color.White;
            BaseTextColor = Color.Black;

            HighlightColor = Color.Navy;
            HighlightTextColor = Color.White;

            Item = item;

            _text = new TextMenuItem(this) { Text = item.ToString() };
            _text.SetLocationConfig(5, 20, CoordinateMode.ParentPercentageOffset, false);
            AddChild(_text);

            OnMousePressed += (i) => TrySelectItem();
            OnMouseStartHover += (i) => _startHover();
            OnMouseEndHover += (i) => _endHover();
        }

        public void TrySelectItem()
        {
            Selected = true;
            _selectedCallback(Item);
        }

        public void RefreshText()
        {
            _text.Text = Item.ToString();
        }

        private void _startHover()
        {
            if(Hoverable)
            {
                Highlight();
            }
        }

        private void _endHover()
        {
            if(Hoverable)
            {
                ResetColors();
            }
        }

        public void Highlight()
        {
            Color = Color.Navy;
            _text.Color = Color.White;

        }

        public void ResetColors()
        {
            Color = Color.White;
            _text.Color = BaseTextColor;

        }
    }
}
