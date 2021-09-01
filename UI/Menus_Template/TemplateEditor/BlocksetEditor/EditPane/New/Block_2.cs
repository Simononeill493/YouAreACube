using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class Block_2 : ContainerMenuItem
    {
        public Blockset_2 Parent;

        public BlockTop_2 Top;
        public List<SpriteMenuItem> Sections;
        public BlockSwitchSection_2 SwitchSection;

        public Block_2() : base(ManualDrawLayer.Create(DrawLayers.MenuBlockLayer))
        {
            Draggable = true;
            Sections = new List<SpriteMenuItem>();

            OnStartDrag += BlockDragged;
        }

        private void BlockDragged(UserInput input) => Parent.BlockRemoved(this,input);

        public override IntPoint GetBaseSize() => this.GetCurrentBlockSize();
        protected override bool _canStartDragging() => base._canStartDragging() & Top.MouseHovering;

        public bool IsHoveringOnBottom => Sections.Last().MouseHovering;
    }
}
