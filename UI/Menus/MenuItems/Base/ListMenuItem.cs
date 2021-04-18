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
                Items.ForEach(item => item.Hoverable = _hoverHighlight);
            }
        }
        private bool _hoverHighlight;

        public event Action<T> OnItemSelected;

        public List<ListItemMenuItem<T>> Items;
        private Point _listItemSize;

        public ListMenuItem(IHasDrawLayer parentDrawLayer,Point listItemSize) : base(parentDrawLayer)
        {
            Items = new List<ListItemMenuItem<T>>();
            _listItemSize = listItemSize;

            this.OnMousePressed += _clicked;
        }

        private void _clicked(UserInput obj)=>_deselectAll();
        private void _itemSelected(T item)=>Selected = item;
        private void _deselectAll() => Items.ForEach(item => item.Selected = false); 

        public void AddItems(List<T> items)
        {
            var yOffset = (_listItemSize.Y * Items.Count);

            foreach (var item in items)
            {
                var listItem = new ListItemMenuItem<T>(this,item, _itemSelected);
                listItem.Hoverable = _hoverHighlight;
                listItem.MultiplyScaleCascade(this.ScaleMultiplier);
                listItem.Size = _listItemSize;
                listItem.SetLocationConfig(0, yOffset, CoordinateMode.ParentPixelOffset, false);

                Items.Add(listItem);
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
            RemoveChildren(Items);
            Items.Clear();
        }

        public void RefreshText() => Items.ForEach(item => item.RefreshText());

        public override Point GetBaseSize()
        {
            var size = _listItemSize;
            size.Y *= Items.Count;
            return size;
        }
    }
}
