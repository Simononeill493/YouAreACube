using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class Block_2 : ContainerMenuItem
    {
        public string Name => Model.Name;
        public BlockModel Model;

        public Blockset_2 Parent;

        public BlockTop_2 Top;
        public List<SpriteMenuItem> Sections;
        public BlockSwitchSection_2 SwitchSection;

        public Block_2(BlockModel model) : base(ManualDrawLayer.Create(DrawLayers.MenuBlockLayer))
        {
            Model = model;

            Draggable = true;
            Sections = new List<SpriteMenuItem>();

            OnStartDrag += BlockDragged;
        }

        private void BlockDragged(UserInput input) => Parent.BlockLifted(this,input);

        public override IntPoint GetBaseSize() => this.GetCurrentBlockSize();
        protected override bool _canStartDragging() => base._canStartDragging() & Top.MouseHovering;

        public bool IsHoveringOnBottom => Sections.Last().MouseHovering;
    }
}
