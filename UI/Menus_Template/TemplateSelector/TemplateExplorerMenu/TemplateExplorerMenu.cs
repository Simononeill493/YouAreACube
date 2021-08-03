using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{

    abstract class TemplateExplorerMenu : SpriteMenuItem
    {
        protected const int DefaultItemsWidth = 6;
        protected const int DefaultItemsHeight = 6;
        protected const int DefaultItemsIncrement = 25;

        protected Kernel _kernel;
        protected List<TemplateBox> _boxes;
        protected TemplateSelectedMenu templateSelectedMenu;

        public TemplateExplorerMenu(IHasDrawLayer parentDrawLayer,Kernel kernel) : base(parentDrawLayer,"EmptyMenuRectangleFull") 
        {
            _kernel = kernel;
        }

        public void MakeBoxes(int width = DefaultItemsWidth,int height = DefaultItemsHeight,int increment = DefaultItemsIncrement)
        {
            _boxes = _generateTemplateBoxes(width,height);
            _setTemplateItemsLocations(width,height,increment);
        }

        protected void _templateBoxClicked(TemplateVersionDictionary template)
        {
            if(template!=null)
            {
                templateSelectedMenu.SetTemplate(template);
            }
        }

        protected List<TemplateBox> _generateTemplateBoxes(int width, int height)
        {
            var items = new List<TemplateBox>();
            var numTemplates = _kernel.KnownTemplates.Count();
            var knownTemplatesList = _kernel.KnownTemplates.ToList();

            for (int i = 0; i < width * height; i++)
            {
                var box = new TemplateBox(this, _templateBoxClicked);
                if(i<numTemplates)
                {
                    box.SetTemplate(knownTemplatesList[i]);
                }
                items.Add(box);
                AddChild(box);
            }

            return items;
        }

        protected void _setTemplateItemsLocations(int width,int height,int increment)
        {
            var xPadding = 8;

            var xOffset = xPadding;
            var yOffset = 8;
            var xIncrement = increment;
            var yIncrement = increment;

            for (int i=0;i<_boxes.Count;i++)
            {
                _boxes[i].SetLocationConfig(new IntPoint(xOffset, yOffset), CoordinateMode.ParentPixelOffset, centered: false);
                xOffset += xIncrement;

                if ((i + 1) % width == 0)
                {
                    xOffset = xPadding;
                    yOffset += yIncrement;
                }
            }
        }
    }
}
