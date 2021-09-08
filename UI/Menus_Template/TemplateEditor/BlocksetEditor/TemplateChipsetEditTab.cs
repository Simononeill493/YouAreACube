using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateChipsetEditTab : SpriteMenuItem
    {
        private CubeTemplate _baseTemplate;
        private Kernel _kernel;

        private BlocksetEditPane _editPane;
        private BlockSearchPane _searchPane;

        public TemplateChipsetEditTab(IHasDrawLayer parentDrawLayer, Kernel kernel, CubeTemplate baseTemplate, IVariableProvider variableProvider) : base(parentDrawLayer, BuiltInMenuSprites.LargeMenuRectangle_BlocksetEditWindow)
        {
            _kernel = kernel;
            _baseTemplate = baseTemplate;

            _searchPane = new BlockSearchPane(this);
            _searchPane.SetLocationConfig(84, 50, CoordinateMode.ParentPercentageOffset, true);
            AddChild(_searchPane);

            _editPane = new BlocksetEditPane(variableProvider,_searchPane);
            _editPane.SetLocationConfig(4, 4, CoordinateMode.ParentPixelOffset, false);
            AddChild(_editPane);

            _searchPane.SendToEditPane = _editPane.RecieveFromSearchPane;

            LoadChipsetForEditing(_baseTemplate);
        }

        protected override void _drawSelf(DrawingInterface drawingInterface)
        {
            base._drawSelf(drawingInterface);
            this.FillRestOfScreen(drawingInterface, DrawLayer + DrawLayers.MinLayerDistance, Color.Black, 1);
        }

        public void LoadChipsetForEditing(CubeTemplate template) => _editPane.LoadChipsetForEditing(template);
        public void AddEditedChipsetToTemplate(CubeTemplate template) => _editPane.AddEditedChipsetToTemplate(template);
    }
}
