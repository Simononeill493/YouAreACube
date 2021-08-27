﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class BlockTopVariableSetter : BlockTop
    {
        private BlockInputSectionVariableAccess _variableDropdown;
        private BlockInputSectionDynamicType _toSetDropdown;

        public BlockTopVariableSetter(string name, IHasDrawLayer parent, BlockData data) : base(name,parent,data)
        {
            _variableDropdown = (BlockInputSectionVariableAccess)InputSections[0];
            _toSetDropdown = (BlockInputSectionDynamicType)InputSections[1];
        }

        public void VariableDropdownChanged(BlockInputOption optionSelected)
        {
            var variable = ((BlockInputOptionVariableAccess)optionSelected).VariableReference;
            var typeName = variable.VariableType.Name;

            CurrentTypeArguments = new List<string>() { typeName };
            _toSetDropdown.SetCurrentType(typeName);
            _topLevelRefreshAll_Delayed();
        }

        protected override List<BlockInputSection> _createInputSections() => BlockSectionFactory.CreateInputSectionsVariableSetter(this);

        public override void SetInitialTypeArguments(List<string> typeArgumentsInitial) 
        {
            CurrentTypeArguments = typeArgumentsInitial;
            _toSetDropdown.SetCurrentType(typeArgumentsInitial.First());
        }

    }
}
