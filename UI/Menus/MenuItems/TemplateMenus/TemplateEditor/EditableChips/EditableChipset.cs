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
        public List<ChipPreviewLarge> GetAttachedChips() => _chips;
        private List<ChipPreviewLarge> _chips;

        private ChipPreviewLarge _topChip;

        private Action<List<ChipPreviewLarge>,UserInput> _createNewChipsetInEditPane;
        public void SetCreateNewChipsetCallback(Action<List<ChipPreviewLarge>,UserInput> createNewChipset) => _createNewChipsetInEditPane = createNewChipset;

        public EditableChipset(IHasDrawLayer parent,float scaleMultiplier) : base(parent, "TopOfChipset")
        {
            _chips = new List<ChipPreviewLarge>();
            MultiplyScaleCascade(scaleMultiplier);
        }

        public void AppendChips(List<ChipPreviewLarge> toAdd,int index)
        {
            _chips.InsertRange(index, toAdd);
            AddChildren(toAdd);
            toAdd.ForEach(chip => chip.SetChipLiftCallback(_liftChipsFromChipset));

            _recalculate();
        }

        public List<ChipPreviewLarge> PopChips(int index)
        {
            var toRemove = _chips.Skip(index).ToList();
            _chips.RemoveRange(index, toRemove.Count());
            RemoveChildrenAfterUpdate(toRemove);

            _recalculate();
            return toRemove;
        }

        public void _liftChipsFromChipset(UserInput input,int index)
        {
            var toRemove = PopChips(index);
            _createNewChipsetInEditPane(toRemove,input);
        }

        public int GetChipIndexThatMouseIsOver(UserInput input)
        {
            for(int i=0;i<_chips.Count;i++)
            {
                if(_chips[i].IsMouseOverAnySection())
                {
                    if(_chips[i].IsMouseOverBottomSection())
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
            for (int i = 0; i < _chips.Count; i++)
            {
                _chips[i].CurrentPositionInChipset = i;
                _chips[i].SetLocationConfig(0, baseSize.Y - (i + 1), CoordinateMode.ParentPixelOffset, false);
                _chips[i].UpdateDimensions(ActualLocation, GetCurrentSize());

                baseSize.Y += _chips[i].GetFullBaseSize().Y;
            }
        }
        private void _updateTopChip()
        {
            if (_topChip != null)
            {
                SetNotDraggableFrom(_topChip);
            }
            _topChip = _chips.First();
            SetDraggableFrom(_topChip);
        }

        public Point GetFullBaseSize()
        {
            var size = GetBaseSize();
            foreach (var chip in _chips)
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

        public Point DefaultMouseDragOffset => GetAttachedChips().First().GetCurrentSize() / 2;
    }
}
