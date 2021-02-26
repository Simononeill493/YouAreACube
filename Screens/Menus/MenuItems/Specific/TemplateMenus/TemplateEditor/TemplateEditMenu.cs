using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateEditMenu : SpriteMenuItem
    {
        private Kernel _kernel;
        private BlockTemplate _template;

        public TemplateEditMenu(IHasDrawLayer parentDrawLayer,Kernel kernel, BlockTemplate template) : base(parentDrawLayer,"EmptyMenuRectangleFull")
        {
            _kernel = kernel;
            _template = template;

            var chipEditPane = new ChipEditPane(this);
            var searchPane = new ChipSearchPane(this,chipEditPane.CreateChip);
            chipEditPane.Trash = searchPane;

            chipEditPane.SetLocationConfig(33, 50, CoordinateMode.ParentPercentageOffset, true);
            searchPane.SetLocationConfig(83, 50, CoordinateMode.ParentPercentageOffset, true);

            AddChild(chipEditPane);
            AddChild(searchPane);
        }
    }
}
