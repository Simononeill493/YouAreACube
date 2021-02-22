using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ChipSearchPane : SpriteMenuItem
    {
        public ChipSearchPane(IHasDrawLayer parentDrawLayer) : base(parentDrawLayer, "SearchPane")
        {
            var searchBar = new SearchBarMenuItem(this);
            searchBar.OnTextChanged += _searchTermChanged;

            var chipTypeDropdown = new DropdownMenuItem<ChipType>(this);
            chipTypeDropdown.SetItems(typeof(ChipType).GetEnumValues().Cast<ChipType>().ToList());
            chipTypeDropdown.OnSelectedChanged += _chipTypeChanged;

            searchBar.SetLocationConfig(50, 6, CoordinateMode.ParentPercentageOffset, true);
            chipTypeDropdown.SetLocationConfig(50, 19, CoordinateMode.ParentPercentageOffset, true);

            AddChild(searchBar);
            AddChild(chipTypeDropdown);
        }

        private void _searchTermChanged(string searchTerm){}
        private void _chipTypeChanged(ChipType chipType){}

    }
}
