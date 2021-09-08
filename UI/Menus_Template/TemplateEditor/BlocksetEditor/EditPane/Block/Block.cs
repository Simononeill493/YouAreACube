using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class Block : ContainerMenuItem
    {
        public BlockModel Model;

        public BlockTop Top;
        public List<SpriteMenuItem> Sections;
        public BlockSwitchSection_2 SwitchSection;

        private Action<Block,UserInput> _draggedCallback;

        public Block(BlockModel model) : base(ManualDrawLayer.Create(DrawLayers.MenuBlockLayer))
        {
            Model = model;

            Draggable = true;
            Sections = new List<SpriteMenuItem>();

            OnStartDrag += _blockDragged;
        }

        public void SetBlocksetParent(Blockset parent)
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

        public List<Blockset> GetSubBlocksets() => SwitchSection != null ? SwitchSection.SubBlocksets.ToList() : new List<Blockset>();
        public override IntPoint GetBaseSize() => this.GetCurrentBlockSize();
        public bool IsHoveringOnBottom => Sections.Last().MouseHovering;

        protected override bool _canStartDragging() => base._canStartDragging() & Top.MouseHovering;
        private void _blockDragged(UserInput input) => _draggedCallback(this, input);
    }
}
