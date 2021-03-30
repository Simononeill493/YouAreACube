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
        public List<ChipTop> Chips { get; private set; }

        public EditableChipset(IHasDrawLayer parent,float scaleMultiplier,Action<List<ChipTop>,UserInput,EditableChipset> liftChipsCallback) : base(parent, "TopOfChipset")
        {
            Chips = new List<ChipTop>();
            MultiplyScaleCascade(scaleMultiplier);

            _liftChipsCallback = liftChipsCallback;
        }

        #region liftAndDrop
        private Action<List<ChipTop>, UserInput, EditableChipset> _liftChipsCallback;

        public void DropChipset(EditableChipset dropped,UserInput input)
        {
            var chipsToDrop = dropped.PopChips(0);
            var itemToDropOn = _getChipsetSectionMouseIsOver();

            if(itemToDropOn==null)
            {
                Console.WriteLine("Warning: dropped chips onto a chipset that was supposed to be under the mouse, but wasn't");
                return;
            }

            itemToDropOn.DropChipsOn(chipsToDrop, input);
        }
        public void DropChipsOn(List<ChipTop> chips, UserInput input) => AppendChips(chips, 0);

        private void _chipLiftedFromChipset(ChipTop chip, UserInput input)
        {
            var chipsRemoved = PopChips(chip.IndexInChipset);
            _liftChipsCallback(chipsRemoved, input,this);
        }

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

        #region addAndRemoveChips
        public void AppendChip(ChipTop toAdd) => AppendChips(new List<ChipTop>() { toAdd }, 0);
        public void AppendChips(List<ChipTop> toAdd,int index)
        {
            Chips.InsertRange(index, toAdd);
            AddChildren(toAdd);

            foreach(var chip in toAdd)
            {
                chip.UpdateDrawLayerCascade(DrawLayer);
                chip.ChipLiftedCallback = _chipLiftedFromChipset;
                chip.AppendChips = AppendChips;
                chip.RefreshTextCallback = _refreshText;
                chip.RefreshAllCallback = _refreshAll;
            }

            _refreshAll();
        }
        public List<ChipTop> PopChips(int index)
        {
            var toRemove = Chips.Skip(index).ToList();
            Chips.RemoveRange(index, toRemove.Count());
            RemoveChildrenAfterUpdate(toRemove);

            _refreshAll();

            return toRemove;
        }
        #endregion

        #region container
        private IEditableChipsetContainer _currentContainer;

        public void SetContainer(IEditableChipsetContainer newContainer)
        {
            if (newContainer == _currentContainer) { return; }

            _clearContainer();

            newContainer.AddChipset(this);
            _currentContainer = newContainer;
        }
        private void _clearContainer()
        {
            if (_currentContainer != null)
            {
                _currentContainer.RemoveChipset(this);
            }

            _currentContainer = null;
        }
        public override void Dispose()
        {
            _clearContainer();
            base.Dispose();
        }
        #endregion

        public int HeightOfAllChips;
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
        private void _refreshText() => Chips.ForEach(c => c.RefreshText());
    }
}
