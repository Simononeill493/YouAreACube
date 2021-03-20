using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class DropdownMenuItem<T> : TextBoxMenuItem
    {
        public bool IsItemSelected = false;

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

        public event Action<T> OnSelectedChanged;

        public DropdownMenuItem(IHasDrawLayer parentDrawLayer) : base(parentDrawLayer, "")
        {
            SpriteName = "Dropdown";

            _items = new List<T>();
            _dropdownItems = new List<DropdownItemMenuItem<T>>();

            this.OnMouseReleased += (i) => { Dropped = !Dropped; };

            var dropButton = new SpriteMenuItem(this, "DropdownArrow");
            dropButton.SetLocationConfig(92, 50, CoordinateMode.ParentPercentageOffset, true);
            AddChild(dropButton);

            text.SetLocationConfig(5, 20, CoordinateMode.ParentPercentageOffset, false);
        }

        public void SetItems(List<T> items)
        {
            _clearDropdownItems();
            _items = items;
            var size = GetCurrentSize() / Scale;

            for (int i = 0; i < _items.Count; i++)
            {
                var dropdownItem = new DropdownItemMenuItem<T>(this, _items[i], DropdownClicked);
                dropdownItem.MultiplyScaleCascade(this.ScaleMultiplier);
                dropdownItem.Size = size;
                dropdownItem.SetLocationConfig(0, 100 * (i + 1), CoordinateMode.ParentPercentageOffset, false);
                dropdownItem.Visible = Dropped;

                _dropdownItems.Add(dropdownItem);
                AddChild(dropdownItem);
            }
        }

        public void AddItems(List<T> toAdd)
        {
            var newItems = new List<T>();
            newItems.AddRange(_items);
            newItems.AddRange(toAdd);

            SetItems(newItems);
        }

        private void DropdownClicked(T item)
        {
            Selected = item;
            Dropped = false;
            IsItemSelected = true;

            text.Text = item.ToString();
            OnSelectedChanged?.Invoke(item);
        }

        private void _clearDropdownItems()
        {
            RemoveChildren(_dropdownItems);
            _dropdownItems.Clear();
        }

        private void _setItemsVisibility(bool visible)
        {
            foreach (var item in _dropdownItems)
            {
                item.Visible = visible;
            }
        }

        public void RefreshText()
        {
            if(IsItemSelected)
            {
                text.Text = Selected.ToString();
            }

            foreach(var item in _dropdownItems)
            {
                item.RefreshText();
            }
        }
    }
}
