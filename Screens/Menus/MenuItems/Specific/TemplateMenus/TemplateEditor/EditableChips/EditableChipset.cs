using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class EditableChipset : DraggableMenuItem
    {
        public List<ChipPreviewLarge> Chips;
        private ChipPreviewLarge topChip;

        public EditableChipset(IHasDrawLayer parent) : base(parent,"BlueChipFull") 
        {
            Chips = new List<ChipPreviewLarge>();
        }

        public void AddChip(ChipData chipData,float scaleMultiplier)
        {
            var chip = new ChipPreviewLarge(this, chipData);
            chip.MultiplyScaleCascade(scaleMultiplier);
            chip.SetLocationConfig(0, GetFullBaseSize().Y-(Chips.Count()+1), CoordinateMode.ParentPixelOffset, false);
            chip.UpdateDimensions(ActualLocation, GetCurrentSize());

            Chips.Add(chip);
            AddChild(chip);

            _resetTopChip();
        }

        public Point GetFullBaseSize()
        {
            var size = GetBaseSize();

            foreach (var chip in Chips)
            {
                var chipSize = chip.GetFullBaseSize();
                size.Y += chipSize.Y;
            }

            return size;
        }
        public Point GetFullSize() => GetFullBaseSize() * Scale;

        public static EditableChipset CreateAtMouse(UserInput input,ChipEditPane container)
        {
            var chipset = new EditableChipset(container);

            chipset.MultiplyScaleCascade(container.ChipScaleMultiplier);
            chipset.UpdateDimensions(container.ActualLocation, container.GetCurrentSize());
            chipset.OnEndDrag += (i) => container.ChipsetReleased(chipset, i);
            chipset.TryStartDrag(input, chipset.GetCurrentSize() / 2);

            return chipset;
        }





        private void _resetTopChip()
        {
            if (topChip != null)
            {
                SetNotDraggableFrom(topChip);
            }
            topChip = Chips.First();
            SetDraggableFrom(topChip);
        }

    }
}
