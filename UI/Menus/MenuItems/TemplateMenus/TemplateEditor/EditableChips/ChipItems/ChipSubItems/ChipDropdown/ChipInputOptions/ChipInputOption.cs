namespace IAmACube
{
    public abstract class ChipInputOption 
    {
        public InputOptionType OptionType { get; protected set; }
        public ChipInputOption(InputOptionType optionType) { OptionType = optionType; }
    }

    public enum InputOptionType
    {
        Value,
        Reference,
    }
}
