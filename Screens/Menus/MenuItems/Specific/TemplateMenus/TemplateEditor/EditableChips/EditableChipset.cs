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

        private ChipPreviewLarge _topChip;
        private Action<UserInput,List<ChipPreviewLarge>> _createNewChipset;

        public EditableChipset(IHasDrawLayer parent) : base(parent, "TopOfChipset")
        {
            Chips = new List<ChipPreviewLarge>();
        }

        public void SetCreateNewChipsetCallback(Action<UserInput, List<ChipPreviewLarge>> createNewChipset)
        {
            _createNewChipset = createNewChipset;
        }

        public void AppendChips(List<ChipPreviewLarge> toAdd,int index)
        {
            Chips.InsertRange(index, toAdd);
            AddChildrenAfterUpdate(toAdd);

            foreach(var chip in toAdd)
            {
                chip.SetChipLiftCallback(_liftChipsFromChipset);
            }

            _recalculate();
        }

        public List<ChipPreviewLarge> PopChips(int index)
        {
            var toRemove = Chips.Skip(index).ToList();
            Chips.RemoveRange(index, toRemove.Count());
            RemoveChildrenAfterUpdate(toRemove);

            _recalculate();
            return toRemove;
        }

        public void _liftChipsFromChipset(UserInput input,int index)
        {
            var toRemove = PopChips(index);
            _createNewChipset(input,toRemove);
        }

        public int GetChipIndexThatMouseIsOver(UserInput input)
        {
            for(int i=0;i<Chips.Count;i++)
            {
                if(Chips[i].IsMouseOverAnySection())
                {
                    if(Chips[i].IsMouseOverBottomSection())
                    {
                        return i+1;
                    }

                    return i;
                }
            }

            return -1;
        }


        private void _recalculate()
        {
            _updateChipPositions();
            _updateTopChip();
            _updateChildDimensions();
        }

        private void _updateChipPositions()
        {
            var baseSize = GetBaseSize();
            for (int i = 0; i < Chips.Count; i++)
            {
                Chips[i].CurrentPositionInChipset = i;
                Chips[i].SetLocationConfig(0, baseSize.Y - (i + 1), CoordinateMode.ParentPixelOffset, false);
                Chips[i].UpdateDimensions(ActualLocation, GetCurrentSize());

                baseSize.Y += Chips[i].GetFullBaseSize().Y;
            }
        }
        private void _updateTopChip()
        {
            if (_topChip != null)
            {
                SetNotDraggableFrom(_topChip);
            }
            _topChip = Chips.First();
            SetDraggableFrom(_topChip);
        }

        public Point GetFullBaseSize()
        {
            var size = GetBaseSize();
            foreach (var chip in Chips)
            {
                var chipSize = chip.GetFullBaseSize();
                size.Y += chipSize.Y;
                if(chipSize.X > size.X)
                {
                    size.X = chipSize.X;
                }
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


        public static EditableChipset CreateAtMouse(UserInput input, ChipEditPane container)
        {
            var chipset = new EditableChipset(container);

            chipset.MultiplyScaleCascade(container.ChipScaleMultiplier);
            chipset.UpdateDimensions(container.ActualLocation, container.GetCurrentSize());
            chipset.OnEndDrag += (i) => container.ChipsetReleased(chipset, i);

            return chipset;
        }
    }
}
