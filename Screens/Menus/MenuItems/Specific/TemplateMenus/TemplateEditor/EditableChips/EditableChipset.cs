using Microsoft.Xna.Framework;
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

        public EditableChipset(IHasDrawLayer parent) : base(parent, "BlueChipFull")
        {
            Chips = new List<ChipPreviewLarge>();
        }

        public void AppendChipset(EditableChipset toAdd, int index)
        {
            Chips.InsertRange(index, toAdd.Chips);
            AddChildrenAfterUpdate(toAdd.Chips);

            _updateChipPositions();
            _resetTopChip();

            _updateChildDimensions();
        }

        private void _updateChipPositions()
        {
            var baseSize = GetBaseSize();
            for (int i = 0; i < Chips.Count;i++)
            {
                Chips[i].SetLocationConfig(0, baseSize.Y - (i+1), CoordinateMode.ParentPixelOffset, false);
                Chips[i].UpdateDimensions(ActualLocation, GetCurrentSize());

                baseSize.Y += Chips[i].GetFullBaseSize().Y;
            }
        }

        public void AddInitialChip(ChipData chipData, float scaleMultiplier)
        {
            var chip = new ChipPreviewLarge(this, chipData);

            chip.MultiplyScaleCascade(scaleMultiplier);
            chip.SetLocationConfig(0, GetBaseSize().Y-1, CoordinateMode.ParentPixelOffset, false);
            chip.UpdateDimensions(ActualLocation, GetCurrentSize());

            Chips.Add(chip);
            AddChild(chip);

            _resetTopChip();
        }

        public int GetHoveredChip(UserInput input)
        {
            for(int i=0;i<Chips.Count;i++)
            {
                if(Chips[i].IsMouseOverAnySection())
                {
                    return i;
                }
            }

            return -1;
        }


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
        public Rectangle GetFullRect()
        {
            var chipLoc = ActualLocation;
            var fullSize = GetFullSize();

            return new Rectangle(chipLoc.X, chipLoc.Y, fullSize.X, fullSize.Y);
        }
    }
}
