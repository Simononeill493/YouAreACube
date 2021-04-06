using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    public class ChipTop : SpriteMenuItem, IChipsDroppableOn
    {
        public string Name;

        public ChipData ChipData;
        public int IndexInChipset = -1;

        public bool HasOutput => ChipData.HasOutput;

        public ChipTop(string name,IHasDrawLayer parent, ChipData data) : base(parent, "ChipFull") 
        {
            Name = name;
            _actualSize = base.GetBaseSize();

            ChipData = data;
            ColorMask = ChipData.ChipColor;
            OnMouseDraggedOn += _onDragHandler;

            _inputSections = new List<ChipInputSection>();

            var title = new TextMenuItem(this, ChipData.Name);
            title.Color = Color.White;
            title.SetLocationConfig(7, 6, CoordinateMode.ParentPixelOffset, false);
            AddChild(title);

            _createInputSections();
            if (_inputSections.Count > 0)
            {
                _inputSections[_inputSections.Count - 1].SpriteName = "ChipFullEnd";
            }
        }

        #region liftChips
        public Action<ChipTop, UserInput> ChipLiftedCallback;

        private void _onDragHandler(UserInput input)
        {
            if (!_isMouseOverInternalSections())
            {
                ChipLiftedCallback(this, input);
            }
        }
        #endregion

        #region dropChips
        public Action<List<ChipTop>, int> AppendChips;

        public virtual void DropChipsOn(List<ChipTop> chips, UserInput input)
        {
            if (IsMouseOverBottomSection())
            {
                AppendChips(chips, IndexInChipset + 1);
            }
            else
            {
                AppendChips(chips, IndexInChipset);
            }
        }
        #endregion

        #region inputsections
        protected List<ChipInputSection> _inputSections;
        public void ManuallySetInputSection(ChipInputOption item,int index)
        {
            _inputSections[index].ManuallySetDropdown(item);
        }
        
        public void SetInputConnectionsFromAbove(List<ChipTop> chipsAbove)
        {
            _inputSections.ForEach(m => m.SetConnectionsFromAbove(chipsAbove));

            foreach (var subChipset in GetSubChipsets())
            {
                var connectionsList = new List<ChipTop>();
                connectionsList.AddRange(chipsAbove);
                subChipset.SetInputConnectionsFromAbove(connectionsList);
            }
        }

        protected void _createInputSections()
        {
            for (int i = 0; i < ChipData.NumInputs; i++)
            {
                var section = ChipSectionFactory.Create(this, i);
                section.DropdownSelectedCallback = _inputSectionDropdownChanged;
                _inputSections.Add(section);
            }

            AddChildren(_inputSections);
            _setInputSectionPositions();
        }
        protected void _setInputSectionPositions()
        {
            var height = base.GetBaseSize().Y - 1;

            foreach (var section in _inputSections)
            {
                section.SetLocationConfig(0, height, CoordinateMode.ParentPixelOffset);
                height += section.GetBaseSize().Y;
            }

            _actualSize.Y = height;
        }

        protected virtual void _inputSectionDropdownChanged(ChipInputSection section, ChipInputDropdown dropdown, ChipInputOption optionSelected) { }
        #endregion

        #region dimensions
        public bool IsMouseOverAnySection() => MouseHovering | _isMouseOverInternalSections();
        public virtual bool IsMouseOverBottomSection() 
        {
            if(_inputSections.Count == 0)
            {
                return true;
            }

            return _inputSections.Last().MouseHovering;
        } 
        protected virtual bool _isMouseOverInternalSections() => _inputSections.Select(s => s.MouseHovering).Any(h => h);

        public override Point GetBaseSize() => _actualSize;
        protected Point _actualSize;
        #endregion

        #region refresh
        public Action TopLevelRefreshAll { get { return _topLevelRefreshAll; } set { _setTopLevelRefreshAll(value); } }
        protected virtual void _setTopLevelRefreshAll(Action topLevelRefreshAll) => _topLevelRefreshAll = topLevelRefreshAll;
        private Action _topLevelRefreshAll;

        protected void _topLevelRefreshAll_Delayed() => _delayedTopLevelRefreshAll = true;
        private bool _delayedTopLevelRefreshAll = false;

        public Action ChipsetRefreshText;

        public virtual void RefreshAll() { }
        public void RefreshText()
        {
            _inputSections.ForEach(s => s.RefreshText());
            GetSubChipsets().ForEach(s => s.RefreshText());
        }

        public override void Update(UserInput input)
        {
            base.Update(input);

            if(_delayedTopLevelRefreshAll)
            {
                TopLevelRefreshAll();
                _delayedTopLevelRefreshAll = false;
            }
        }
        #endregion

        public IChipsetGenerator _generator;
        public void SetGenerator(IChipsetGenerator generator) => _generator = generator;

        public virtual void GenerateSubChipsets() { }
        public virtual List<EditableChipset> GetSubChipsets() => new List<EditableChipset>();

        public static ChipTop GenerateChipFromChipData(ChipData data,string name = "")
        {
            var initialDrawLayer = ManualDrawLayer.Zero;

            if (data.Name.Equals("If"))
            {
                return new ChipTopSwitch(name,initialDrawLayer, data, new List<string>() { "Yes", "No" });
            }
            if (data.Name.Equals("KeySwitch"))
            {
                return new ChipTopSwitch(name,initialDrawLayer, data, new List<string>() {  });
            }
            else if(data.HasOutput)
            {
                return new ChipTopWithOutput(name,initialDrawLayer, data);
            }
            else
            {
                return new ChipTop(name,initialDrawLayer, data);
            }
        }
    }
}
