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
        public BlockTemplate Template;
        private Kernel _kernel;

        private ChipEditPane _editPane;
        private ChipSearchPane _searchPane;

        private Action _goBackToTemplateSelectScreen;

        public TemplateEditMenu(IHasDrawLayer parentDrawLayer,Kernel kernel, BlockTemplate template, Action goBackToTemplateSelectScreen) : base(parentDrawLayer, "EditPaneWindow")
        {
            _kernel = kernel;
            Template = template;
            _goBackToTemplateSelectScreen = goBackToTemplateSelectScreen;

            //var saveButton = new SpriteMenuItem(this, "SaveButton");
            //saveButton.SetLocationConfig(0, -saveButton.GetBaseSize().Y, CoordinateMode.ParentPixelOffset, false);
            //saveButton.OnMouseReleased += (i) => { _saveButtonPressed(); };
            //AddChild(saveButton);

            _editPane = new ChipEditPane(this);
            _editPane.SetLocationConfig(4, 4, CoordinateMode.ParentPixelOffset, false);
            AddChild(_editPane);

            _searchPane = new ChipSearchPane(this);
            _searchPane.SetLocationConfig(84, 50, CoordinateMode.ParentPercentageOffset, true);
            AddChild(_searchPane);

            _editPane.IsMouseOverSearchPane = _searchPane.IsMouseOver;
            _searchPane.AddToEditPane = _editPane.CreateNewChipsetFromSearchChipClick;
            _searchPane.RefreshFilter();

            _editPane.LoadTemplate(Template);
        }

        public void OpenQuitDialog()
        {
            var dialogBox = new TemplateQuitDialog(ManualDrawLayer.Create(DrawLayers.MenuDialogLayer), this, _quitButtonPressed);
            dialogBox.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, true);
            dialogBox.AddPausedItems(_editPane, _searchPane);

            AddChildAfterUpdate(dialogBox);
        }

        private void _quitButtonPressed(TemplateQuitButtonOption option)
        {
            switch (option)
            {
                case TemplateQuitButtonOption.SaveAndQuit:
                    _openSaveDialog();
                    break;
                case TemplateQuitButtonOption.QuitWithoutSaving:
                    _goBackToTemplateSelectScreen();
                    break;
            }
        }

        private void _openSaveDialog()
        {
            var versionNumber = Template.Versions.GetNewVersionNumber();
            var dialogBox = new TemplateSaveDialog(ManualDrawLayer.Create(DrawLayers.MenuDialogLayer), this, versionNumber);
            dialogBox.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, true);
            dialogBox.AddPausedItems(_editPane, _searchPane);

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

                Template.Chips = block;
            }
        }

        protected override void _drawSelf(DrawingInterface drawingInterface)
        {
            base._drawSelf(drawingInterface);
            this.FillRestOfScreen(drawingInterface, DrawLayer + DrawLayers.MinLayerDistance, Color.Black, 1);
        }
    }
}
