﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class DropdownMenuItem<T> : SpriteMenuItem
    {
        public T Selected { get; private set; }
        public bool Dropped
        {
            get { return _dropped; }
            set
            {
                _dropped = value;
                _setItemsVisibility(value);
            }
        }
        private bool _dropped;

        private List<T> _items;
        private List<DropdownItemMenuItem<T>> _dropdownItems;

        private TextMenuItem _text;

        public DropdownMenuItem(IHasDrawLayer parentDrawLayer) : base(parentDrawLayer, "Dropdown")
        {
            _items = new List<T>();
            _dropdownItems = new List<DropdownItemMenuItem<T>>();

            this.OnClick += () => { Dropped = !Dropped; };

            var dropButton = new SpriteMenuItem(this, "DropdownArrow");
            dropButton.SetLocationConfig(92, 50, CoordinateMode.ParentPercentageOffset, true);
            AddChild(dropButton);

            _text = new TextMenuItem(this) { Text = "" };
            _text.SetLocationConfig(5, 20, CoordinateMode.ParentPercentageOffset, false);
            AddChild(_text);
        }

        public void SetItems(List<T> items)
        {
            _clearDropdown();
            _items = items;
            var size = GetSize() / Scale;

            for (int i = 0; i < _items.Count; i++)
            {
                var dropdownItem = new DropdownItemMenuItem<T>(this, _items[i], DropdownClicked);
                dropdownItem.Size = size;
                dropdownItem.SetLocationConfig(0, 100 * (i + 1), CoordinateMode.ParentPercentageOffset, false);
                dropdownItem.Visible = Dropped;

                _dropdownItems.Add(dropdownItem);
                AddChild(dropdownItem);
            }
        }


        protected override void _drawSelf(DrawingInterface drawingInterface)
        {
            base._drawSelf(drawingInterface);
        }

        private void DropdownClicked(T item)
        {
            Selected = item;
            Dropped = false;

            _text.Text = item.ToString();
        }

        private void _clearDropdown()
        {
            foreach(var item in _dropdownItems)
            {
                _children.Remove(item);
            }

            _dropdownItems.Clear();
        }
        private void _setItemsVisibility(bool visible)
        {
            foreach (var item in _dropdownItems)
            {
                item.Visible = visible;
            }
        }


    }
}
