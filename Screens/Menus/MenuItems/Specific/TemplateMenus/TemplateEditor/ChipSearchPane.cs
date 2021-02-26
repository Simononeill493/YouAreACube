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

        private SearchBarMenuItem _searchBar;
        private DropdownMenuItem<ChipType> _dropdown;

        private List<ChipPreviewSmall> _chipPreviews;

        private Action<ChipPreviewSmall, UserInput> _createChip;

        public ChipSearchPane(IHasDrawLayer parentDrawLayer, Action<ChipPreviewSmall, UserInput> createChip) : base(parentDrawLayer, "SearchPane")
        {
            _createChip = createChip;

            _chipPreviews = new List<ChipPreviewSmall>();

            _searchBar = new SearchBarMenuItem(this);
            _searchBar.OnTextChanged += _searchTermChanged;

            _dropdown = new DropdownMenuItem<ChipType>(this);
            _dropdown.SetItems(typeof(ChipType).GetEnumValues().Cast<ChipType>().ToList());
            _dropdown.OnSelectedChanged += _chipTypeChanged;

            _searchBar.SetLocationConfig(50, 6, CoordinateMode.ParentPercentageOffset, true);
            _dropdown.SetLocationConfig(50, 19, CoordinateMode.ParentPercentageOffset, true);

            AddChild(_searchBar);
            AddChild(_dropdown);

            _setPreviews(ChipDatabase.BuiltInChips.Values.ToList());
        }

        private void _tryCreateChip(ChipPreviewSmall preview, UserInput input)
        {
            if(!MenuScreen.UserDragging)
            {
                _createChip(preview, input);
            }
        }

        private void _setPreviews(List<ChipData> chips)
        {
            _clearPreviews();

            for (int i = 0; i < Math.Min(chips.Count, MaxVisibleChips); i++)
            {
                var chipPreview = new ChipPreviewSmall(this, chips[i]);
                chipPreview.SetLocationConfig(3, 25 + (20 * (i + 1)), CoordinateMode.ParentPixelOffset, false);
                chipPreview.OnMousePressed += (input) => _tryCreateChip(chipPreview, input);

                _chipPreviews.Add(chipPreview);
                AddChildAfterUpdate(chipPreview);
            }
        }
        private void _clearPreviews()
        {
            RemoveChildrenAfterUpdate(_chipPreviews);
            _chipPreviews.Clear();
        }

        private void _refreshFilter()
        {
            var chips = ChipDatabase.BuiltInChips.Values;
            var filtered = chips.Where(c => c.Name.ToLower().Contains(_searchBar.Text.ToLower()));
            if(_dropdown.IsItemSelected)
            {
                filtered = filtered.Where(c => c.ChipType == _dropdown.Selected);
            }

            _setPreviews(filtered.ToList());
        }
        private void _searchTermChanged(string searchTerm)
        {
            _refreshFilter();
        }
        private void _chipTypeChanged(ChipType chipType)
        {
            _refreshFilter();
        }
    }
}
