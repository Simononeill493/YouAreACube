using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    partial class EditableChipset : DraggableMenuItem
    {
        public List<ChipTopSection> Chips { get; private set; }
        public Action<List<ChipTopSection>, UserInput> LiftChipsCallback;

        public EditableChipset(IHasDrawLayer parent,float scaleMultiplier) : base(parent, "BlankPixel")
        {
            Chips = new List<ChipTopSection>();
            MultiplyScaleCascade(scaleMultiplier);
        }

        public void AppendChips(List<ChipTopSection> toAdd,int index)
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
        public List<ChipTopSection> PopChips(int index)
        {
            var toRemove = Chips.Skip(index).ToList();
            Chips.RemoveRange(index, toRemove.Count());
            RemoveChildrenAfterUpdate(toRemove);

            _refreshAll();

            return toRemove;
        }

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

        private void _liftChipsFromChipset(UserInput input, int index) => LiftChipsCallback(PopChips(index), input);
    }
}
