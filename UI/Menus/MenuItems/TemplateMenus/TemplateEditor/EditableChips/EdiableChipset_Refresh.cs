using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    partial class EditableChipset : DraggableMenuItem
    {
        private ChipTop _topChip;

        private void _refreshAll()
        {
            _updateChipPositions();
            _updateChipConnections();

            _setTopChip();
            _refreshText();

            _updateChildDimensions();
        }

        private void _updateChipPositions()
        {
            var baseSize = GetBaseSize();

            for (int i = 0; i < Chips.Count; i++)
            {
                Chips[i].CurrentPositionInChipset = i;
                Chips[i].SetLocationConfig(0, baseSize.Y - (i+1), CoordinateMode.ParentPixelOffset, false);
                Chips[i].UpdateDimensions(ActualLocation, GetCurrentSize());

                baseSize.Y += Chips[i].GetFullBaseSize().Y;
            }
        }

        private void _updateChipConnections()
        {
            var chipsAboveCurrent = new List<ChipTop>();

            foreach (var chip in Chips)
            {
                chip.SetConnectionsFromAbove(chipsAboveCurrent);
                chipsAboveCurrent.Add(chip);
            }
        }

        private void _setTopChip()
        {
            if (_topChip != null)
            {
                SetNotDraggableFrom(_topChip);
            }

            _topChip = Chips.First();
            SetDraggableFrom(_topChip);
        }

        private void _refreshText() => Chips.ForEach(c => c.RefreshText());
    }
}
