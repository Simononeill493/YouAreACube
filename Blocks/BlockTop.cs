using Microsoft.Xna.Framework;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{

    public partial class BlockTop : SpriteMenuItem, IBlocksDroppableOn
    {
        public string Name;
        public int IndexInBlockset = -1;
        public BlockData BlockData;

        public List<string> CurrentTypeArguments = BlockUtils.NoTypeArguments;
        public List<BlockInputSection> InputSections = new List<BlockInputSection>();
        public BlockParentCallbacks Callbacks;

        public bool HasOutput => BlockData.HasOutput;

        public BlockTop(string name,IHasDrawLayer parent, BlockData data) : base(parent, "ChipFull") 
        {
            Name = name;
            BlockData = data;
            ColorMask = BlockData.ChipDataType.GetColor();
            OnMouseDraggedOn += _onDragHandler;

            _topSectionActualSize = base.GetBaseSize();

            _createBlockNameText();
            _createInputSections();
            _setEndSpriteForLastInputSection();
        }

        private void _createBlockNameText()
        {
            var title = _addTextItem(BlockData.Name, 7, 6, CoordinateMode.ParentPixelOffset, false);
            title.Color = Color.White;
        }

        private void _setEndSpriteForLastInputSection()
        {
            if (InputSections.Count > 0)
            {
                InputSections[InputSections.Count - 1].SpriteName = "ChipFullEnd";
            }
        }

        private void _onDragHandler(UserInput input)
        {
            if (!_isMouseOverLowerSection())
            {
                Callbacks.BlockLifted(this, input);
            }
        }

        public virtual void DropBlocksOnThis(List<BlockTop> chips, UserInput input)
        {
            if (IsMouseOverBottomSection())
            {
                Callbacks.AppendBlocks(chips, IndexInBlockset + 1);
            }
            else
            {
                Callbacks.AppendBlocks(chips, IndexInBlockset);
            }
        }

        public List<BlockInputOption> GetCurrentInputs() => InputSections.Select(section => section.CurrentlySelected).ToList();
        public void ManuallySetInputSection(BlockInputOption item,int index) =>InputSections[index].ManuallySetInput(item);
        
        public void SetInputConnectionsFromAbove(List<BlockTop> chipsAbove)
        {
            InputSections.ForEach(m => m.SetConnectionsFromAbove(chipsAbove));

            foreach (var subChipset in GetSubBlocksets())
            {
                var connectionsList = new List<BlockTop>();
                connectionsList.AddRange(chipsAbove);
                subChipset.SetInputConnectionsFromAbove(connectionsList);
            }
        }

        protected void _createInputSections()
        {
            for (int i = 0; i < BlockData.NumInputs; i++)
            {
                var section = BlockSectionFactory.CreateInputSection(this, i);
                section.ItemSelectedCallback = _inputSectionSelectionChanged;
                InputSections.Add(section);
            }

            AddChildren(InputSections);
            _setInputSectionPositions();
        }
        protected void _setInputSectionPositions()
        {
            var height = base.GetBaseSize().Y - 1;

            foreach (var section in InputSections)
            {
                section.SetLocationConfig(0, height, CoordinateMode.ParentPixelOffset);
                height += section.GetBaseSize().Y;
            }

            _topSectionActualSize.Y = height;
        }

        protected virtual void _inputSectionSelectionChanged(BlockInputSection section, BlockInputOption optionSelected)
        {
            if (optionSelected.OptionType == InputOptionType.Reference)
            {
                var referenceOption = (BlockInputOptionReference)optionSelected;

                var inputOptions = section.InputBaseTypes;
                var dataTypeFeedingIn = referenceOption.BlockReference.OutputTypeCurrent;

                if (inputOptions.Contains("List<Variable>"))
                {
                    var afterOpeningList = dataTypeFeedingIn.Substring(5);
                    var extracted = afterOpeningList.Substring(0, afterOpeningList.Length - 1);
                    CurrentTypeArguments[0] = extracted;
                }
                else if (inputOptions.Contains("Variable"))
                {
                    CurrentTypeArguments[0] = dataTypeFeedingIn;
                }
                else if (inputOptions.Contains("AnyCube"))
                {
                    CurrentTypeArguments[0] = dataTypeFeedingIn;
                }

            }
        }

        public bool IsMouseOverAnySection() => MouseHovering | _isMouseOverLowerSection();
        public virtual bool IsMouseOverBottomSection() => (InputSections.Count == 0) || InputSections.Last().MouseHovering;
        protected virtual bool _isMouseOverLowerSection() => InputSections.Select(s => s.MouseHovering).Any(h => h);

        public override IntPoint GetBaseSize() => _topSectionActualSize;
        protected IntPoint _topSectionActualSize;


        protected void _topLevelRefreshAll_Delayed() => _delayedTopLevelRefreshAll = true;
        private bool _delayedTopLevelRefreshAll = false;


        public void RefreshText()
        {
            InputSections.ForEach(s => s.RefreshText());
            GetSubBlocksets().ForEach(s => s.RefreshText());
        }

        public override void Update(UserInput input)
        {
            base.Update(input);

            if(_delayedTopLevelRefreshAll)
            {
                Callbacks.TopLevelRefreshAll();
                _delayedTopLevelRefreshAll = false;
            }
        }

        public IBlocksetGenerator BlocksetGenerator;

        public virtual List<Blockset> GetSubBlocksets() => new List<Blockset>();



        public virtual void RefreshAll() { }
        public virtual void GenerateSubChipsets() { }

        public override string ToString() => Name;
    }
}
