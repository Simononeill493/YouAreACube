using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IAmACube
{
    class DialogBoxMenuItem : SpriteScreenItem
    {
        private ScreenItem _container;
        public event Action OnClosed;

        private Dictionary<ScreenItem,bool> _pausedItems;

        public DialogBoxMenuItem(IHasDrawLayer parentDrawLayer, ScreenItem container, string sprite) : base(parentDrawLayer, sprite)
        {
            _container = container;
            _pausedItems = new Dictionary<ScreenItem, bool>();
        }

        public void AddPausedItems(params ScreenItem[] items) => AddPausedItems(items.ToList());
        public void AddPausedItems(IEnumerable<ScreenItem> items)
        {
            foreach(var item in items)
            {
                _pausedItems[item] = item.Enabled;
                item.Enabled = false;
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
            _pausedItems.ToList().ForEach(kvp => kvp.Key.Enabled = kvp.Value);
            _container.RemoveChildAfterUpdate(this);
            OnClosed?.Invoke();
        }
    }
}
