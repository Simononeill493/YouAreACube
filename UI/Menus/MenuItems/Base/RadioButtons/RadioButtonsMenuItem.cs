using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class RadioButtonsMenuItem<T> : MenuItem
    {
        public int ButtonGapPixels = 4;
        private List<RadioButtonMenuItem<T>> _buttons;
        public event Action<T> OnItemSelected;

        public RadioButtonsMenuItem(IHasDrawLayer parent) : base(parent)
        {
            _buttons = new List<RadioButtonMenuItem<T>>();
        }

        public void AddOption(T option,string text)
        {
            var currentSize = GetBaseSize();

            var button = new RadioButtonMenuItem<T>(this, option,text);
            button.ButtonIndex = _buttons.Count;
            button.SetLocationConfig(0, currentSize.Y, CoordinateMode.ParentPixelOffset);
            button.OnMouseReleased += (i) => _radioButtonClicked(button.ButtonIndex);

            _buttons.Add(button);
            AddChildAfterUpdate(button);
        }

        protected void _radioButtonClicked(int buttonIndex)
        {
            foreach(var button in _buttons)
            {
                if(button.ButtonIndex == buttonIndex)
                {
                    button.SetButton();
                    OnItemSelected?.Invoke(button.Option);
                }
                else
                {
                    button.ClearButton();
                }
            }
        }

        public override Point GetBaseSize()
        {
            var size = Point.Zero;
            foreach(var button in _buttons)
            {
                var buttonSize = button.GetBaseSize();
                if(buttonSize.X>size.X)
                {
                    size.X = buttonSize.X;
                }

                size.Y += buttonSize.Y + ButtonGapPixels;
            }

            return size;
        }
    }
}
