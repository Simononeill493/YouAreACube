﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockTemplateSelectionDialog : DialogBoxMenuItem
    {
        TemplateExplorerMenu _templateSearchMenu;
        BlockInputModel _model;

        public BlockTemplateSelectionDialog(IHasDrawLayer parentDrawLayer, ScreenItem container, BlockInputModel model,Kernel kernel) : base(parentDrawLayer, container, MenuSprites.BlankPixel)
        {
            _model = model;

            _templateSearchMenu = new TemplateExplorerMenuInternal(this, kernel, CompleteSelection);
            _templateSearchMenu.SetLocationConfig(50, 50, CoordinateMode.ParentPercentage, true);
            _templateSearchMenu.UpdateDrawLayerCascade(DrawLayer - (DrawLayers.MinLayerDistance * 10));
            AddChild(_templateSearchMenu);
        }

        public void CompleteSelection(CubeTemplate template)
        {
            var option = BlockInputOption.CreateValue(template);
            _model.InputOption = option;
            Close();
        }

        public override IntPoint GetBaseSize() => _templateSearchMenu.GetBaseSize();
    }
}
