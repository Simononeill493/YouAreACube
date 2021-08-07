using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlocksetEditTab : SpriteMenuItem
    {
        private CubeTemplate _baseTemplate;
        private Kernel _kernel;

        private BlocksetEditPane _editPane;
        private BlockSearchPane _searchPane;

        private Action _goBackToTemplateSelectScreen;
        private (InputOptionMenu Menu, BlockInputSection Section) _inputMenuToOpenDetails;

        public BlocksetEditTab(IHasDrawLayer parentDrawLayer,Kernel kernel, CubeTemplate baseTemplate, Action goBackToTemplateSelectScreen) : base(parentDrawLayer, "EditPaneWindow")
        {
            _kernel = kernel;
            _baseTemplate = baseTemplate;
            _goBackToTemplateSelectScreen = goBackToTemplateSelectScreen;

            //var saveButton = new SpriteMenuItem(this, "SaveButton");
            //saveButton.SetLocationConfig(0, -saveButton.GetBaseSize().Y, CoordinateMode.ParentPixelOffset, false);
            //saveButton.OnMouseReleased += (i) => { _saveButtonPressed(); };
            //AddChild(saveButton);

            _editPane = new BlocksetEditPane(this,(menu,section) => _inputMenuToOpenDetails = (menu, section));
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
                OpenInputSectionDialog(_inputMenuToOpenDetails.Menu, _inputMenuToOpenDetails.Section);
                _inputMenuToOpenDetails.Section = null;
            }
        }

        public void OpenInputSectionDialog(InputOptionMenu menu, BlockInputSection section)
        {
            var dialogBox = new BlockTemplateSelectionDialog(ManualDrawLayer.Create(DrawLayers.MenuDialogLayer), this,section,_kernel);
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
    }
}
