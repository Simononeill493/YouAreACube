namespace IAmACube
{
    public class ChipInputOptionReference : ChipInputOption
    {
        public ChipTopSection ChipReference;
        public override string ToString()
        {
            return ChipReference.OutputName;
        }

        public ChipInputOptionReference(ChipTopSection chipReference)
        {
            ChipReference = chipReference;
        }
    }
}
