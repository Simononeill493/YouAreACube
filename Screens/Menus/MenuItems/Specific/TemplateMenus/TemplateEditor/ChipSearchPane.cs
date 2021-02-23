using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ChipSearchPane : SpriteMenuItem
    {
        public const int MaxVisibleChips = 6;
        private bool _toRefresh;

        private SearchBarMenuItem searchBar;
        private DropdownMenuItem<ChipType> dropdown;

        private List<ChipPreviewSmall> _chipPreviews;

        public ChipSearchPane(IHasDrawLayer parentDrawLayer) : base(parentDrawLayer, "SearchPane")
        {
            _chipPreviews = new List<ChipPreviewSmall>();

            searchBar = new SearchBarMenuItem(this);
            searchBar.OnTextChanged += _searchTermChanged;

            dropdown = new DropdownMenuItem<ChipType>(this);
            dropdown.SetItems(typeof(ChipType).GetEnumValues().Cast<ChipType>().ToList());
            dropdown.OnSelectedChanged += _chipTypeChanged;

            searchBar.SetLocationConfig(50, 6, CoordinateMode.ParentPercentageOffset, true);
            dropdown.SetLocationConfig(50, 19, CoordinateMode.ParentPercentageOffset, true);

            AddChild(searchBar);
            AddChild(dropdown);

            SetPreviews(ChipDatabase.BuiltInChips.Values.ToList());
        }

        public override void Update(UserInput input)
        {
            base.Update(input);
            if(_toRefresh)
            {
                _refreshFilter();
                _toRefresh = false;
            }
        }

        private void SetPreviews(List<ChipData> chips)
        {
            _clearPreviews();

            for (int i = 0; i < Math.Min(chips.Count, MaxVisibleChips); i++)
            {
                var chipPreview = new ChipPreviewSmall(this);
                chipPreview.SetLocationConfig(3, 25 + (20 * (i + 1)), CoordinateMode.ParentPixelOffset, false);
                chipPreview.SetText(chips[i].Name);

                _chipPreviews.Add(chipPreview);
                AddChild(chipPreview);
            }
        }

        private void _clearPreviews()
        {
            RemoveChildren(_chipPreviews);
            _chipPreviews.Clear();
        }
        private void _refreshFilter()
        {
            var chips = ChipDatabase.BuiltInChips.Values;
            var filtered = chips.Where(c => c.Name.ToLower().Contains(searchBar.Text.ToLower()));
            if(dropdown.IsItemSelected)
            {
                filtered = filtered.Where(c => c.ChipType == dropdown.Selected);
            }

            SetPreviews(filtered.ToList());
            _updateChildLocations();
        }
        private void _searchTermChanged(string searchTerm)
        {
            _toRefresh = true;
        }
        private void _chipTypeChanged(ChipType chipType)
        {
            _toRefresh = true;
        }
    }
}
