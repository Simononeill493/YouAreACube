﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockInputDropdownChipsetSelector : BlockInputDropdown
    {
        public BlockInputDropdownChipsetSelector(IHasDrawLayer parent, BlockInputModel model, Func<string> textProvider) : base(parent, new List<string>(), model, textProvider)
        {
        }

        public override void PopulateItems()
        {
            _list.ClearItems();
            _list.AddItems(BlocksetEditPane.Model.GetInputsFromChipsets());
        }
    }
}
