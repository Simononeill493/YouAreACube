﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ListItemMenuItem<T> : RectangleScreenItem
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

        private TextScreenItem _text;
        private Action<T> _selectedCallback;

        public ListItemMenuItem(IHasDrawLayer parentDrawLayer,T item,Action<T> itemSelected) : base(parentDrawLayer)
        {
            _selectedCallback = itemSelected;

            SetConstantColor(Color.White);
            BaseTextColor = Color.Black;

            HighlightColor = Color.Navy;
            HighlightTextColor = Color.White;

            Item = item;

            _text = new TextScreenItem(this, ()=>item.ToString());
            _text.SetLocationConfig(5, 20, CoordinateMode.ParentPercentage, false);
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
            SetConstantColor(Color.Navy);
            _text.SetConstantColor(Color.White);

        }

        public void ResetColors()
        {
            SetConstantColor(Color.White);
            _text.SetConstantColor(BaseTextColor);

        }
    }
}
