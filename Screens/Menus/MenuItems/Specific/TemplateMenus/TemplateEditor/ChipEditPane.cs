using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ChipEditPane : SpriteMenuItem
    {
        public MenuItem Trash;
        public List<ChipPreviewLarge> AllChips;

        public ChipEditPane(IHasDrawLayer parentDrawLayer) : base(parentDrawLayer, "ChipEditPane")
        {
            AllChips = new List<ChipPreviewLarge>();


            var size = GetBaseSize();

            var plusButton = new SpriteMenuItem(this, "PlusButton");
            plusButton.SetLocationConfig(size.X-9, 0, CoordinateMode.ParentPixelOffset, false);
            plusButton.OnClick += (i) => _scaleChipsUp();
            AddChild(plusButton);


            var minusButton = new SpriteMenuItem(this, "MinusButton");
            minusButton.SetLocationConfig(size.X - 17, 0, CoordinateMode.ParentPixelOffset, false);
            minusButton.OnClick += (i) => _scaleChipsDown();

            AddChild(minusButton);
        }

        protected void _scaleChipsUp()
        {
            Console.WriteLine("Scaled chip pane up");
        }

        protected void _scaleChipsDown()
        {
            Console.WriteLine("Scaled chip pane down");
        }


        public void TryCreateChip(ChipPreviewSmall preview, UserInput input)
        {
            if(MenuScreen.UserDragging) { return; }

            var chip = new ChipPreviewLarge(this, preview.Chip);
            if(!chip.TryBeginDrag(input, chip.GetCurrentSize() / 2))
            {
                return;
            }

            chip.SetLocationConfig(input.MousePos, CoordinateMode.Absolute, true);
            chip.OnEndDrag += (i) => ChipReleased(chip, i);

            AllChips.Add(chip);
            AddChildAfterUpdate(chip);
        }

        public void ChipReleased(ChipPreviewLarge chip,UserInput input)
        {
            if(Trash.IsMouseOver(input))
            {
                DeleteChip(chip);
            }
            else
            {
                _attachChipToPane(chip, GetCurrentSize());
            }
        }

        public void DeleteChip(ChipPreviewLarge chip)
        {
            AllChips.Remove(chip);
            RemoveChildAfterUpdate(chip);
        }

        private void _attachChipToPane(ChipPreviewLarge chip,Point planeSize)
        {
            var displacement = this.GetLocationOffset(chip.ActualLocation);
            var chipSize = chip.GetFullSize();
            var bottomRightDisplacement = displacement + chipSize;

            if (displacement.X < 0) { displacement.X = 0; }
            if (displacement.Y < 0) { displacement.Y = 0; }

            if (bottomRightDisplacement.X > planeSize.X) { displacement.X = planeSize.X - chipSize.X; }
            if (bottomRightDisplacement.Y > planeSize.Y) { displacement.Y = planeSize.Y - chipSize.Y; }

            chip.SetLocationConfig((displacement / Scale), CoordinateMode.ParentPixelOffset, false);
            chip.UpdateThisAndChildLocations(ActualLocation, planeSize);
        }
    }
}
