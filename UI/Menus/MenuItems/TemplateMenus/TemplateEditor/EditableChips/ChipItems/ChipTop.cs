using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    public abstract class ChipTop : SpriteMenuItem, IChipsDroppableOn
    {
        public ChipData Chip;
        public int IndexInChipset = -1;

        public Action RefreshAllCallback;
        public Action RefreshTextCallback;
        public Action<ChipTop, UserInput> ChipLiftedCallback;
        public Action<List<ChipTop>, int> AppendChips;

        public ChipTop(IHasDrawLayer parent, ChipData data) : base(parent, "ChipFull") 
        {
            _actualSize = base.GetBaseSize();

            Chip = data;
            ColorMask = Chip.ChipColor;
            OnMouseDraggedOn += _onDragHandler;

            _inputSections = new List<ChipInputSection>();

            var title = new TextMenuItem(this, Chip.Name);
            title.Color = Color.White;
            title.SetLocationConfig(7, 6, CoordinateMode.ParentPixelOffset, false);
            AddChild(title);
        }

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

        private void _onDragHandler(UserInput input)
        {
            if (!_isMouseOverInternalSections)
            {
                ChipLiftedCallback(this, input);
            }
        }

        #region inputsections
        protected List<ChipInputSection> _inputSections;
        public void SetInputConnectionsFromAbove(List<ChipTop> chipsAbove) => _inputSections.ForEach(m => m.SetConnectionsFromAbove(chipsAbove));

        protected void _createInputSections()
        {
            for (int i = 0; i < Chip.NumInputs; i++)
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
                Chip.SetOutputTypeFromGeneric(genericOption.BaseOutput);
            }
            else if (optionSelected.OptionType == InputOptionType.Base)
            {
                Chip.ResetOutputType();
            }
        }
        #endregion

        #region dimensions
        public bool IsMouseOverAnySection() => MouseHovering | _isMouseOverInternalSections;
        public virtual bool IsMouseOverBottomSection() 
        {
            if(_inputSections.Count == 0)
            {
                return true;
            }

            return _inputSections.Last().MouseHovering;
        } 
        protected virtual bool _isMouseOverInternalSections => _inputSections.Select(s => s.MouseHovering).Any(h => h);

        public override Point GetBaseSize() => _actualSize;
        protected Point _actualSize;
        #endregion

        public void RefreshText() => _inputSections.ForEach(s => s.RefreshText());
        public virtual void GenerateSubChipsets(IChipsetGenerator generator) { }
        public virtual List<EditableChipset> GetSubChipsets() => new List<EditableChipset>(); 
    }
}
