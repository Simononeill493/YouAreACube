using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockSearchPane : SpriteMenuItem
    {
        public const int MaxVisibleChips = 6;
        public const int PreviewPixelXOffset = 3;
        public const int PreviewPixeYInit = 45;
        public const int PreviewPixeYDistance = 20;

        public Action<BlockData,UserInput> SendToEditPane;

        //private SearchBarMenuItem _searchBar;
        //private DropdownMenuItem<ChipType> _dropdown;
        private List<BlockPreview> _chipPreviews;

        private string _searchTerm = "";
        private ChipType _selectedChipType;

        public BlockSearchPane(IHasDrawLayer parentDrawLayer) : base(parentDrawLayer, BuiltInMenuSprites.SearchPane)
        {
            _chipPreviews = new List<BlockPreview>();

            var _searchBar = new SearchBarMenuItem(this,()=>_searchTerm,_searchTermChanged);
            _searchBar.SetLocationConfig(50, 6, CoordinateMode.ParentPercentageOffset, true);
            AddChild(_searchBar);


            var _dropdown = new DropdownMenuItem<ChipType>(this, () => _selectedChipType, (v) => { _selectedChipType = v; }, () => typeof(ChipType).GetEnumValues().Cast<ChipType>().ToList());
            _dropdown.SetLocationConfig(50, 19, CoordinateMode.ParentPercentageOffset, true);
            _dropdown.OnSelectedChanged += _chipTypeChanged;
            AddChild(_dropdown);

            RefreshFilter();
        }
        
        public void RefreshFilter()
        {
            var filtered = BlockDataDatabase.SearchBaseBlocks(_searchTerm);
            filtered = filtered.Where(c => c.ChipDataType == _selectedChipType);

            _setPreviews(filtered.ToList());
        }

        private void _setPreviews(List<BlockData> chips)
        {
            _clearPreviews();
            var numChipsToShow = Math.Min(chips.Count, MaxVisibleChips);
            var yOffset = PreviewPixeYInit;

            for (int i = 0; i < numChipsToShow; i++)
            {
                var chipPreview = new BlockPreview(this, chips[i]);
                chipPreview.SetLocationConfig(PreviewPixelXOffset, yOffset, CoordinateMode.ParentPixelOffset, false);
                chipPreview.OnMousePressed += (input) => _createChipAndAddToEditPane(chipPreview, input);
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

        private void _searchTermChanged(string searchTerm)
        {
            _searchTerm = searchTerm;
            RefreshFilter();
        }
        private void _chipTypeChanged(ChipType chipType) => RefreshFilter();

        private void _createChipAndAddToEditPane(BlockPreview preview, UserInput input) => SendToEditPane(preview.Block, input);
    }
}
