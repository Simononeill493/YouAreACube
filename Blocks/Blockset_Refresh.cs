using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public partial class Blockset : DraggableMenuItem
    {
        public Action TopLevelRefreshAll 
        { 
            get { return _topLevelRefreshAll; } 
            set 
            {
                _topLevelRefreshAll = value;
                foreach(var c in Blocks)
                {
                    c.TopLevelRefreshAll = _topLevelRefreshAll;
                }
            }
        }
        private Action _topLevelRefreshAll;

        public void RefreshAll()
        {
            Blocks.ForEach(c => c.RefreshAll());

            UpdateInputConnections();
            _updateChipPositions();
            _updateChildDimensions();
            RefreshText();
        }

        private void _updateChipPositions()
        {
            var baseSize = GetBaseSize();
            var cumulativeYOffset = baseSize.Y;

            for (int i = 0; i < Blocks.Count; i++)
            {
                Blocks[i].IndexInBlockset = i;
                Blocks[i].SetLocationConfig(0, cumulativeYOffset - (i + 1), CoordinateMode.ParentPixelOffset, false);
                Blocks[i].UpdateDimensions(ActualLocation, GetCurrentSize());

                cumulativeYOffset += Blocks[i].GetBaseSize().Y;
            }

            HeightOfAllBlocks = cumulativeYOffset - Blocks.Count;
        }

        public void SetInputConnectionsFromAbove(List<BlockTop> chipsAbove) => _setInputConnections(chipsAbove);
        public void UpdateInputConnections() => _setInputConnections(new List<BlockTop>());
        private void _setInputConnections(List<BlockTop> chipsAboveCurrent)
        {
            foreach (var chip in Blocks)
            {
                chip.SetInputConnectionsFromAbove(chipsAboveCurrent);
                chipsAboveCurrent.Add(chip);
            }
        }

        public void RefreshText() => Blocks.ForEach(c => c.RefreshText());
    }
}
