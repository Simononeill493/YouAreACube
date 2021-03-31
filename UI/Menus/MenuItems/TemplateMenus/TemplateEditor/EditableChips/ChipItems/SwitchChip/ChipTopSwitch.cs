using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{ 
    class ChipTopSwitch : ChipTop
    {
        private ChipSwitchButtons _switchButtons;
        private SpriteMenuItem _switchSectionBottom;

        private Dictionary<string,EditableChipset> _switchSections;
        private List<EditableChipset> _sectionsList => _switchSections.Values.ToList();

        private EditableChipset _extendedChipset;
        private bool _switchSectionExtended => (_extendedChipset!=null);

        public ChipTopSwitch(IHasDrawLayer parent, ChipData data,List<string> switchInitialOptions) : base(parent, data)
        {
            _createInputSections();
            _switchSections = new Dictionary<string, EditableChipset>();

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

            _extendedChipset = _switchSections[sectionName];

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
        private IChipsetGenerator _generator;
        private List<string> _switchInitialOptions;

        public override void GenerateSubChipsets(IChipsetGenerator generator)
        {
            _generator = generator;
            _addNewSwitchSections(_switchInitialOptions);
        }

        protected void _addNewSwitchSections(List<string> sectionNames) => sectionNames.ForEach(n => _addNewSwitchSection(n));
        protected void _addNewSwitchSection(string sectionName)
        {
            var switchChipset = _generator.CreateChipset();
            switchChipset.TopLevelRefreshAll = TopLevelRefreshAll;
            switchChipset.Visible = false;
            switchChipset.Draggable = false; 
            AddChildAfterUpdate(switchChipset);

            _switchSections[sectionName] = switchChipset;
            _switchButtons.AddSwitchSection(sectionName);
            _switchButtons.UpdateButtonText();
        }
        #endregion

        public override List<EditableChipset> GetSubChipsets() => _sectionsList;
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
            _sectionsList.ForEach(c => c.RefreshAll());

            if(_switchSectionExtended)
            {
                _setSectionLocations();
            }
        }
        protected override void _setTopLevelRefreshAll(Action topLevelRefreshAll)
        {
            base._setTopLevelRefreshAll(topLevelRefreshAll);
            _sectionsList.ForEach(c => c.TopLevelRefreshAll = topLevelRefreshAll);
        }
    }
}