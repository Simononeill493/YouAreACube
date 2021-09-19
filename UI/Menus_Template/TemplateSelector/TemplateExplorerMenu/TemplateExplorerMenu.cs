using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{

    abstract class TemplateExplorerMenu : SpriteScreenItem
    {
        protected const int DefaultItemsWidth = 6;
        protected const int DefaultItemsHeight = 6;

        protected Kernel _kernel;
        protected TemplateSelectedMenu templateSelectedMenu;

        public TemplateExplorerMenu(IHasDrawLayer parentDrawLayer,Kernel kernel) : base(parentDrawLayer,MenuSprites.LargeMenuRectangle) 
        {
            _kernel = kernel;

            var templateMatrix = new SpriteBoxMatrix<TemplateBox>(this, DefaultItemsWidth, DefaultItemsHeight);
            templateMatrix.XPadding = 8;
            templateMatrix.YPadding = 8;

            templateMatrix.SetLocationConfig(0, 0, CoordinateMode.ParentPixel, false);
            templateMatrix.AddBoxes(_generateTemplateBoxes(templateMatrix.Capacity));
            templateMatrix.PadToCapacity(_generateEmptyTemplateBox);
            templateMatrix.UpdateBoxLocations();
            AddChild(templateMatrix);
        }


        protected List<TemplateBox> _generateTemplateBoxes(int matrixCapacity)
        {
            var items = new List<TemplateBox>();
            var knownTemplatesList = _getTemplatesToDisplay();
            var numTemplates = knownTemplatesList.Count();

            for (int i = 0; i < numTemplates; i++)
            {
                var box = _generateEmptyTemplateBox();
                box.SetTemplate(knownTemplatesList[i]);
                items.Add(box);

                if(i>matrixCapacity)
                {
                    break;
                }
            }

            return items;
        }

        protected void _templateBoxSelected(TemplateVersionDictionary template) => templateSelectedMenu.SetTemplate(template);

        private TemplateBox _generateEmptyTemplateBox() => new TemplateBox(this, _templateBoxSelected);
        private List<TemplateVersionDictionary> _getTemplatesToDisplay() => _kernel.KnownTemplates.ToList();
    }
}
