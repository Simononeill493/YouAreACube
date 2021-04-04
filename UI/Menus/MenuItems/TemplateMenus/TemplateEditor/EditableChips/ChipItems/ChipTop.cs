using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    public abstract class ChipTop : SpriteMenuItem, IChipsDroppableOn
    {
        public ChipData ChipData;
        public int IndexInChipset = -1;

        public ChipTop(IHasDrawLayer parent, ChipData data) : base(parent, "ChipFull") 
        {
            _actualSize = base.GetBaseSize();

            ChipData = data;
            ColorMask = ChipData.ChipColor;
            OnMouseDraggedOn += _onDragHandler;

            _inputSections = new List<ChipInputSection>();

            var title = new TextMenuItem(this, ChipData.Name);
            title.Color = Color.White;
            title.SetLocationConfig(7, 6, CoordinateMode.ParentPixelOffset, false);
            AddChild(title);
        }

        #region liftChips
        public Action<ChipTop, UserInput> ChipLiftedCallback;

        private void _onDragHandler(UserInput input)
        {
            if (!_isMouseOverInternalSections())
            {
                ChipLiftedCallback(this, input);
            }
        }
        #endregion

        #region dropChips
        public Action<List<ChipTop>, int> AppendChips;

        public virtual void DropChipsOn(List<ChipTop> chips, UserInput input)
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
        protected List<ChipInputSection> _inputSections;
        public void SetInputConnectionsFromAbove(List<ChipTop> chipsAbove)
        {
            _inputSections.ForEach(m => m.SetConnectionsFromAbove(chipsAbove));

            foreach (var subChipset in GetSubChipsets())
            {
                var connectionsList = new List<ChipTop>();
                connectionsList.AddRange(chipsAbove);
                subChipset.SetInputConnectionsFromAbove(connectionsList);
            }
        }

        protected void _createInputSections()
        {
            for (int i = 0; i < ChipData.NumInputs; i++)
            {
                var section = ChipSectionFactory.Create(this, i);
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

        protected virtual void _inputSectionDropdownChanged(ChipInputDropdown dropdown, ChipInputOption optionSelected)
        {
            if (optionSelected.OptionType == InputOptionType.Generic)
            {
                var genericOption = (ChipInputOptionGeneric)optionSelected;
                ChipData.SetOutputTypeFromGeneric(genericOption.BaseOutput);
            }
            else if (optionSelected.OptionType == InputOptionType.Base)
            {
                ChipData.ResetOutputType();
            }

            _topLevelRefreshAll_Delayed();
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

        public override Point GetBaseSize() => _actualSize;
        protected Point _actualSize;
        #endregion

        #region refresh
        public Action TopLevelRefreshAll { get { return _topLevelRefreshAll; } set { _setTopLevelRefreshAll(value); } }
        protected virtual void _setTopLevelRefreshAll(Action topLevelRefreshAll) => _topLevelRefreshAll = topLevelRefreshAll;
        private Action _topLevelRefreshAll;

        private void _topLevelRefreshAll_Delayed() => _delayedTopLevelRefreshAll = true;
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

        public IChipsetGenerator _generator;
        public void SetGenerator(IChipsetGenerator generator) => _generator = generator;

        public virtual void GenerateSubChipsets() { }
        public virtual List<EditableChipset> GetSubChipsets() => new List<EditableChipset>();

        public static ChipTop GenerateChipFromChipData(ChipData data)
        {
            var initialDrawLayer = ManualDrawLayer.Zero;

            if (data.Name.Equals("If"))
            {
                return new ChipTopSwitch(initialDrawLayer, data, new List<string>() { "Yes", "No" });
            }
            else
            {
                return new ChipTopStandard(initialDrawLayer, data);
            }
        }
    }
}
