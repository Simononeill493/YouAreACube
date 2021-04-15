using Microsoft.Xna.Framework;
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

        private ChipEditPane _editPane;
        private ChipSearchPane _searchPane;

        public TemplateEditMenu(IHasDrawLayer parentDrawLayer,Kernel kernel, BlockTemplate template) : base(parentDrawLayer, "EditPaneWindow")
        {
            _kernel = kernel;
            _template = template;

            var saveButton = new SpriteMenuItem(this, "SaveButton");
            saveButton.SetLocationConfig(0, -saveButton.GetBaseSize().Y, CoordinateMode.ParentPixelOffset, false);
            saveButton.OnMouseReleased += (i) => { _saveButtonPressed(); };
            AddChild(saveButton);

            _editPane = new ChipEditPane(this);
            _editPane.SetLocationConfig(4, 4, CoordinateMode.ParentPixelOffset, false);
            AddChild(_editPane);

            _searchPane = new ChipSearchPane(this);
            _searchPane.SetLocationConfig(84, 50, CoordinateMode.ParentPercentageOffset, true);
            AddChild(_searchPane);

            _editPane.IsMouseOverSearchPane = _searchPane.IsMouseOver;
            _searchPane.AddToEditPane = _editPane.CreateNewChipsetFromSearchChipClick;
            _searchPane.RefreshFilter();

            _editPane.LoadTemplate(_template);
        }

        private void _saveButtonPressed()
        {
            var versionNumber = _template.Versions.GetNewVersionNumber();
            var dialogBox = new ChipSaveDialog(ManualDrawLayer.Create(DrawLayers.MenuHoverLayer),this,versionNumber);
            dialogBox.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, true);
            _editPane.Enabled = false;
            _searchPane.Enabled = false;

            dialogBox.OnClosed += () => 
            {
                _editPane.Enabled = true;
                _searchPane.Enabled = true;
            };
            
            AddChildAfterUpdate(dialogBox);
        }

        public void Save()
        {
            if(_editPane.TopLevelChipsets.Count==1)
            {
                var chipset = _editPane.TopLevelChipsets.First();
                chipset.Name = "_Initial";

                var json = EditableChipsetParser.ParseEditableChipsetToJson(chipset);
                var block = ChipBlockParser.ParseJsonToBlock(json);

                _template.Chips = block;
            }
        }

        protected override void _drawSelf(DrawingInterface drawingInterface)
        {
            base._drawSelf(drawingInterface);
            this.FillRestOfScreen(drawingInterface, DrawLayer + DrawLayers.MinLayerDistance, Color.Black, 1);
        }
    }
}
