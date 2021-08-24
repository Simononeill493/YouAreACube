using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TemplateEditMenu : SpriteMenuItem
    {
        private TabArrayMenuItem _tabs;
        private TemplateChipsetEditTab _chipsetEditTab;
        private TemplateVariableEditTab _variableEditTab;
        private TemplateBaseStatsEditTab _statsEditTab;
        private TemplateAppearanceEditTab _appearanceEditTab;

        private Action _goBackToTemplateSelector;
        private CubeTemplate _baseTemplate;
        private Kernel _kernel;

        public TemplateEditMenu(IHasDrawLayer parentDrawLayer, Kernel kernel, CubeTemplate baseTemplate, Action goBackToTemplateSelector) : base(parentDrawLayer, BuiltInMenuSprites.LargeMenuRectangle_BlocksetEditWindow)
        {
            _goBackToTemplateSelector = goBackToTemplateSelector;
            _baseTemplate = baseTemplate;
            _kernel = kernel;

            _variableEditTab = new TemplateVariableEditTab(this, baseTemplate);
            _variableEditTab.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, centered: true);
            AddChild(_variableEditTab);

            _chipsetEditTab = new TemplateChipsetEditTab(this, kernel, baseTemplate, _variableEditTab);
            _chipsetEditTab.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, centered: true);
            AddChild(_chipsetEditTab);


            _statsEditTab = new TemplateBaseStatsEditTab(this,baseTemplate);
            _statsEditTab.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, centered: true);
            AddChild(_statsEditTab);

            _appearanceEditTab = new TemplateAppearanceEditTab(this, baseTemplate);
            _appearanceEditTab.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, centered: true);
            AddChild(_appearanceEditTab);

            _tabs = new TabArrayMenuItem(this,MenuOrientation.Horizontal,10);
            _tabs.SetLocationConfig(0, -SpriteManager.GetSpriteSize(BuiltInMenuSprites.BasicTabButton).Y, CoordinateMode.ParentPixelOffset, false);
            _tabs.AddTabButton("Stats", _statsEditTab);
            _tabs.AddTabButton("Variables", _variableEditTab);
            _tabs.AddTabButton("Chipset", _chipsetEditTab).OnMouseReleased += (i) => _chipsetEditTab.RefreshAllBlocksets();
            _tabs.AddTabButton("Appearance", _appearanceEditTab);
            _tabs.SwitchToFirstTab();
            AddChild(_tabs);
        }

        public void OpenQuitDialog()
        {
            var dialogBox = new TemplateQuitDialog(ManualDrawLayer.Dialog, this, _quitDialogButtonPressed);
            dialogBox.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, true);
            dialogBox.AddPausedItems(_tabs.Tabs);

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
                case TemplateQuitButtonOption.SaveAppearanceOnly:
                    UpdateCurrentTemplateAppearance();
                    _goBackToTemplateSelector();
                    break;

            }
        }

        private void _openSaveDialog()
        {
            var versionNumber = _baseTemplate.Versions.GetNewVersionNumber();

            var dialogBox = new TemplateSaveDialog(ManualDrawLayer.Dialog, this, versionNumber, _statsEditTab.CurrentName, _saveDialogButtonPressed);
            dialogBox.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, true);
            dialogBox.AddPausedItems(_tabs.Tabs);

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
                default:
                    throw new Exception();
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

        public void UpdateCurrentTemplateAppearance()
        {
            var (spriteName, spriteType) = _appearanceEditTab.GenerateSpriteData();
            _baseTemplate.Sprite = spriteName;
            _baseTemplate.SpriteType = spriteType;
        }


        private CubeTemplate _createNewTemplateFromThisMenu()
        {
            var template = _baseTemplate.Clone();
            _variableEditTab.AddEditedVariablesToTemplate(template);
            _chipsetEditTab.AddEditedChipsetToTemplate(template);
            _statsEditTab.AddEditedFieldsToTemplate(template);
            _appearanceEditTab.AddEditedAppearanceToTemplate(template);
            return template;
        }

    }
}
