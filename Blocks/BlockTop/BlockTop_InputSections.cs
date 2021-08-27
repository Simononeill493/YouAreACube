using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    partial class BlockTop
    {
        public List<BlockInputSection> InputSections = new List<BlockInputSection>();
        public List<BlockInputOption> GetCurrentInputs() => InputSections.Select(section => section.CurrentlySelected).ToList();
        public void ManuallySetInputSection(BlockInputOption item, int index) => InputSections[index].ManuallySetInput(item);

        public virtual void RefreshInputConnections(List<BlockTop> chipsAbove, TemplateVariableSet variables)
        {
            InputSections.ForEach(m => m.RefreshInputConnections(chipsAbove,variables));

            foreach (var subBlockset in GetSubBlocksets())
            {
                var connectionsList = new List<BlockTop>();
                connectionsList.AddRange(chipsAbove);
                subBlockset.RefreshInputConnections(connectionsList,variables);
            }
        }

        protected void _addInputSections()
        {
            var sections = _createInputSections();

            sections.ForEach(s => _setInputSectionData(s));
            AddChildren(InputSections);
            _setInputSectionPositions();
        }

        protected virtual List<BlockInputSection> _createInputSections() => BlockSectionFactory.CreateInputSectionsBasic(this, BlockData.NumInputs);

        private void _setInputSectionData(BlockInputSection section)
        {
            section.ItemSelectedCallback = _inputSectionSelectionChanged;
            InputSections.Add(section);
        }

        protected virtual void _inputSectionSelectionChanged(BlockInputSection section, BlockInputOption optionSelected)
        {
            if (optionSelected.OptionType == InputOptionType.Reference | optionSelected.OptionType == InputOptionType.Variable)
            {
                string dataTypeFeedingIn;
                if(optionSelected.OptionType == InputOptionType.Reference)
                {
                    var referenceOption = (BlockInputOptionReference)optionSelected;
                    dataTypeFeedingIn = referenceOption.BlockReference.OutputTypeCurrent;

                }
                else if(optionSelected.OptionType == InputOptionType.Variable)
                {
                    var variableOption = (BlockInputOptionVariable)optionSelected;
                    dataTypeFeedingIn = variableOption.VariableReference.VariableType.Name;
                }
                else
                {
                    throw new Exception();
                }

                var inputOptions = section.InputBaseTypes;
                if (inputOptions.Contains("List<Variable>"))
                {
                    var afterOpeningList = dataTypeFeedingIn.Substring(5);
                    var extracted = afterOpeningList.Substring(0, afterOpeningList.Length - 1);
                    CurrentTypeArguments[0] = extracted;
                }
                else if (inputOptions.Contains("Variable"))
                {
                    CurrentTypeArguments[0] = dataTypeFeedingIn;
                }
                else if (inputOptions.Contains("AnyCube"))
                {
                    CurrentTypeArguments[0] = dataTypeFeedingIn;
                }
            }

            if (optionSelected.OptionType == InputOptionType.SubMenu)
            {
                var menuOption = (BlockInputOptionSubMenu)optionSelected;

                TopLevelContainer.OpenInputSubMenu(menuOption.MenuToOpen, section);
            }
        }

        protected void _setInputSectionPositions()
        {
            var height = base.GetBaseSize().Y - 1;

            foreach (var section in InputSections)
            {
                section.SetLocationConfig(0, height, CoordinateMode.ParentPixelOffset);
                height += section.GetBaseSize().Y;
            }

            _topSectionActualSize.Y = height;
        }

        private void _setEndSpriteForLastInputSection()
        {
            if (InputSections.Count > 0)
            {
                InputSections[InputSections.Count - 1].SpriteName = BuiltInMenuSprites.BlockBottom;
            }
        }
    }
}
