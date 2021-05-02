using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    partial class EditableChipset : DraggableMenuItem, IChipsDroppableOn
    {
        public string Name;
        public List<ChipTop> Chips { get; private set; }

        public EditableChipset(string name,IHasDrawLayer parent,float scaleMultiplier,Action<List<ChipTop>,UserInput,EditableChipset> liftChipsCallback) : base(parent, "TopOfChipset")
        {
            Name = name;
            Chips = new List<ChipTop>();
            MultiplyScaleCascade(scaleMultiplier);

            _liftChipsCallback = liftChipsCallback;
            HeightOfAllChips += GetBaseSize().Y;
        }

        #region lift
        private Action<List<ChipTop>, UserInput, EditableChipset> _liftChipsCallback;

        private void _chipLiftedFromChipset(ChipTop chip, UserInput input)
        {
            var chipsRemoved = PopChips(chip.IndexInChipset);
            _liftChipsCallback(chipsRemoved, input, this);
        }
        #endregion

        #region drop
        public void AppendToTop(List<ChipTop> chipsToDrop) => AppendChips(chipsToDrop, 0);
        public void AppendToBottom(List<ChipTop> chipsToDrop) => AppendChips(chipsToDrop, Chips.Count);

        public void DropChipsOn(List<ChipTop> chipsToDrop, UserInput input)
        {
            var itemToDropOn = _getChipsetSectionMouseIsOver();

            if(itemToDropOn==null)
            {
                Console.WriteLine("Warning: dropped chips onto a chipset that was supposed to be under the mouse, but wasn't");
                return;
            }
            else if(itemToDropOn==this)
            {
                AppendToTop(chipsToDrop);
            }
            else
            {
                itemToDropOn.DropChipsOn(chipsToDrop, input);
            }
        }
        #endregion

        #region addAndRemoveChips
        public void AppendChipToStart(ChipTop toAdd) => AppendChips(new List<ChipTop>() { toAdd }, 0);
        public void AppendChipToEnd(ChipTop toAdd) => AppendChips(new List<ChipTop>() { toAdd }, Chips.Count);
        public void AppendChips(List<ChipTop> toAdd, int index)
        {
            Chips.InsertRange(index, toAdd);
            AddChildren(toAdd);

            foreach (var chip in toAdd)
            {
                chip.UpdateDrawLayerCascade(DrawLayer);
                chip.ChipLiftedCallback = _chipLiftedFromChipset;
                chip.AppendChips = AppendChips;
                chip.ChipsetRefreshText = RefreshText;
                chip.TopLevelRefreshAll = TopLevelRefreshAll;
            }

            TopLevelRefreshAll();
        }
        public List<ChipTop> PopChips(int index)
        {
            var toRemove = Chips.Skip(index).ToList();
            Chips.RemoveRange(index, toRemove.Count());
            RemoveChildrenAfterUpdate(toRemove);

            TopLevelRefreshAll();
            return toRemove;
        }
        #endregion

        #region container
        private IEditableChipsetContainer _currentContainer;

        public void SetContainer(IEditableChipsetContainer newContainer)
        {
            if (newContainer == _currentContainer) { return; }

            ClearContainer();

            newContainer.AddChipset(this);
            _currentContainer = newContainer;
        }
        public void ClearContainer()
        {
            if (_currentContainer != null)
            {
                _currentContainer.RemoveChipset(this);
            }

            _currentContainer = null;
        }
        public override void Dispose()
        {
            if (_currentContainer != null)
            {
                throw new Exception("Tried to dispose a chipset that is still contained!");
            }

            base.Dispose();
        }
        #endregion

        #region dimensions
        public int HeightOfAllChips;

        private IChipsDroppableOn _getChipsetSectionMouseIsOver()
        {
            if (MouseHovering)
            {
                return this;
            }

            foreach (var chip in Chips)
            {
                if (chip.IsMouseOverAnySection())
                {
                    return chip;
                }
            }

            return null;
        }
        public bool IsMouseOverAnyChip() => _getChipsetSectionMouseIsOver() != null;
        #endregion

        public List<EditableChipset> GetThisAndSubChipsets()
        {
            var sub = GetSubChipsets();
            sub.Add(this);
            return sub;
        }
        public List<EditableChipset> GetSubChipsets()
        {
            var output = new List<EditableChipset>();
            var subChipsets = Chips.Select(c => c.GetSubChipsets());

            foreach (var sublist in subChipsets)
            {
                output.AddRange(sublist);
            }
            return output;
        }
    }
}
