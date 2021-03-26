using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    public abstract class ChipTop : SpriteMenuItem
    {
        public ChipData Chip;
        public int CurrentPositionInChipset = -1;

        public System.Action ChipsetRefreshTextCallback;
        public Action<UserInput, int> LiftChipFromChipset;

        protected List<MenuItem> _sections;
        protected List<ChipMiddleSection> _inputSections;

        public ChipTop(IHasDrawLayer parent, ChipData data) : base(parent, "BlueChipFull") 
        { 
            Chip = data;

            OnMouseDraggedOn += _chipDraggedHandler;

            var title = new TextMenuItem(this, Chip.Name);
            title.Color = Color.White;
            title.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, true);
            AddChild(title);

            _sections = new List<MenuItem>() { this };
            _inputSections = new List<ChipMiddleSection>();
        }

        private void _chipDraggedHandler(UserInput input)
        {
            if (!MenuScreen.UserDragging)
            {
                LiftChipFromChipset(input, CurrentPositionInChipset);
            }
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

        protected void _createInputSections(ChipData chip)
        {
            for (int i = 1; i < chip.NumInputs + 1; i++)
            {
                var section = ChipSectionFactory.Create(this, i);
                section.DropdownSelectedCallback = _inputSectionDropdownChanged;

                _addSection(section);
                _inputSections.Add(section);
            }

            _updateChildDimensions();
        }
        protected void _addSection(MenuItem section)
        {
            var chipFullSize = GetFullBaseSize();
            section.SetLocationConfig(0, chipFullSize.Y - 1, CoordinateMode.ParentPixelOffset, false);

            _sections.Add(section);
            AddChild(section);
        }

        protected override void _drawSelf(DrawingInterface drawingInterface)
        {
            base._drawSelf(drawingInterface);

            if (MenuScreen.UserDragging & IsMouseOverAnySection())
            {
                _highlightInsertionPoint(drawingInterface);
            }
        }
        protected void _highlightInsertionPoint(DrawingInterface drawingInterface)
        {
            var size = GetFullSize();
            var height = ActualLocation.Y;

            if (IsMouseOverBottomSection())
            {
                height += (size.Y - (1 * Scale));
            }

            drawingInterface.DrawRectangle(ActualLocation.X, height, size.X, 5 * Scale, DrawLayers.MenuHoverLayer, Color.Red, false);
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

        public virtual Point GetFullBaseSize() => new Point(GetBaseSize().X, _sections.Sum(s => s.GetBaseSize().Y) - (_sections.Count));
        public Point GetFullSize() => GetFullBaseSize() * Scale;

        public void RefreshText() => _inputSections.ForEach(m => m.RefreshText());
        public void SetConnectionsFromAbove(List<ChipTop> chipsAbove) => _inputSections.ForEach(m => m.SetConnectionsFromAbove(chipsAbove));
    }
}
