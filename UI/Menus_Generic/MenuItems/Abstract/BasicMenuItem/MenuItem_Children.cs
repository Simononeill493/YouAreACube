using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public abstract partial class MenuItem : IHasDrawLayer
    {
        private List<MenuItem> _children = new List<MenuItem>();
        private List<MenuItem> _toAdd = new List<MenuItem>();
        private List<MenuItem> _toRemove = new List<MenuItem>();

        public void AddChild(MenuItem item) =>_children.Add(item);
        public void RemoveChild(MenuItem item) => _children.Remove(item);

        public void AddChildren<T>(List<T> items) where T : MenuItem => _children.AddRange(items);
        public void RemoveChildren<T>(List<T> toRemove) where T : MenuItem => toRemove.ForEach(item => _children.Remove(item));
        
        public void AddChildAfterUpdate(MenuItem item) => _toAdd.Add(item);
        public void AddChildrenAfterUpdate<T>(List<T> items) where T : MenuItem => _toAdd.AddRange(items);

        public void RemoveChildAfterUpdate(MenuItem item) => _toRemove.Add(item);
        public void RemoveChildrenAfterUpdate<T>(List<T> toRemove) where T : MenuItem => _toRemove.AddRange(toRemove);

        private void _updateChildren(UserInput input) => _children.ForEach(child => child.Update(input));
        private void _drawChildren(DrawingInterface drawingInterface)=>_children.ForEach(child => child.Draw(drawingInterface));


        public void AddAndRemoveQueuedChildren_Cascade()
        {
            _addAndRemoveQueuedChildren();
            foreach (var child in _children)
            {
                child.AddAndRemoveQueuedChildren_Cascade();
            }
        }
        private void _addAndRemoveQueuedChildren()
        {
            if (_toAdd.Any())
            {
                _children.AddRange(_toAdd);
            }

            _toRemove.ForEach(child => _children.Remove(child));
            _toAdd.Clear();
            _toRemove.Clear();
        }


        protected TextMenuItem _addTextItem(string text, int x, int y, CoordinateMode mode, bool centered)
        {
            var textItem = new TextMenuItem(this, text);
            textItem.SetLocationConfig(x, y, mode, centered);
            AddChild(textItem);
            return textItem;
        }


        protected TextBoxMenuItem _addTextBox(string text, int x, int y, CoordinateMode mode, bool centered, bool editable = false, int maxTextLength =9)
        {
            var textBox = new TextBoxMenuItem(this, text) { Editable = editable, MaxTextLength = maxTextLength };
            textBox.SetLocationConfig(x,y,mode,centered);
            AddChild(textBox);

            return textBox;
        }

        protected ButtonMenuItem _addButton(string text, int x, int y, CoordinateMode mode, bool centered, Action<UserInput> clicked)
        {
            var button = new ButtonMenuItem(this, text);
            button.SetLocationConfig(x,y,mode,centered);
            button.OnMouseReleased += clicked;
            AddChild(button);

            return button;
        }
    }
}
