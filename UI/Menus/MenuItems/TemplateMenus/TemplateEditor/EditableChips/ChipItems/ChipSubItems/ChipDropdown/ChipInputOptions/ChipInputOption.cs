namespace IAmACube
{
    public abstract class ChipInputOption 
    {
        public InputOptionType OptionType;
        public ChipInputOption(InputOptionType optionType) { OptionType = optionType; }
    }

    public enum InputOptionType
    {
        Base,
        Reference,
        Generic
    }
}
