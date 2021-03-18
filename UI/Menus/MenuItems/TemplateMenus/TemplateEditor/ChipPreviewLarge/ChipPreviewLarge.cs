using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ChipPreviewLarge : SpriteMenuItem
    {
        public ChipData Chip;
        public int CurrentPositionInChipset = -1;

        private TextMenuItem _title;
        private List<ChipPreviewLargeMiddleSection> _middleSections = new List<ChipPreviewLargeMiddleSection>();

        private ChipPreviewOutputLabel _outputLabel;

        public Action<UserInput, int> LiftChipFromChipset;

        public ChipPreviewLarge(IHasDrawLayer parent, ChipData data) : base(parent, "BlueChipFull")
        {
            Chip = data;
            _title = new TextMenuItem(this);
            _title.Color = Color.White;
            _title.Text = Chip.Name;

            _title.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, true);
            AddChild(_title);

            if(Chip.Output!=null)
            {
                _outputLabel = new ChipPreviewOutputLabel(this, Chip);
                _outputLabel.SetLocationConfig(100, 0, CoordinateMode.ParentPercentageOffset);
                AddChild(_outputLabel);
            }

            _addSections(Chip,GetBaseSize());

            OnMouseDraggedOn += _OnDraggedHandler;
        }

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

        private void _addSections(ChipData chip,Point size)
        {
            for(int i=1;i<chip.NumInputs+1;i++)
            {
                var parentDrawLayer = ManualDrawLayer.Create(DrawLayer - (DrawLayers.MinLayerDistance * i));
                var section = ChipSectionFactory.Create(parentDrawLayer,chip.GetInputType(i));

                if (i == chip.NumInputs) { section.SpriteName = "BlueChipFullEnd"; }

                section.SetLocationConfig(ActualLocation.X, ActualLocation.Y + (size.Y*i) - 1, CoordinateMode.ParentPixelOffset, false);

                _middleSections.Add(section);
                AddChild(section);
            }

            _updateChildDimensions();
        }


        private void _OnDraggedHandler(UserInput input)
        {
            if (!MenuScreen.UserDragging)
            {
                LiftChipFromChipset(input,CurrentPositionInChipset);
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
            if(_middleSections.Count==0)
            {
                return true;
            }

            return _middleSections.Last().MouseHovering;
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
    }
}
