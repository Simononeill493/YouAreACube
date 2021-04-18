﻿using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class DialogBoxMenuItem : DraggableMenuItem
    {
        private MenuItem _container;
        public event Action OnClosed;

        private List<MenuItem> _pausedItems;

        public DialogBoxMenuItem(IHasDrawLayer parentDrawLayer, MenuItem container, string sprite) : base(parentDrawLayer, sprite)
        {
            _container = container;
            _pausedItems = new List<MenuItem>();
        }

        public void AddPausedItems(params MenuItem[] items)
        {
            foreach(var item in items)
            {
                item.Enabled = false;
                _pausedItems.Add(item);
            }
        }

        protected override void _drawSelf(DrawingInterface drawingInterface)
        {
            base._drawSelf(drawingInterface);
            this.FillRestOfScreen(drawingInterface, DrawLayer + DrawLayers.MinLayerDistance, new Color(0,0,0,192), 0);
        }

        public override void Update(UserInput input)
        {
            base.Update(input);
            if(input.IsKeyJustReleased(Microsoft.Xna.Framework.Input.Keys.Escape))
            {
                input.RemoveKeyJustReleased(Microsoft.Xna.Framework.Input.Keys.Escape);
                Close();
            }
        }

        public void Close()
        {
            foreach (var item in _pausedItems)
            {
                item.Enabled = true;
            }

            _container.RemoveChildAfterUpdate(this);
            OnClosed?.Invoke();
        }
    }
}