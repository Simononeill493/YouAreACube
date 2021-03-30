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
        protected List<ChipInputSection> _inputSections;

        public ChipTop(IHasDrawLayer parent, ChipData data) : base(parent, "ChipFull") 
        { 
            Chip = data;
            ColorMask = Chip.ChipColor;
            OnMouseDraggedOn += (input) => ChipLiftedCallback(this, input);

            _sections = new List<MenuItem>();
            _inputSections = new List<ChipInputSection>();

            var title = new TextMenuItem(this, Chip.Name);
            title.Color = Color.White;
            title.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, true);
            AddChild(title);
        }

        public Action<ChipTop, UserInput> ChipLiftedCallback;
        public Action<List<ChipTop>,int> AppendChips;

        public virtual void DropChipsOn(List<ChipTop> chips, UserInput input) 
        {
            if (IsMouseOverBottomSection())
            {
                AppendChips(chips,IndexInChipset+1);
            }
            else
            {
                AppendChips(chips,IndexInChipset);
            }
        }

        public bool IsMouseOverAnySection()
        {
            if (MouseHovering)
            {
                return true;
            }

            foreach (var section in _sections)
            {
                if (section.MouseHovering)
                {
                    return true;
                }
            }

            return false;
        }
        public bool IsMouseOverBottomSection()
        {
            if (_sections.Count == 0)
            {
                return true;
            }

            return _sections.Last().MouseHovering;
        }

        public void SetConnectionsFromAbove(List<ChipTop> chipsAbove) => _inputSections.ForEach(m => m.SetConnectionsFromAbove(chipsAbove));

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
    }
}
