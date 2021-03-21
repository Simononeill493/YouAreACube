namespace IAmACube
{
    public class ChipInputOptionReference : ChipInputOption
    {
        public ChipItem ChipReference;
        public override string ToString()
        {
            return ChipReference.OutputName;
        }

        public ChipInputOptionReference(ChipItem chipReference)
        {
            ChipReference = chipReference;
        }
    }
}
