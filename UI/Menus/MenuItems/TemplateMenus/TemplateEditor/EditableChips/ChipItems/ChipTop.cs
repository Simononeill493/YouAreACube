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

        public Action ChipsetRefreshAllCallback;
        public Action ChipsetRefreshTextCallback;
        public Action<UserInput, int> LiftChipFromChipset;

        private List<MenuItem> _sections;
        protected List<ChipMiddleSection> _inputSections;

        public ChipTop(IHasDrawLayer parent, ChipData data) : base(parent, "ChipFull") 
        { 
            Chip = data;
            ColorMask = Chip.ChipType.GetColor();

            OnMouseDraggedOn += _chipDraggedHandler;

            var title = new TextMenuItem(this, Chip.Name);
            title.Color = Color.White;
            title.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, true);
            AddChild(title);

            _sections = new List<MenuItem>() { this };
            _inputSections = new List<ChipMiddleSection>();
        }

        public virtual (EditableChipset chipset, int index, bool bottom) GetSubChipThatMouseIsOverIfAny(UserInput input) => (null, -1, false);

        private void _chipDraggedHandler(UserInput input)
        {
            if (!MenuScreen.IsUserDragging)
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
            _addAsSection(section);
            AddChild(section);
        }
        protected void _addSectionAfterUpdate(MenuItem section)
        {
            _addAsSection(section);
            AddChildAfterUpdate(section);
        }

        protected void _removeSection(MenuItem section)
        {
            _sections.Remove(section);
            RemoveChild(section);
        }
        protected void _removeSectionAfterUpdate(MenuItem section)
        {
            _sections.Remove(section);
            RemoveChildAfterUpdate(section);
        }

        private void _addAsSection(MenuItem section)
        {
            var chipFullSize = GetFullBaseSize();
            section.SetLocationConfig(0, chipFullSize.Y - 1, CoordinateMode.ParentPixelOffset, false);

            section.ScaleMultiplier = this.ScaleMultiplier;
            section.UpdateDimensionsCascade(ActualLocation,chipFullSize);

            _sections.Add(section);
        }

        public override void Update(UserInput input)
        {
            base.Update(input);
            
            if(input.IsKeyJustPressed(Microsoft.Xna.Framework.Input.Keys.R))
            {
                _refreshSectionPositions();
                UpdateDimensionsCascade(ActualLocation, GetBaseSize());
            }
        }
        private void _refreshSectionPositions()
        {
            var height = GetBaseSize().Y;
            for (int i = 1; i < _sections.Count; i++)
            {
                _sections[i].SetLocationConfig(0, height - 1, CoordinateMode.ParentPixelOffset);
                height += _sections[i].GetBaseSize().Y;
            }
        }

        protected override void _drawSelf(DrawingInterface drawingInterface)
        {
            base._drawSelf(drawingInterface);

            if (IsMouseOverAnySection() & (MenuScreen.DraggedItem!=null))
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

        public virtual Point GetFullBaseSize()
        {
            var fullBaseSize = new Point(GetBaseSize().X, 0);

            foreach(var s in _sections)
            {
                var asChipset = (s as EditableChipset);
                if(asChipset!=null)
                {
                    fullBaseSize.Y += asChipset.GetFullBaseSize().Y;
                }
                else
                {
                    fullBaseSize.Y += s.GetBaseSize().Y;
                }
            }

            fullBaseSize.Y -= _sections.Count;
            return fullBaseSize;
        }
        public Point GetFullSize() => GetFullBaseSize() * Scale;

        public void RefreshText() => _inputSections.ForEach(m => m.RefreshText());
        public void SetConnectionsFromAbove(List<ChipTop> chipsAbove) => _inputSections.ForEach(m => m.SetConnectionsFromAbove(chipsAbove));
    }
}
