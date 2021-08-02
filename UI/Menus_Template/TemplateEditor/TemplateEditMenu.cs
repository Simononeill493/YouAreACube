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
        private CubeTemplate _baseTemplate;
        private Kernel _kernel;

        private BlockEditPane _editPane;
        private BlockSearchPane _searchPane;

        private Action _goBackToTemplateSelectScreen;
        private (InputOptionMenu Menu, BlockInputSection Section) _inputMenuToOpenDetails;

        public TemplateEditMenu(IHasDrawLayer parentDrawLayer,Kernel kernel, CubeTemplate baseTemplate, Action goBackToTemplateSelectScreen) : base(parentDrawLayer, "EditPaneWindow")
        {
            _kernel = kernel;
            _baseTemplate = baseTemplate;
            _goBackToTemplateSelectScreen = goBackToTemplateSelectScreen;

            //var saveButton = new SpriteMenuItem(this, "SaveButton");
            //saveButton.SetLocationConfig(0, -saveButton.GetBaseSize().Y, CoordinateMode.ParentPixelOffset, false);
            //saveButton.OnMouseReleased += (i) => { _saveButtonPressed(); };
            //AddChild(saveButton);

            _editPane = new BlockEditPane(this,(menu,section) => _inputMenuToOpenDetails = (menu, section));
            _editPane.SetLocationConfig(4, 4, CoordinateMode.ParentPixelOffset, false);
            AddChild(_editPane);

            _searchPane = new BlockSearchPane(this);
            _searchPane.SetLocationConfig(84, 50, CoordinateMode.ParentPercentageOffset, true);
            AddChild(_searchPane);

            _editPane.IsMouseOverSearchPane = _searchPane.IsMouseOver;
            _searchPane.AddToEditPane = _editPane.ConfigureNewBlocksetFromSearchPaneClick;
            _searchPane.RefreshFilter();

            _editPane.LoadTemplateForEditing(_baseTemplate);
        }

        public override void Update(UserInput input)
        {
            base.Update(input);

            if(_inputMenuToOpenDetails.Section!=null)
            {
                EditPaneMenuCallback(_inputMenuToOpenDetails.Menu, _inputMenuToOpenDetails.Section);
                _inputMenuToOpenDetails.Section = null;
            }
        }

        public void EditPaneMenuCallback(InputOptionMenu menu, BlockInputSection section)
        {
            var templateSearchPane = new TemplateExplorerMenu(this, _kernel, (a) => { });
            templateSearchPane.SpriteName = "EmptyMenuRectangleMedium";
            templateSearchPane.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, true);
            templateSearchPane.UpdateDrawLayerCascade(DrawLayer - (DrawLayers.MinLayerDistance * 10));
            AddChild(templateSearchPane);
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

        private CubeTemplate _createNewTemplateFromThisMenu()
        {
            var template = _baseTemplate.Clone();

            if (_editPane.TopLevelChipsets.Count == 1)
            {
                var chipset = _editPane.TopLevelChipsets.First();
                chipset.Name = "_Initial";

                var json = Parser_BlocksetToJSON.ParseBlocksetToJson(chipset);
                var block = Parser_JSONToChipset.ParseJsonToBlock(json);

                TemplateParsingTester.TestParsingRoundTrip(chipset.Name, block);

                template.Chipset = block;

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
