using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateEditMenu : SpriteMenuItem
    {
        BlocksetEditTab _blocksetEditTab;
        Action _goBackToTemplateSelector;
        CubeTemplate _baseTemplate;
        Kernel _kernel;

        public TemplateEditMenu(IHasDrawLayer parentDrawLayer, Kernel kernel, CubeTemplate baseTemplate, Action goBackToTemplateSelector) : base(parentDrawLayer, "EditPaneWindow")
        {
            _goBackToTemplateSelector = goBackToTemplateSelector;
            _baseTemplate = baseTemplate;
            _kernel = kernel;

            _blocksetEditTab = new BlocksetEditTab(this, kernel, baseTemplate, goBackToTemplateSelector);
            _blocksetEditTab.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, centered: true);
            _blocksetEditTab.Enabled = false;
            _blocksetEditTab.Visible = false;
            AddChild(_blocksetEditTab);

            var blankTab = new SpriteMenuItem(this, "EmptyMenuRectangleFull");
            blankTab.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, centered: true);
            blankTab.Enabled = false;
            blankTab.Visible = false;
            AddChild(blankTab);


            var tabs = new TabArrayMenuItem(this);
            tabs.SetLocationConfig(0, 0, CoordinateMode.ParentPercentageOffset, false);
            tabs.AddTab("Chipset", _blocksetEditTab);
            tabs.AddTab("Blank", blankTab);
            tabs.SwitchToFirstTab();
            AddChild(tabs);
        }

        public void OpenQuitDialog()
        {
            var dialogBox = new TemplateQuitDialog(ManualDrawLayer.Create(DrawLayers.MenuDialogLayer), this, _quitDialogButtonPressed);
            dialogBox.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, true);
            dialogBox.AddPausedItems(_blocksetEditTab);

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
                    _goBackToTemplateSelector();
                    break;
            }
        }

        private void _openSaveDialog()
        {
            var versionNumber = _baseTemplate.Versions.GetNewVersionNumber();
            var dialogBox = new TemplateSaveDialog(ManualDrawLayer.Create(DrawLayers.MenuDialogLayer), this, versionNumber, _saveDialogButtonPressed);
            dialogBox.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, true);
            dialogBox.AddPausedItems(_blocksetEditTab);

            AddChildAfterUpdate(dialogBox);
        }

        private void _saveDialogButtonPressed(TemplateSaveDialogOption option, string name)
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

            _goBackToTemplateSelector();
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
            if (newTemplate == null)
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
            _addBlocksetDataToTemplate(template);
            return template;
        }

        private void _addBlocksetDataToTemplate(CubeTemplate template)
        {
            if (_blocksetEditTab.HasOneBlockset)
            {
                var chipset = _blocksetEditTab.GetInitial();

                var json = Parser_BlocksetToJSON.ParseBlocksetToJson(chipset);
                var block = Parser_JSONToChipset.ParseJsonToBlock(json);

                TemplateParsingTester.TestParsingRoundTrip(chipset.Name, block);

                template.Chipset = block;

                if (!template.Active)
                {
                    template.Active = true;
                    template.Speed = 30;
                }
            }
        }
    }
}
