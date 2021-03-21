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
        public const int PreviewPixelXOffset = 3;
        public const int PreviewPixeYInit = 45;
        public const int PreviewPixeYDistance = 20;

        private SearchBarMenuItem _searchBar;
        private DropdownMenuItem<ChipType> _dropdown;

        private List<ChipPreview> _chipPreviews;

        private Action<ChipPreview, UserInput> _tryCreateChipInEditPane;
        public void SetCreateChipCallback(Action<ChipPreview, UserInput> tryCreateChip) => _tryCreateChipInEditPane = tryCreateChip;

        public ChipSearchPane(IHasDrawLayer parentDrawLayer) : base(parentDrawLayer, "SearchPane")
        {
            _chipPreviews = new List<ChipPreview>();

            _searchBar = new SearchBarMenuItem(this);
            _searchBar.SetLocationConfig(50, 6, CoordinateMode.ParentPercentageOffset, true);
            _searchBar.OnTextChanged += _searchTermChanged;
            AddChild(_searchBar);

            _dropdown = new DropdownMenuItem<ChipType>(this);
            _dropdown.SetLocationConfig(50, 19, CoordinateMode.ParentPercentageOffset, true);
            _dropdown.SetItems(typeof(ChipType).GetEnumValues().Cast<ChipType>().ToList());
            _dropdown.OnSelectedChanged += _chipTypeChanged;
            AddChild(_dropdown);
        }
        
        public void RefreshFilter()
        {
            var filtered = ChipDatabase.SearchChips(_searchBar.Text);
            filtered = _dropdown.IsItemSelected ? filtered.Where(c => c.ChipType == _dropdown.Selected) : filtered;

            _setPreviews(filtered.ToList());
        }

        private void _setPreviews(List<ChipData> chips)
        {
            _clearPreviews();
            var numChipsToShow = Math.Min(chips.Count, MaxVisibleChips);
            var yOffset = PreviewPixeYInit;

            for (int i = 0; i < numChipsToShow; i++)
            {
                var chipPreview = new ChipPreview(this, chips[i]);
                chipPreview.SetLocationConfig(PreviewPixelXOffset, yOffset, CoordinateMode.ParentPixelOffset, false);
                chipPreview.OnMousePressed += (input) => _tryCreateChipInEditPane(chipPreview, input);
                _chipPreviews.Add(chipPreview);

                yOffset += PreviewPixeYDistance;
            }

            AddChildrenAfterUpdate(_chipPreviews);
        }
        private void _clearPreviews()
        {
            RemoveChildrenAfterUpdate(_chipPreviews);
            _chipPreviews.Clear();
        }

        private void _searchTermChanged(string searchTerm) => RefreshFilter();
        private void _chipTypeChanged(ChipType chipType) => RefreshFilter();
    }
}
