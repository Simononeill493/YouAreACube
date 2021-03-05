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

        public void AddChild(MenuItem item)
        {
            _children.Add(item);
        }
        public void RemoveChild(MenuItem item)
        {
            _children.Remove(item);
        }
        public void RemoveChildren<T>(List<T> toRemove) where T : MenuItem
        {
            foreach (var item in toRemove)
            {
                _children.Remove(item);
            }
        }
        public void AddChildAfterUpdate(MenuItem item) => _toAdd.Add(item);
        public void AddChildrenAfterUpdate<T>(List<T> items) where T : MenuItem => _toAdd.AddRange(items);

        public void RemoveChildAfterUpdate(MenuItem item) => _toRemove.Add(item);
        public void RemoveChildrenAfterUpdate<T>(List<T> toRemove) where T : MenuItem => _toRemove.AddRange(toRemove);


        private void _updateChildren(UserInput input)
        {
            foreach (var child in _children)
            {
                child.Update(input);
            }
        }
        private void _drawChildren(DrawingInterface drawingInterface)
        {
            foreach (var child in _children)
            {
                child.Draw(drawingInterface);
            }
        }
        private void _addAndRemoveQueuedChildren()
        {
            if (_toAdd.Any())
            {
                var size = GetCurrentSize();
                foreach (var child in _toAdd)
                {
                    _children.Add(child);
                    child.UpdateDimensionsCascade(ActualLocation, size);
                }
            }

            foreach (var child in _toRemove)
            {
                _children.Remove(child);
            }

            _toAdd.Clear();
            _toRemove.Clear();
        }
    }
}
