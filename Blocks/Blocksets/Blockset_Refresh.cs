using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public partial class Blockset : SpriteMenuItem
    {
        public Action TopLevelRefreshAll 
        { 
            get { return _topLevelRefreshAll; } 
            set 
            {
                _topLevelRefreshAll = value;
                foreach(var c in Blocks)
                {
                    c.Callbacks.TopLevelRefreshAll = _topLevelRefreshAll;
                }
            }
        }
        private Action _topLevelRefreshAll;

        public void RefreshAll()
        {
            Blocks.ForEach(c => c.RefreshAll());

            RefreshInputConnections();
            _updateChipPositions();
            _updateChildLocations();
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
                Blocks[i].UpdateLocation(ActualLocation, GetCurrentSize());

                cumulativeYOffset += Blocks[i].GetBaseSize().Y;
            }

            HeightOfAllBlocks = cumulativeYOffset - Blocks.Count;
        }

        public void RefreshInputConnections(List<BlockTop> chipsAbove, TemplateVariableSet variables) => _refreshInputConnections(chipsAbove,variables);
        public void RefreshInputConnections() => _refreshInputConnections(new List<BlockTop>(),_variableProvider.GetVariables());
        private void _refreshInputConnections(List<BlockTop> chipsAboveCurrent, TemplateVariableSet variables)
        {
            foreach (var chip in Blocks)
            {
                chip.RefreshInputConnections(chipsAboveCurrent,variables);
                chipsAboveCurrent.Add(chip);
            }
        }

        public void RefreshText() => Blocks.ForEach(c => c.RefreshText());
    }
}
