using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{ 
    class ChipTopSwitch : ChipTop
    {
        public Dictionary<string, EditableChipset> SwitchSections;
        public List<EditableChipset> SectionsList => SwitchSections.Values.ToList();

        private ChipSwitchButtons _switchButtons;
        private SpriteMenuItem _switchSectionBottom;

        private EditableChipset _extendedChipset;
        private bool _switchSectionExtended => (_extendedChipset!=null);

        public ChipTopSwitch(string name,IHasDrawLayer parent, ChipData data,List<string> switchInitialOptions) : base(name,parent, data)
        {
            SwitchSections = new Dictionary<string, EditableChipset>();

            _switchButtons = new ChipSwitchButtons(this, ColorMask, () => _closeSwitchSection(true), _openSwitchSection);
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
                _extendedChipset.Visible = false;
                _extendedChipset.Enabled = false;

            }

            _switchSectionBottom.Visible = false;
            _switchSectionBottom.Enabled = false;

            _actualSize = _unextendedSize;

            _extendedChipset = null;

            if (shouldRefresh)
            {
                TopLevelRefreshAll();
            }
        }
        private void _openSwitchSection(string sectionName)
        {
            _closeSwitchSection(false);

            _extendedChipset = SwitchSections[sectionName];

            TopLevelRefreshAll();
        }

        #region dimensions
        private Point _unextendedSize;

        private void _setSectionLocations()
        {
            _actualSize = _unextendedSize;

            _extendedChipset.Visible = true;
            _extendedChipset.Enabled = true;
            _extendedChipset.SetLocationConfig(0, _actualSize.Y - 1, CoordinateMode.ParentPixelOffset);
            _extendedChipset.UpdateDrawLayerCascade(DrawLayer - DrawLayers.MinLayerDistance);
            _actualSize.Y += _extendedChipset.HeightOfAllChips - 1;

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
                return _switchSectionBottom.MouseHovering | _extendedChipset.IsMouseOverAnyChip();
            }

            return false;
        }
        #endregion

        #region addSection
        private List<string> _switchInitialOptions;

        public override void GenerateSubChipsets()
        {
            _addNewSwitchSections(_switchInitialOptions);
        }

        protected void _addNewSwitchSections(List<string> sectionNames) => sectionNames.ForEach(n => _addNewSwitchSection(n));
        protected void _addNewSwitchSection(string sectionName) => AddSwitchSection(sectionName, _generator.CreateChipset(Name+"-subChip_"+(GetSubChipsets().Count+1.ToString())));

        public void AddSwitchSection(string sectionName,EditableChipset switchChipset)
        {
            switchChipset.TopLevelRefreshAll = TopLevelRefreshAll;
            switchChipset.Visible = false;
            switchChipset.Draggable = false;
            AddChildAfterUpdate(switchChipset);

            SwitchSections[sectionName] = switchChipset;
            _switchButtons.AddSwitchSection(sectionName);
            _switchButtons.UpdateButtonText();
        }
        #endregion

        public override List<EditableChipset> GetSubChipsets() => SectionsList;
        public override void DropChipsOn(List<ChipTop> chips, UserInput input) 
        {
            if (!_switchSectionExtended)
            {
                base.DropChipsOn(chips, input);
            }
            else
            {
                if(_extendedChipset.IsMouseOverAnyChip())
                {
                    _extendedChipset.DropChipsOn(chips, input);
                }
                else if(_switchSectionBottom.MouseHovering)
                {
                    _extendedChipset.AppendToBottom(chips);
                }
                else
                {
                    base.DropChipsOn(chips, input);
                }
            }
        }

        public override void RefreshAll()
        {
            base.RefreshAll();
            SectionsList.ForEach(c => c.RefreshAll());

            if(_switchSectionExtended)
            {
                _setSectionLocations();
            }
        }
        protected override void _setTopLevelRefreshAll(Action topLevelRefreshAll)
        {
            base._setTopLevelRefreshAll(topLevelRefreshAll);
            SectionsList.ForEach(c => c.TopLevelRefreshAll = topLevelRefreshAll);
        }
    }
}