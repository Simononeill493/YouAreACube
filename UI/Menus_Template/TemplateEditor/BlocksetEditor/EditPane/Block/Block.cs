using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class Block : ContainerScreenItem
    {
        public BlockModel Model;

        public BlockTop Top;
        public List<SpriteScreenItem> Sections;
        public BlockSwitchSection SwitchSection;

        private Action<Block,UserInput> _draggedCallback;
        private Action<BlockInputOption, BlockInputModel> _subMenuCallback;

        public Block(BlockModel model) : base(ManualDrawLayer.Create(DrawLayers.MenuBlockLayer))
        {
            Model = model;

            Draggable = true;
            Sections = new List<SpriteScreenItem>();

            OnStartDrag += _blockDragged;
        }

        public void SetBlocksetParent(Blockset parent)
        {
            _draggedCallback = parent.LiftBlocks;
            _subMenuCallback = parent.OpenSubMenu;
            VisualParent = parent;
        }

        public void DropdownItemSelected(BlockInputOption selectedOption,BlockInputModel seletedModel)
        {
            if(selectedOption.InputOptionType == InputOptionType.SubMenu)
            {
                _subMenuCallback(selectedOption, seletedModel);
            }
        }

        public override void HideAndDisable()
        {
            base.HideAndDisable();
            GetSubBlocksets().ForEach(b => b.HideAndDisable());
        }
        public override void ShowAndEnable()
        {
            base.ShowAndEnable();
            SwitchSection?.ActiveSection?.ShowAndEnable();
        }

        public List<Blockset> GetSubBlocksets() => SwitchSection != null ? SwitchSection.SubBlocksets.ToList() : new List<Blockset>();
        public override IntPoint GetBaseSize() => this.GetCurrentBlockSize();
        public bool IsHoveringOnBottom => Sections.Last().MouseHovering;

        protected override bool _canStartDragging() => base._canStartDragging() & BlocksetEditPane.MouseIsOverPane & Top.MouseHovering;
        private void _blockDragged(UserInput input) => _draggedCallback(this, input);
    }
}
