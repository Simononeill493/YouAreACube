using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public abstract class ChipTop : SpriteMenuItem
    {
        public ChipData Chip;
        public int CurrentPositionInChipset = -1;

        public Action<UserInput, int> LiftChipFromChipset;
        public System.Action OutputTextChangedCallback;

        public abstract bool IsMouseOverAnySection();
        public abstract bool IsMouseOverBottomSection();

        public abstract void SetConnectionsFromAbove(List<ChipTop> chipsAbove);
        public abstract void RefreshText();

        protected List<MenuItem> _sections;

        public ChipTop(IHasDrawLayer parent, ChipData data) : base(parent, "BlueChipFull") 
        { 
            Chip = data;

            OnMouseDraggedOn += _onDraggedHandler;

            var title = new TextMenuItem(this, Chip.Name);
            title.Color = Color.White;
            title.SetLocationConfig(50, 50, CoordinateMode.ParentPercentageOffset, true);
            AddChild(title);

            _sections = new List<MenuItem>() { this };
        }

        private void _onDraggedHandler(UserInput input)
        {
            if (!MenuScreen.UserDragging)
            {
                LiftChipFromChipset(input, CurrentPositionInChipset);
            }
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

        public virtual Point GetFullBaseSize()
        {
            var baseSize = GetBaseSize();
            return new Point(baseSize.X, _sections.Sum(s => s.GetBaseSize().Y)-(_sections.Count));
        }
        public Point GetFullSize() => GetFullBaseSize() * Scale;


    }
}
