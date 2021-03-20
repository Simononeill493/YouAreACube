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
        public List<ChipPreviewLarge> Chips { get; private set; }
        private ChipPreviewLarge _baseChip;
        public Action<List<ChipPreviewLarge>, UserInput> CreateNewChipsetInEditPane;

        public EditableChipset(IHasDrawLayer parent,float scaleMultiplier) : base(parent, "BlankPixel")
        {
            Chips = new List<ChipPreviewLarge>();
            MultiplyScaleCascade(scaleMultiplier);
        }

        public void AppendChips(List<ChipPreviewLarge> toAdd,int index)
        {
            Chips.InsertRange(index, toAdd);
            AddChildren(toAdd);

            foreach(var chip in toAdd)
            {
                chip.LiftChipFromChipset = _liftChipsFromChipset;
                chip.OutputTextChangedCallback = _refreshText;
            }

            _refreshAll();
        }
        public List<ChipPreviewLarge> PopChips(int index)
        {
            var toRemove = Chips.Skip(index).ToList();
            Chips.RemoveRange(index, toRemove.Count());
            RemoveChildrenAfterUpdate(toRemove);

            _refreshAll();

            return toRemove;
        }

        private void _refreshAll()
        {
            _updateChipPositions();
            _updateChipConnections();
            _refreshText();
            _updateBaseChip();
            _updateChildDimensions();
        }

        public void _liftChipsFromChipset(UserInput input, int index) => CreateNewChipsetInEditPane(PopChips(index), input);
        public int GetChipIndexThatMouseIsOver(UserInput input)
        {
            for (int i = 0; i < Chips.Count; i++)
            {
                if (Chips[i].IsMouseOverAnySection())
                {
                    if (Chips[i].IsMouseOverBottomSection())
                    {
                        return i + 1;
                    }

                    return i;
                }
            }

            return -1;
        }

        private void _updateChipConnections()
        {
            var chipsAboveCurrent = new List<ChipPreviewLarge>();

            foreach(var chip in Chips)
            {
                chip.AddConnectionsFromAbove(chipsAboveCurrent);
                chipsAboveCurrent.Add(chip);
            }
        }

        private void _refreshText() => Chips.ForEach(c => c.RefreshText());

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
        private void _updateBaseChip()
        {
            if (_baseChip != null)
            {
                SetNotDraggableFrom(_baseChip);
            }
            _baseChip = Chips.First();
            SetDraggableFrom(_baseChip);
        }

        public Point DefaultMouseDragOffset => Chips.First().GetCurrentSize() / 2;

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

    }
}
