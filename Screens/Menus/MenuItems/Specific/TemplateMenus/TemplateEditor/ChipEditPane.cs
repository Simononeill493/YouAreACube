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
        }

        public void CreateChip(ChipPreviewSmall preview, UserInput input)
        {
            var chip = new ChipPreviewLarge(this, preview.Chip);
            chip.ManuallyAttachToMouse(chip.GetSize() / 2);
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
                _pushInside(chip, GetSize());
            }
        }

        public void DeleteChip(ChipPreviewLarge chip)
        {
            AllChips.Remove(chip);
            RemoveChildAfterUpdate(chip);
        }

        private void _pushInside(ChipPreviewLarge chip,Point planeSize)
        {
            var chipTopLeft = chip.ActualLocation;
            var chipBottomRight = chipTopLeft + chip.GetSize();
            var planeTopLeft = ActualLocation;
            var planeBottomRight = planeTopLeft + planeSize;

            var topLeftTooFar = planeTopLeft - chipTopLeft;
            var bottomRightTooFar = planeBottomRight - chipBottomRight;

            int leftToofar = topLeftTooFar.X;
            int topTooFar = topLeftTooFar.Y;
            int rightTooFar = bottomRightTooFar.X;
            int bottomTooFar = bottomRightTooFar.Y;

            if (leftToofar < 0) { leftToofar = 0; }
            if (topTooFar < 0) { topTooFar = 0; }
            if (bottomTooFar > 0) { bottomTooFar = 0; }
            if (rightTooFar > 0) { rightTooFar = 0; }

            var xChange = leftToofar + rightTooFar;
            var yChange = topTooFar + bottomTooFar;

            chip.SetLocationConfig(chipTopLeft.X + xChange, chipTopLeft.Y + yChange, CoordinateMode.Absolute, false);
            chip.UpdateThisAndChildLocations(ActualLocation, planeSize);
        }
    }
}
