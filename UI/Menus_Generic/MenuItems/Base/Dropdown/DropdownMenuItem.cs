using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class DropdownMenuItem<T> : TextBoxMenuItem
    {
        public event Action<T> OnSelectedChanged;

        public bool Dropped
        {
            get { return _dropped; }
            set
            {
                if (value) { PopulateItems(); }

                _dropped = value;
                _list.Visible = value;
                _list.Enabled = value;
            }
        }
        protected bool _dropped;

        protected ListMenuItem<T> _list;
        private Action<T> _selectItem;
        private Func<T> _getSelectedItem;
        private Func<List<T>> _getOptionsDefault;

        public DropdownMenuItem(IHasDrawLayer parentDrawLayer,Func<T> getSelectedItem,Action<T> selectItem,Func<List<T>> getOptions) : base(parentDrawLayer,()=>_selectedItemToString(getSelectedItem()))
        {
            _getSelectedItem = getSelectedItem;
            _selectItem = selectItem;
            _getOptionsDefault = getOptions;

            SpriteName = MenuSprites.BasicDropdown;
            _textItem.SetLocationConfig(5, 20, CoordinateMode.ParentPercentage, false);

            _list = new ListMenuItem<T>(ManualDrawLayer.Dropdown, GetBaseSize());
            _list.SetLocationConfig(0, 100, CoordinateMode.ParentPercentage);
            _list.Visible = false;
            _list.Enabled = false;
            _list.HoverHighlight = true;
            _list.OnItemSelected += ListItemSelected;
            AddChild(_list);

            var dropButton = new SpriteMenuItem(this, MenuSprites.DropdownArrow);
            dropButton.SetLocationConfig(92, 50, CoordinateMode.ParentPercentage, true);
            AddChild(dropButton);

            this.OnMouseReleased += (i) => { Dropped = !Dropped; };
        }

        public virtual void PopulateItems()
        {
            _list.SetItems(_getOptionsDefault());
        }

        protected virtual void ListItemSelected(T item)
        {
            _selectItem(item);
            Dropped = false;
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

        private static string _selectedItemToString(T item)
        {
            if (item == null)
            {
                return "";
            }

            return item.ToString();
        }

        public bool _isItemSelected => (_getSelectedItem() != null);
    }
}
