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

        private IChipsetGenerator _generator;
        private Point _unextendedSize;

        private Dictionary<string,EditableChipset> _switchSections;

        public ChipTopSwitch(IHasDrawLayer parent, ChipData data,List<string> switchInitialOptions) : base(parent, data)
        {
            _createInputSections();
            _switchSections = new Dictionary<string, EditableChipset>();

            _switchButtons = new ChipSwitchButtons(this, ColorMask);
            _switchButtons.SetLocationConfig(0, GetBaseSize().Y-1,CoordinateMode.ParentPixelOffset);
            AddChild(_switchButtons);

            _switchSectionBottom = new SpriteMenuItem(this, "ChipFullGreyed") { Visible = false };
            AddChild(_switchSectionBottom);

            _actualSize.Y += _switchButtons.GetBaseSize().Y-1;
            _unextendedSize = _actualSize;

            _addNewSwitchSections(switchInitialOptions);
        }

        protected void _addNewSwitchSections(List<string> sectionNames) => sectionNames.ForEach(n => _addNewSwitchSection(n));
        protected void _addNewSwitchSection(string sectionName)
        {
            var switchChipset = _generator.CreateChipset();
            _switchSections[sectionName] = switchChipset;
        }

        public override void GenerateSubChipsets(IChipsetGenerator generator) => _generator = generator;
        public override List<EditableChipset> GetSubChipsets() => _switchSections.Values.ToList();

        public override void DropChipsOn(List<ChipTop> chips, UserInput input) { }

        public override bool IsMouseOverBottomSection() => _switchButtons.MouseHovering;
        protected override bool _isMouseOverInternalSections => base._isMouseOverInternalSections | _switchButtons.MouseHovering;

    }
}