namespace IAmACube
{
    public class ChipInputOptionReference : ChipInputOption
    {
        public ChipTopStandard ChipReference;

        public override string ToString()
        {
            return ChipReference.OutputName;
        }

        public ChipInputOptionReference(ChipTopStandard chipReference) : base(InputOptionType.Reference)
        {
            ChipReference = chipReference;
        }
    }
}
