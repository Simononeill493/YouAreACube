using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateMenu : SpriteMenuItem
    {
        private Kernel _kernel;
        private List<TemplateItem> _items;

        public TemplateMenu(Kernel kernel) : base("EmptyMenuRectangleFull") 
        {
            _kernel = kernel;
            SetLocation(50, 50, CoordinateMode.Relative, centered: true);
            _items = _makeItemsFromKernelTemplates();
            _setTemplateItemsLocations();
        }

        private List<TemplateItem> _makeItemsFromKernelTemplates()
        {
            var items = new List<TemplateItem>();
            foreach (var template in _kernel.KnownTemplates)
            {
                var item = new TemplateItem(template);
                items.Add(item);
                AddChild(item);
            }

            return items;
        }

        private void _setTemplateItemsLocations()
        {
            var sizeInc = GetSize() / 10;
            int lengthInc = sizeInc.X;
            int xOffs = lengthInc/2;

            foreach (var item in _items)
            {
                item.SetLocation(Location + new Point(xOffs, sizeInc.Y/2), CoordinateMode.Absolute, centered: false);
                xOffs += lengthInc;
            }
        }

        public override void Draw(DrawingInterface drawingInterface)
        {
            base.Draw(drawingInterface);
        }

        public override void RefreshDimensions()
        {
            _refreshOwnDimensions();
            _setTemplateItemsLocations();
        }
    }
}
