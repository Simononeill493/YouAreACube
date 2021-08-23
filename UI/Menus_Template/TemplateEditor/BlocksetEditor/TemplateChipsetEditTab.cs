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

        private (InputOptionMenu Menu, BlockInputSection Section) _inputMenuToOpenDetails;

        public TemplateChipsetEditTab(IHasDrawLayer parentDrawLayer,Kernel kernel, CubeTemplate baseTemplate,IVariableProvider variableProvider) : base(parentDrawLayer, BuiltInMenuSprites.LargeMenuRectangle_BlocksetEditWindow)
        {
            _kernel = kernel;
            _baseTemplate = baseTemplate;

            //var saveButton = new SpriteMenuItem(this, "SaveButton");
            //saveButton.SetLocationConfig(0, -saveButton.GetBaseSize().Y, CoordinateMode.ParentPixelOffset, false);
            //saveButton.OnMouseReleased += (i) => { _saveButtonPressed(); };
            //AddChild(saveButton);

            _editPane = new BlocksetEditPane(this,(menu,section) => _inputMenuToOpenDetails = (menu, section), variableProvider);
            _editPane.SetLocationConfig(4, 4, CoordinateMode.ParentPixelOffset, false);
            AddChild(_editPane);

            _searchPane = new BlockSearchPane(this);
            _searchPane.SetLocationConfig(84, 50, CoordinateMode.ParentPercentageOffset, true);
            AddChild(_searchPane);

            _editPane.IsMouseOverSearchPane = _searchPane.IsMouseOver;
            _searchPane.AddToEditPane = _editPane.ConfigureNewBlocksetFromSearchPaneClick;
            _searchPane.RefreshFilter();

            LoadChipsetForEditing(_baseTemplate);
        }

        public void LoadChipsetForEditing(CubeTemplate template) => _editPane.LoadTemplateForEditing(template);

        public void AddEditedChipsetToTemplate(CubeTemplate template)
        {
            if (HasOneBlockset)
            {
                var chipset = GetInitial();
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


        public override void Update(UserInput input)
        {
            base.Update(input);

            if(_inputMenuToOpenDetails.Section!=null)
            {
                OpenInputSectionDialog(_inputMenuToOpenDetails.Menu, _inputMenuToOpenDetails.Section);
                _inputMenuToOpenDetails.Section = null;
            }
        }

        public void OpenInputSectionDialog(InputOptionMenu menu, BlockInputSection section)
        {
            var dialogBox = new BlockTemplateSelectionDialog(ManualDrawLayer.Dialog, this,section,_kernel);
            dialogBox.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, true);
            dialogBox.AddPausedItems(_editPane, _searchPane);

            AddChildAfterUpdate(dialogBox);
        }
 

        protected override void _drawSelf(DrawingInterface drawingInterface)
        {
            base._drawSelf(drawingInterface);
            this.FillRestOfScreen(drawingInterface, DrawLayer + DrawLayers.MinLayerDistance, Color.Black, 1);
        }

        public bool HasOneBlockset => _editPane.TopLevelBlockSets.Count == 1;

        public Blockset GetInitial()
        {
            if(!HasOneBlockset)
            {
                throw new Exception();
            }

            var init = _editPane.TopLevelBlockSets.First();
            init.Name = "_Initial";

            return init;
        }


        public void RefreshAllBlocksets() => _editPane.RefreshAllBlocksets();

    }
}
