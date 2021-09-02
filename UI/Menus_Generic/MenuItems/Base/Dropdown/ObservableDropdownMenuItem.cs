using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    abstract class ObservableDropdownMenuItem<T> : DropdownMenuItem<T>
    {
        public override T SelectedItem { get { throw new NotImplementedException(); } protected set { throw new NotImplementedException(); } }
        public override bool Dropped
        {
            get { return _dropped; }
            set
            {
                if (value) { PopulateItems(); }
                base.Dropped = value;
            }
        }

        protected ObservableTextMenuItem ObservableText;

        public ObservableDropdownMenuItem(IHasDrawLayer parentDrawLayer, Func<string> textProvider) : base(parentDrawLayer) 
        {
            ObservableText.TextProvider = textProvider;
        }

        protected override void _setTextItem(string initialString = "")
        {
            ObservableText = new ObservableTextMenuItem(this);
            ObservableText.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, centered: true);
            AddChild(ObservableText);

            TextItem = ObservableText;
        }

        protected override void ListItemSelected(T item)
        {
            Dropped = false;
            InvokeSelectedChanged(item);
        }

        public abstract void PopulateItems();

        public override void ManuallySetItem(T item) => throw new NotImplementedException();
        public override void RefreshText() => throw new NotImplementedException();
    }
}
