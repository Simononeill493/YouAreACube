using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ChipTopSection : SpriteMenuItem
    {
        public ChipData Chip;
        public int CurrentPositionInChipset = -1;
        public string OutputName => _outputLabel.TextBox.Text;

        private ChipItemOutputLabel _outputLabel;
        private List<ChipMiddleSection> _middleSections = new List<ChipMiddleSection>();

        public ChipTopSection(IHasDrawLayer parent, ChipData data) : base(parent, "BlueChipFull")
        {
            Chip = data;
            OnMouseDraggedOn += _onDraggedHandler;

            var title = new TextMenuItem(this, Chip.Name);
            title.Color = Color.White;
            title.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, true);
            AddChild(title);

            _tryCreateOutputLabel();
            _createMiddleSections(Chip,GetBaseSize());
        }
        private void _tryCreateOutputLabel()
        {
            if (Chip.OutputType != null)
            {
                _outputLabel = new ChipItemOutputLabel(this, Chip);
                _outputLabel.SetLocationConfig(100, 0, CoordinateMode.ParentPercentageOffset);
                _outputLabel.TextBox.OnTextChanged += OutputNameChanged;
                AddChild(_outputLabel);
            }
        }
        private void _createMiddleSections(ChipData chip,Point size)
        {
            for(int i=1;i<chip.NumInputs+1;i++)
            {
                var section = ChipSectionFactory.Create(this,i);
                section.SetLocationConfig(ActualLocation.X, ActualLocation.Y + (size.Y*i) - 1, CoordinateMode.ParentPixelOffset, false);
                section.DropdownSelectedCallback = _middleSectionDropdownChanged;

                _middleSections.Add(section);
                AddChild(section);
            }

            _updateChildDimensions();
        }

        public void SetConnectionsFromAbove(List<ChipTopSection> chipsAbove) =>_middleSections.ForEach(m => m.SetConnectionsFromAbove(chipsAbove));
        public void _middleSectionDropdownChanged(ChipInputDropdown dropdown,ChipInputOption optionSelected)
        {
            if (optionSelected.OptionType == InputOptionType.Generic)
            {
                var genericOption = (ChipInputOptionGeneric)optionSelected;
                Chip.SetOutputTypeFromGeneric(genericOption.BaseOutput);
            }
            else if(optionSelected.OptionType == InputOptionType.Base)
            {
                Chip.ResetOutputType();
            }

            _outputLabel?.SetOutputDataTypeLabel(Chip.OutputTypeCurrent);
        }

        public Point GetFullBaseSize()
        {
            var baseSize = GetBaseSize();

            if(_outputLabel!=null)
            {
                baseSize.X += _outputLabel.GetBaseSize().X;
            }

            return new Point(baseSize.X, baseSize.Y + (baseSize.Y * Chip.NumInputs));
        }
        public Point GetFullSize() => GetFullBaseSize() * Scale;

        #region visual
        protected override void _drawSelf(DrawingInterface drawingInterface)
        {
            base._drawSelf(drawingInterface);

            if (MenuScreen.UserDragging & IsMouseOverAnySection())
            {
                _highlightInsertionPoint(drawingInterface);
            }
        }
        private void _highlightInsertionPoint(DrawingInterface drawingInterface)
        {
            var size = GetFullSize();
            var height = ActualLocation.Y;

            if (IsMouseOverBottomSection())
            {
                height += (size.Y - (1 * Scale));
            }

            drawingInterface.DrawRectangle(ActualLocation.X, height, size.X, 5 * Scale, DrawLayers.MenuHoverLayer, Color.Red, false);
        }

        public void RefreshText() => _middleSections.ForEach(m => m.RefreshText());

        private void OutputNameChanged(string newName) => OutputTextChangedCallback.Invoke();
        public System.Action OutputTextChangedCallback;
        #endregion

        #region mouseControls
        public Action<UserInput, int> LiftChipFromChipset;

        private void _onDraggedHandler(UserInput input)
        {
            if (!MenuScreen.UserDragging)
            {
                LiftChipFromChipset(input, CurrentPositionInChipset);
            }
        }

        public bool IsMouseOverAnySection()
        {
            if (MouseHovering)
            {
                return true;
            }

            foreach (var section in _middleSections)
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
            if (_middleSections.Count == 0)
            {
                return true;
            }

            return _middleSections.Last().MouseHovering;
        }
        #endregion
    }
}
