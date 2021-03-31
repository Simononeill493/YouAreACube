using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public partial class EditableChipset : DraggableMenuItem
    {
        public Action TopLevelRefreshAll;

        public void RefreshAll()
        {
            Chips.ForEach(c => c.RefreshAll());

            UpdateInputConnections();
            _updateChipPositions();
            _updateChildDimensions();
            RefreshText();
        }

        private void _updateChipPositions()
        {
            var baseSize = GetBaseSize();
            var cumulativeYOffset = baseSize.Y;

            for (int i = 0; i < Chips.Count; i++)
            {
                Chips[i].IndexInChipset = i;
                Chips[i].SetLocationConfig(0, cumulativeYOffset - (i + 1), CoordinateMode.ParentPixelOffset, false);
                Chips[i].UpdateDimensions(ActualLocation, GetCurrentSize());

                cumulativeYOffset += Chips[i].GetBaseSize().Y;
            }

            HeightOfAllChips = cumulativeYOffset - Chips.Count;
        }

        public void SetInputConnectionsFromAbove(List<ChipTop> chipsAbove) => _setInputConnections(chipsAbove);
        public void UpdateInputConnections() => _setInputConnections(new List<ChipTop>());
        private void _setInputConnections(List<ChipTop> chipsAboveCurrent)
        {
            foreach (var chip in Chips)
            {
                chip.SetInputConnectionsFromAbove(chipsAboveCurrent);
                chipsAboveCurrent.Add(chip);
            }
        }

        public void RefreshText() => Chips.ForEach(c => c.RefreshText());
    }
}
