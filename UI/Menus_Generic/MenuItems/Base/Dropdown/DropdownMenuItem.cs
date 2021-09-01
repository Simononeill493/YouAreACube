using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class DropdownMenuItem<T> : TextBoxMenuItem
    {
        public T SelectedItem { get; private set; }
        protected ListMenuItem<T> _list;

        public virtual bool Dropped
        {
            get { return _dropped; }
            set
            {
                _dropped = value;
                _list.Visible = value;
                _list.Enabled = value;
            }
        }
        protected bool _dropped;

        public DropdownMenuItem(IHasDrawLayer parentDrawLayer, List<T> initialItems) : this(parentDrawLayer)
        {
            AddItems(initialItems);
        }
        public DropdownMenuItem(IHasDrawLayer parentDrawLayer) : base(parentDrawLayer, "")
        {
            SpriteName = BuiltInMenuSprites.BasicDropdown;
            TextItem.SetLocationConfig(5, 20, CoordinateMode.ParentPercentageOffset, false);

            _list = new ListMenuItem<T>(ManualDrawLayer.Dropdown, GetBaseSize());
            _list.SetLocationConfig(0, 100, CoordinateMode.ParentPercentageOffset);
            _list.Visible = false;
            _list.Enabled = false;
            _list.HoverHighlight = true;
            _list.OnItemSelected += ListItemSelected;
            AddChild(_list);

            var dropButton = new SpriteMenuItem(this, BuiltInMenuSprites.DropdownArrow);
            dropButton.SetLocationConfig(92, 50, CoordinateMode.ParentPercentageOffset, true);
            AddChild(dropButton);

            this.OnMouseReleased += (i) => { Dropped = !Dropped; };
        }


        public void ManuallySetItem(T item) => SelectedItem = item;
        public void SetItems(List<T> items) => _list.SetItems(items);
        public void AddItems(List<T> toAdd) =>_list.AddItems(toAdd);
        public void ClearItems() => _list.ClearItems();

        private void ListItemSelected(T item)
        {
            SelectedItem = item;
            Dropped = false;

            TextItem.Text = item.ToString();
            OnSelectedChanged?.Invoke(item);
        }


        public override void Update(UserInput input)
        {
            base.Update(input);
            if(Dropped & input.MouseLeftJustPressed & !MouseHovering)
            {
                Dropped = false;
            }
        }

        
        public void RefreshText()
        {
            if(IsItemSelected)
            {
                TextItem.Text = SelectedItem.ToString();
            }

            _list.RefreshText();
        }

        public event Action<T> OnSelectedChanged;
        public bool IsItemSelected => (SelectedItem != null);
    }
}
