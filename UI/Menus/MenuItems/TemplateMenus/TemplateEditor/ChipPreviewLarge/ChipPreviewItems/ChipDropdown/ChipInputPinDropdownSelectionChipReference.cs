namespace IAmACube
{
    public class ChipInputPinDropdownSelectionChipReference : ChipInputPinDropdownSelection
    {
        public ChipPreviewLarge ChipReference;
        public override string ToString()
        {
            return ChipReference.OutputName;
        }

        public ChipInputPinDropdownSelectionChipReference(ChipPreviewLarge chipReference)
        {
            ChipReference = chipReference;
        }
    }
}
