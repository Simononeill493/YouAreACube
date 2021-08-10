using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    public partial class BlockTop : SpriteMenuItem, IBlocksDroppableOn
    {
        public string Name;
        public BlockData BlockData;
        public int IndexInBlockset = -1;

        public List<string> CurrentTypeArguments = BlockUtils.NoTypeArguments;
        public bool HasOutput => BlockData.HasOutput;

        public IBlocksetTopLevelContainer TopLevelContainer;
        public BlockParentCallbacks Callbacks = BlockParentCallbacks.Empty;

        public BlockTop(string name,IHasDrawLayer parent, BlockData data) : base(parent, BuiltInMenuSprites.Block) 
        {
            Name = name;
            BlockData = data;

            ColorMask = BlockData.ChipDataType.GetColor();
            OnMouseDraggedOn += _onDragHandler;
            _topSectionActualSize = base.GetBaseSize();

            var title = _addTextItem(BlockData.Name, 7, 6, CoordinateMode.ParentPixelOffset, false);
            title.Color = Color.White;

            _createInputSections();
            _setEndSpriteForLastInputSection();
        }

        public virtual void DropBlocksOnThis(List<BlockTop> chips, UserInput input)
        {
            if (IsMouseOverBottomSection())
            {
                Callbacks.ParentAppendBlocks(chips, IndexInBlockset + 1);
            }
            else
            {
                Callbacks.ParentAppendBlocks(chips, IndexInBlockset);
            }
        }

        public override IntPoint GetBaseSize() => _topSectionActualSize;
        protected IntPoint _topSectionActualSize;

        public virtual List<Blockset> GetSubBlocksets() => new List<Blockset>();
        public virtual void GenerateSubBlocksets() { }

        public override string ToString() => Name;
    }
}
