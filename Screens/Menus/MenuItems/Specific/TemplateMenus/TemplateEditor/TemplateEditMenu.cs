﻿using System;
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
            var searchPane = new ChipSearchPane(this,chipEditPane.TryCreateChip);
            chipEditPane.Trash = searchPane;

            chipEditPane.SetLocationConfig(34, 50, CoordinateMode.ParentPercentageOffset, true);
            searchPane.SetLocationConfig(84, 50, CoordinateMode.ParentPercentageOffset, true);

            AddChild(chipEditPane);
            AddChild(searchPane);
        }
    }
}
