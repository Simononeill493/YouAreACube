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

        protected List<MenuItem> _sections;

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

            _sections = new List<MenuItem>();
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

        protected void _addSection(MenuItem section) => _addSections(new List<MenuItem>() { section });
        protected void _addSections<T>(List<T> sections) where T : MenuItem
        {
            _sections.AddRange(sections);
            AddChildren(sections);

            _setSectionPositions();
        }

        protected void _removeSection(MenuItem section) => _removeSections(new List<MenuItem>() { section });
        protected void _removeSections<T>(List<T> sections) where T : MenuItem
        {
            sections.ForEach(item => _sections.Remove(item));
            RemoveChildren(sections);

            _setSectionPositions();
        }

        protected void _addChildAsSection(MenuItem section) => _addChildrenAsSections(new List<MenuItem>() { section });
        protected void _addChildrenAsSections<T>(List<T> sections) where T : MenuItem
        {
            _sections.AddRange(sections);
            sections.ForEach(s => s.Visible = true);
            _setSectionPositions();
        }

        protected void _removeChildAsSection(MenuItem section) => _removeChildrenAsSections(new List<MenuItem>() { section });
        protected void _removeChildrenAsSections<T>(List<T> sections) where T : MenuItem
        {
            sections.ForEach(item => _sections.Remove(item));
            sections.ForEach(s => s.Visible = false);
            _setSectionPositions();
        }

        protected void _setSectionPositions()
        {
            var height = base.GetBaseSize().Y - 1;

            foreach (var section in _sections)
            {
                section.SetLocationConfig(0, height, CoordinateMode.ParentPixelOffset);
                height += section.GetBaseSize().Y;
            }

            _actualSize.Y = height;
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

            _addSections(_inputSections);
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
        public bool IsMouseOverBottomSection() 
        {
            if(_sections.Count == 0)
            {
                return true;
            }

            return _sections.Last().MouseHovering;
        } 
        private bool _isMouseOverInternalSections => _sections.Select(s => s.MouseHovering).Any(h => h);

        public override Point GetBaseSize() => _actualSize;
        private Point _actualSize;
        #endregion

        public void RefreshText() => _inputSections.ForEach(s => s.RefreshText());
        public virtual void GenerateSubChipsets(Func<EditableChipset> generator) { }
        public virtual List<EditableChipset> GetSubChipsets() { return new List<EditableChipset>(); }
    }
}
