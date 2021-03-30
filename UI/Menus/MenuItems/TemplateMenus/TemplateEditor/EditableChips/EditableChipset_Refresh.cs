using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public partial class EditableChipset : DraggableMenuItem
    {
        private void _refreshAll()
        {
            _updateChipConnections();
            _updateChipPositions();
            _updateChildDimensions();
            _refreshText();
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

            HeightOfAllChips = cumulativeYOffset;
        }

        private void _updateChipConnections()
        {
            var chipsAboveCurrent = new List<ChipTop>();

            foreach (var chip in Chips)
            {
                chip.SetInputConnectionsFromAbove(chipsAboveCurrent);
                chipsAboveCurrent.Add(chip);
            }
        }
    }
}
