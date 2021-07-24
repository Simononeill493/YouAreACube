using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    public class BlockTop : SpriteMenuItem, IBlocksDroppableOn
    {
        public string Name;

        public BlockData BlockData;
        public int IndexInChipset = -1;

        public bool HasOutput => BlockData.HasOutput;
        public List<string> CurrentTypeArguments;

        public BlockTop(string name,IHasDrawLayer parent, BlockData data) : base(parent, "ChipFull") 
        {
            CurrentTypeArguments = new List<string>() { "" };

            Name = name;
            _actualSize = base.GetBaseSize();

            BlockData = data;
            ColorMask = BlockData.ChipDataType.GetColor();
            OnMouseDraggedOn += _onDragHandler;

            _inputSections = new List<BlockInputSection>();

            var title = _addTextItem(BlockData.Name, 7, 6, CoordinateMode.ParentPixelOffset, false);
            title.Color = Color.White;

            _createInputSections();
            if (_inputSections.Count > 0)
            {
                _inputSections[_inputSections.Count - 1].SpriteName = "ChipFullEnd";
            }
        }

        #region liftChips
        public Action<BlockTop, UserInput> ChipLiftedCallback;

        private void _onDragHandler(UserInput input)
        {
            if (!_isMouseOverInternalSections())
            {
                ChipLiftedCallback(this, input);
            }
        }
        #endregion

        #region dropChips
        public Action<List<BlockTop>, int> AppendChips;

        public virtual void DropBlocksOn(List<BlockTop> chips, UserInput input)
        {
            if (IsMouseOverBottomSection())
            {
                AppendChips(chips, IndexInChipset + 1);
            }
            else
            {
                AppendChips(chips, IndexInChipset);
            }
        }
        #endregion

        #region inputsections
        protected List<BlockInputSection> _inputSections;
        public List<BlockInputOption> GetCurrentInputs() => _inputSections.Select(section => section.CurrentlySelected).ToList();
        public void ManuallySetInputSection(BlockInputOption item,int index) =>_inputSections[index].ManuallySetDropdown(item);
        
        public void SetInputConnectionsFromAbove(List<BlockTop> chipsAbove)
        {
            _inputSections.ForEach(m => m.SetConnectionsFromAbove(chipsAbove));

            foreach (var subChipset in GetSubChipsets())
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
                section.DropdownSelectedCallback = _inputSectionDropdownChanged;
                _inputSections.Add(section);
            }

            AddChildren(_inputSections);
            _setInputSectionPositions();
        }
        protected void _setInputSectionPositions()
        {
            var height = base.GetBaseSize().Y - 1;

            foreach (var section in _inputSections)
            {
                section.SetLocationConfig(0, height, CoordinateMode.ParentPixelOffset);
                height += section.GetBaseSize().Y;
            }

            _actualSize.Y = height;
        }

        protected virtual void _inputSectionDropdownChanged(BlockInputSection section, BlockInputDropdown dropdown, BlockInputOption optionSelected)
        {
            if (optionSelected.OptionType == InputOptionType.Reference)
            {
                var referenceOption = (BlockInputOptionReference)optionSelected;

                var inputOptions = section.InputBaseTypes;
                var dataTypeFeedingIn = referenceOption.ChipReference.OutputTypeCurrent;

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
        #endregion

        #region dimensions
        public bool IsMouseOverAnySection() => MouseHovering | _isMouseOverInternalSections();
        public virtual bool IsMouseOverBottomSection() 
        {
            if(_inputSections.Count == 0)
            {
                return true;
            }

            return _inputSections.Last().MouseHovering;
        } 
        protected virtual bool _isMouseOverInternalSections() => _inputSections.Select(s => s.MouseHovering).Any(h => h);

        public override IntPoint GetBaseSize() => _actualSize;
        protected IntPoint _actualSize;
        #endregion

        #region refresh
        public Action TopLevelRefreshAll { get { return _topLevelRefreshAll; } set { _setTopLevelRefreshAll(value); } }
        protected virtual void _setTopLevelRefreshAll(Action topLevelRefreshAll) => _topLevelRefreshAll = topLevelRefreshAll;
        private Action _topLevelRefreshAll;

        protected void _topLevelRefreshAll_Delayed() => _delayedTopLevelRefreshAll = true;
        private bool _delayedTopLevelRefreshAll = false;

        public Action ChipsetRefreshText;

        public virtual void RefreshAll() { }
        public void RefreshText()
        {
            _inputSections.ForEach(s => s.RefreshText());
            GetSubChipsets().ForEach(s => s.RefreshText());
        }

        public override void Update(UserInput input)
        {
            base.Update(input);

            if(_delayedTopLevelRefreshAll)
            {
                TopLevelRefreshAll();
                _delayedTopLevelRefreshAll = false;
            }
        }
        #endregion

        public IBlocksetGenerator _generator;
        public void SetGenerator(IBlocksetGenerator generator) => _generator = generator;

        public List<string> GetSelectedInputTypes()
        {
            var output = new List<string>();

            foreach (var section in _inputSections)
            {
                var baseType = section.CurrentlySelected.BaseType;
                output.Add(baseType);
            }

            return output;

        }
        public virtual void GenerateSubChipsets() { }
        public virtual List<Blockset> GetSubChipsets() => new List<Blockset>();

        public static BlockTop GenerateChipFromChipData(BlockData data,string name = "")
        {
            var initialDrawLayer = ManualDrawLayer.Zero;

            if (data.Name.Equals("If"))
            {
                return new BlockTopSwitch(name,initialDrawLayer, data, new List<string>() { "Yes", "No" });
            }
            if (data.Name.Equals("KeySwitch"))
            {
                return new BlockTopSwitch(name,initialDrawLayer, data, new List<string>() {  });
            }
            else if(data.HasOutput)
            {
                return new BlockTopWithOutput(name,initialDrawLayer, data);
            }
            else
            {
                return new BlockTop(name,initialDrawLayer, data);
            }
        }

        public override string ToString() => Name;
    }
}
