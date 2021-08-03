using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockTemplateSelectionDialog : DialogBoxMenuItem
    {
        TemplateExplorerMenu _templateSearchMenu;
        BlockInputSection _section;

        public BlockTemplateSelectionDialog(IHasDrawLayer parentDrawLayer, MenuItem container, BlockInputSection section,Kernel kernel) : base(parentDrawLayer, container, "BlankPixel")
        {
            _section = section;

            _templateSearchMenu = new TemplateExplorerMenuInternal(this, kernel, CompleteSelection);
            _templateSearchMenu.MakeBoxes();

            //_templateSearchMenu.SpriteName = "EmptyMenuRectangleMedium";
            _templateSearchMenu.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, true);
            _templateSearchMenu.UpdateDrawLayerCascade(DrawLayer - (DrawLayers.MinLayerDistance * 10));
            AddChild(_templateSearchMenu);
        }

        public void CompleteSelection(CubeTemplate template)
        {
            _section.ManuallySetInput(new BlockInputOptionValue(template));
            _section.RefreshText();
            Close();
        }

        public override IntPoint GetBaseSize() => _templateSearchMenu.GetBaseSize();

    }
}
