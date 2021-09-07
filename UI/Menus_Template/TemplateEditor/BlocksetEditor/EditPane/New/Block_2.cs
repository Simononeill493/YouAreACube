using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class Block_2 : ContainerMenuItem
    {
        public BlockModel Model;

        public BlockTop_2 Top;
        public List<SpriteMenuItem> Sections;
        public BlockSwitchSection_2 SwitchSection;

        private Action<Block_2,UserInput> _draggedCallback;

        public Block_2(BlockModel model) : base(ManualDrawLayer.Create(DrawLayers.MenuBlockLayer))
        {
            Model = model;

            Draggable = true;
            Sections = new List<SpriteMenuItem>();

            OnStartDrag += _blockDragged;
        }

        public void SetBlocksetParent(Blockset_2 parent)
        {
            _draggedCallback = parent.LiftBlocks;
            VisualParent = parent;
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

        public List<Blockset_2> GetSubBlocksets() => SwitchSection != null ? SwitchSection.SubBlocksets.ToList() : new List<Blockset_2>();
        public override IntPoint GetBaseSize() => this.GetCurrentBlockSize();
        public bool IsHoveringOnBottom => Sections.Last().MouseHovering;

        protected override bool _canStartDragging() => base._canStartDragging() & Top.MouseHovering;
        private void _blockDragged(UserInput input) => _draggedCallback(this, input);
    }
}
