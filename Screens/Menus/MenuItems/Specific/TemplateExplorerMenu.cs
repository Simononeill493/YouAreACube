﻿using System;
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
            _boxes = _generateTemplateBoxes(_templateClick);
            _setTemplateItemsLocations();
        }

        private List<TemplateBox> _generateTemplateBoxes(Action<BlockTemplate> _templateClick)
        {
            var items = new List<TemplateBox>();
            var numTemplates = _kernel.KnownTemplates.Count();
            for (int i = 0; i < ItemsWidth * ItemsHeight; i++)
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
            var xOffset = 5;
            var yOffset = 5;
            var xIncrement = 10;
            var yIncrement = 15;

            for (int i=0;i<_boxes.Count;i++)
            {
                _boxes[i].SetLocationConfig(new Point(xOffset, yOffset), CoordinateMode.Relative, centered: false, attachedToParent: true);
                xOffset += xIncrement;

                if ((i + 1) % ItemsWidth == 0)
                {
                    xOffset = 5;
                    yOffset += yIncrement;
                }
            }
        }

        public override void Draw(DrawingInterface drawingInterface)
        {
            base.Draw(drawingInterface);
        }
    }
}
