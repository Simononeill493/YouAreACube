using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateExplorerMenu : SpriteMenuItem
    {
        private const int ItemsWidth = 8;
        private const int ItemsHeight = 6;

        private Kernel _kernel;
        private List<TemplateBox> _boxes;

        public TemplateExplorerMenu(Kernel kernel,Action<BlockTemplate> _templateClick) : base("EmptyMenuRectangleFull") 
        {
            _kernel = kernel;
            SetLocation(50, 50, CoordinateMode.Relative, centered: true);
            _boxes = _generateTemplateBoxes(_templateClick);
            _setTemplateItemsLocations();
        }

        private List<TemplateBox> _generateTemplateBoxes(Action<BlockTemplate> _templateClick)
        {
            var items = new List<TemplateBox>();
            var numTemplates = _kernel.KnownTemplates.Count();
            for (int i = 0; i < ItemsWidth*ItemsHeight; i++)
            {
                var box = new TemplateBox(_templateClick);
                if(i<numTemplates)
                {
                    box.Template = _kernel.KnownTemplates[i];
                }
                items.Add(box);
                AddChild(box);
            }

            return items;
        }

        private void _setTemplateItemsLocations()
        {
            var size = GetSize();
            var increment = size.X / 10;

            var xOffset = increment / 2;
            var yOffset = increment / 2;

            for (int i = 0; i < _boxes.Count; i++)
            {
                _boxes[i].SetLocation(Location + new Point(xOffset, yOffset), CoordinateMode.Absolute, centered: false);
                xOffset += increment;

                if((i+1)% ItemsWidth == 0)
                {
                    xOffset = increment / 2;
                    yOffset += increment;
                }
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
