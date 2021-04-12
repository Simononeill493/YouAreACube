using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ListMenuItem<T> : MenuItem
    {
        public T Selected
        {
            get { return _selected; }
            set
            {
                _selected = value;
                OnItemSelected?.Invoke(Selected);
            }
        }
        private T _selected;

        public bool HoverHighlight
        {
            get { return _hoverHighlight; }
            set
            {
                _hoverHighlight = value;
                _items.ForEach(item => item.Hoverable = _hoverHighlight);
            }
        }
        private bool _hoverHighlight;

        public event Action<T> OnItemSelected;

        private List<ListItemMenuItem<T>> _items;
        private Point _listItemSize;

        public ListMenuItem(IHasDrawLayer parentDrawLayer,Point listItemSize) : base(parentDrawLayer)
        {
            _items = new List<ListItemMenuItem<T>>();
            _listItemSize = listItemSize;

            this.OnMousePressed += _clicked;
        }

        private void _clicked(UserInput obj)=>_deselectAll();
        private void _itemSelected(T item)=>Selected = item;
        private void _deselectAll() => _items.ForEach(item => item.Selected = false); 

        public void AddItems(List<T> items)
        {
            var yOffset = (_listItemSize.Y * _items.Count);

            foreach (var item in items)
            {
                var listItem = new ListItemMenuItem<T>(this,item, _itemSelected);
                listItem.Hoverable = _hoverHighlight;
                listItem.MultiplyScaleCascade(this.ScaleMultiplier);
                listItem.Size = _listItemSize;
                listItem.SetLocationConfig(0, yOffset, CoordinateMode.ParentPixelOffset, false);

                _items.Add(listItem);
                AddChild(listItem);

                yOffset += _listItemSize.Y;
            }
        }

        public void SetItems(List<T> items)
        {
            ClearItems();
            AddItems(items);
        }

        public void ClearItems()
        {
            RemoveChildren(_items);
            _items.Clear();
        }

        public void RefreshText() => _items.ForEach(item => item.RefreshText());

        public override Point GetBaseSize()
        {
            var size = _listItemSize;
            size.Y *= _items.Count;
            return size;
        }
    }
}
