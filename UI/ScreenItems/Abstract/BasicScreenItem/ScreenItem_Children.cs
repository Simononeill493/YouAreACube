﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    abstract partial class ScreenItem : IHasDrawLayer
    {
        public ScreenItem _parent;

        public List<ScreenItem> _children = new List<ScreenItem>();
        private List<ScreenItem> _toAdd = new List<ScreenItem>();
        private List<ScreenItem> _toRemove = new List<ScreenItem>();

        public void AddChild(ScreenItem item)
        {
            item._parent = this;
            _children.Add(item);
        }
        public void RemoveChild(ScreenItem item)
        {
            if(item._parent==this)
            {
                item._parent = null;
            }
            _children.Remove(item);
        }

        public void AddChildren<T>(List<T> toAdd) where T : ScreenItem => toAdd.ForEach(c => AddChild(c));
        public void RemoveChildren<T>(List<T> toRemove) where T : ScreenItem => toRemove.ForEach(c => RemoveChild(c));
        
        public void AddChildAfterUpdate(ScreenItem item) => _toAdd.Add(item);
        public void AddChildrenAfterUpdate<T>(List<T> items) where T : ScreenItem => _toAdd.AddRange(items);

        public void RemoveChildAfterUpdate(ScreenItem item) => _toRemove.Add(item);
        public void RemoveChildrenAfterUpdate<T>(List<T> toRemove) where T : ScreenItem => _toRemove.AddRange(toRemove);

        private void _updateChildren(UserInput input)
        {
            _children.ForEach(child => child.Update(input));
        }
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
            AddChildren(_toAdd);
            RemoveChildren(_toRemove);

            _toAdd.Clear();
            _toRemove.Clear();
        }


        protected T _addItem<T>(T item, int x, int y, CoordinateMode mode, bool centered = false) where T : ScreenItem
        {
            item.SetLocationConfig(x, y, mode, centered);
            AddChild(item);
            return item;
        }

        protected SpriteScreenItem _addSpriteItem(string spriteName, int x, int y, CoordinateMode mode, bool centered)
        {
            var spriteItem = new SpriteScreenItem(this, spriteName);
            spriteItem.SetLocationConfig(x, y, mode, centered);
            AddChild(spriteItem);
            return spriteItem;
        }

        protected TextScreenItem _addStaticTextItem(string text, int x, int y, CoordinateMode mode, bool centered) => _addTextItem(() => text, x, y, mode, centered);
        protected TextScreenItem _addTextItem(Func<string> textProvider, int x, int y, CoordinateMode mode, bool centered)
        {
            var textItem = new TextScreenItem(this, textProvider);
            textItem.SetLocationConfig(x, y, mode, centered);
            AddChild(textItem);
            return textItem;
        }

        protected TextBoxMenuItem _addTextBox(Func<string> getText, Action<string> setText, int x, int y, CoordinateMode mode, bool centered, bool editable = false, int maxTextLength = 9)
        {
            var textBox = new TextBoxMenuItem(this, getText,setText) { Editable = editable, MaxTextLength = maxTextLength };
            textBox.SetLocationConfig(x, y, mode, centered);
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
