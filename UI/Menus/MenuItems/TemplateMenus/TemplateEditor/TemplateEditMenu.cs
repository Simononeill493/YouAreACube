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
        private BlockTemplate _baseTemplate;
        private Kernel _kernel;

        private ChipEditPane _editPane;
        private ChipSearchPane _searchPane;

        private Action _goBackToTemplateSelectScreen;

        public TemplateEditMenu(IHasDrawLayer parentDrawLayer,Kernel kernel, BlockTemplate baseTemplate, Action goBackToTemplateSelectScreen) : base(parentDrawLayer, "EditPaneWindow")
        {
            _kernel = kernel;
            _baseTemplate = baseTemplate;
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
            _searchPane.AddToEditPane = _editPane.ConfigureNewChipsetFromSearchPaneClick;
            _searchPane.RefreshFilter();

            _editPane.LoadTemplateForEditing(_baseTemplate);
        }

        public void OpenQuitDialog()
        {
            var dialogBox = new TemplateQuitDialog(ManualDrawLayer.Create(DrawLayers.MenuDialogLayer), this, _quitDialogButtonPressed);
            dialogBox.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, true);
            dialogBox.AddPausedItems(_editPane, _searchPane);

            AddChildAfterUpdate(dialogBox);
        }

        private void _quitDialogButtonPressed(TemplateQuitButtonOption option)
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
            var versionNumber = _baseTemplate.Versions.GetNewVersionNumber();
            var dialogBox = new TemplateSaveDialog(ManualDrawLayer.Create(DrawLayers.MenuDialogLayer), this, versionNumber, _saveDialogButtonPressed);
            dialogBox.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, true);
            dialogBox.AddPausedItems(_editPane, _searchPane);

            AddChildAfterUpdate(dialogBox);
        }

        private void _saveDialogButtonPressed(TemplateSaveDialogOption option,string name)
        {
            switch (option)
            {
                case TemplateSaveDialogOption.SaveAsNewTemplate:
                    SaveNewTemplate(name);
                    break;
                case TemplateSaveDialogOption.SaveAsNewVersion:
                    SaveNewVersion(name);
                    break;
            }

            _goBackToTemplateSelectScreen(); 
        }

        public void SaveNewVersion(string name)
        {
            var newTemplate = _createNewTemplateFromThisMenu();
            if (newTemplate == null)
            {
                return;
            }

            newTemplate.Name = name;
            var allVersions = _baseTemplate.Versions;
            var newVersionNum = allVersions.GetNewVersionNumber();
            allVersions[newVersionNum] = newTemplate;
        }


        public void SaveNewTemplate(string name)
        {
            var newTemplate = _createNewTemplateFromThisMenu();
            if(newTemplate== null)
            {
                return;
            }

            newTemplate.Name = "Initial";

            var newTemplateVersions = new TemplateVersionDictionary(name, newTemplate);
            _kernel.AddKnownTemplate(newTemplateVersions);
        }

        private BlockTemplate _createNewTemplateFromThisMenu()
        {
            var template = _baseTemplate.Clone();

            if (_editPane.TopLevelChipsets.Count == 1)
            {
                var chipset = _editPane.TopLevelChipsets.First();
                chipset.Name = "_Initial";

                var json = EditableChipsetToJSONParser.ParseEditableChipsetToJson(chipset);
                var block = JSONToChipBlockParser.ParseJsonToBlock(json);

                TemplateParsingTester.TestParsingRoundTrip(chipset.Name, block);

                template.ChipBlock = block;

                if (!template.Active)
                {
                    template.Active = true;
                    template.Speed = 30;
                }
                return template;
            }

            return null;
        }

        protected override void _drawSelf(DrawingInterface drawingInterface)
        {
            base._drawSelf(drawingInterface);
            this.FillRestOfScreen(drawingInterface, DrawLayer + DrawLayers.MinLayerDistance, Color.Black, 1);
        }
    }
}
