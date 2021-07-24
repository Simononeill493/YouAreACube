using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{ 
    class BlockTopSwitch : BlockTop
    {
        public List<Blockset> SwitchBlocksets;

        private BlockSwitchButtons _switchButtons;
        private SpriteMenuItem _switchSectionBottom;

        private Blockset _extendedBlockset;
        private bool _switchSectionExtended => (_extendedBlockset!=null);

        public BlockTopSwitch(string name,IHasDrawLayer parent, BlockData data,List<string> switchInitialOptions) : base(name,parent, data)
        {
            SwitchBlocksets = new List<Blockset>();

            _switchButtons = new BlockSwitchButtons(this, ColorMask, () => _closeSwitchSection(true), _openSwitchSection);
            _switchButtons.SetLocationConfig(0, GetBaseSize().Y-1,CoordinateMode.ParentPixelOffset);
            AddChild(_switchButtons);

            _switchSectionBottom = new SpriteMenuItem(this, "ChipFullGreyed") { Visible = false };
            AddChild(_switchSectionBottom);

            _actualSize.Y += _switchButtons.GetBaseSize().Y-1;
            _unextendedSize = _actualSize;

            _switchInitialOptions = switchInitialOptions;
        }

        private void _closeSwitchSection(bool shouldRefresh)
        {
            if(_switchSectionExtended)
            {
                _extendedBlockset.Visible = false;
                _extendedBlockset.Enabled = false;

            }

            _switchSectionBottom.Visible = false;
            _switchSectionBottom.Enabled = false;

            _actualSize = _unextendedSize;

            _extendedBlockset = null;

            if (shouldRefresh)
            {
                TopLevelRefreshAll();
            }
        }
        private void _openSwitchSection(int sectionIndex)
        {
            _closeSwitchSection(false);

            _extendedBlockset = SwitchBlocksets[sectionIndex];

            TopLevelRefreshAll();
        }

        #region dimensions
        private IntPoint _unextendedSize;

        private void _setSectionLocations()
        {
            _actualSize = _unextendedSize;

            _extendedBlockset.Visible = true;
            _extendedBlockset.Enabled = true;
            _extendedBlockset.SetLocationConfig(0, _actualSize.Y - 1, CoordinateMode.ParentPixelOffset);
            _extendedBlockset.UpdateDrawLayerCascade(DrawLayer - DrawLayers.MinLayerDistance);
            _actualSize.Y += _extendedBlockset.HeightOfAllBlocks - 1;

            _switchSectionBottom.Visible = true;
            _switchSectionBottom.Enabled = true;

            _switchSectionBottom.SetLocationConfig(0, _actualSize.Y, CoordinateMode.ParentPixelOffset);
            _actualSize.Y += _switchSectionBottom.GetBaseSize().Y;
        }

        public override bool IsMouseOverBottomSection() => _switchSectionExtended ? _switchSectionBottom.MouseHovering : _switchButtons.MouseHovering;
        protected override bool _isMouseOverInternalSections()
        {
            if (base._isMouseOverInternalSections() | _switchButtons.MouseHovering)
            {
                return true;
            }

            if (_switchSectionExtended)
            {
                return _switchSectionBottom.MouseHovering | _extendedBlockset.IsMouseOverAnyChip();
            }

            return false;
        }
        #endregion

        #region addSection
        private List<string> _switchInitialOptions;
        public override void GenerateSubChipsets() => _addNewSwitchSections(_switchInitialOptions);
        protected void _addNewSwitchSections(List<string> sectionNames) => sectionNames.ForEach(n => _addNewSwitchSection(n));
        protected void _addNewSwitchSection(string sectionName) => AddSwitchSection(sectionName, _generator.CreateBlockset(Name+"-subChip_"+(GetSubChipsets().Count+1.ToString())));

        public void AddSwitchSection(string sectionName,Blockset switchChipset)
        {
            switchChipset.TopLevelRefreshAll = TopLevelRefreshAll;
            switchChipset.Enabled = false;
            switchChipset.Visible = false;
            switchChipset.Draggable = false;
            AddChildAfterUpdate(switchChipset);

            SwitchBlocksets.Add(switchChipset);
            _switchButtons.AddSwitchSection(sectionName);
            _switchButtons.UpdateButtonText();
        }
        #endregion

        public override List<Blockset> GetSubChipsets() => SwitchBlocksets.ToList();
        public override void DropBlocksOn(List<BlockTop> chips, UserInput input) 
        {
            if (!_switchSectionExtended)
            {
                base.DropBlocksOn(chips, input);
            }
            else
            {
                if(_extendedBlockset.IsMouseOverAnyChip())
                {
                    _extendedBlockset.DropBlocksOn(chips, input);
                }
                else if(_switchSectionBottom.MouseHovering)
                {
                    _extendedBlockset.AppendBlocksToBottom(chips);
                }
                else
                {
                    base.DropBlocksOn(chips, input);
                }
            }
        }


        public List<(string,Blockset)> GetSwitchSectionsWithNames()
        {
            var names = _switchButtons.SwitchSectionsNames;
            if (SwitchBlocksets.Count != names.Count)
            {
                throw new Exception("Tried to extract switch chip section names, but the numbers don't match up.");
            }

            var output = new List<(string, Blockset)>();
            for(int i=0;i<SwitchBlocksets.Count;i++)
            {
                output.Add((names[i],SwitchBlocksets[i]));
            }
            return output;
        }


        public override void RefreshAll()
        {
            base.RefreshAll();
            SwitchBlocksets.ForEach(c => c.RefreshAll());
            _switchButtons.UpdateButtonText();

            if (_switchSectionExtended)
            {
                _setSectionLocations();
            }
        }
        protected override void _setTopLevelRefreshAll(Action topLevelRefreshAll)
        {
            base._setTopLevelRefreshAll(topLevelRefreshAll);
            SwitchBlocksets.ForEach(c => c.TopLevelRefreshAll = topLevelRefreshAll);
        }

    }
}