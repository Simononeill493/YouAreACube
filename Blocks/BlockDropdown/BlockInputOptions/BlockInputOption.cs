namespace IAmACube
{
    public abstract class BlockInputOption 
    {
        public InputOptionType OptionType { get; protected set; }
        public abstract string BaseType { get; }
        public BlockInputOption(InputOptionType optionType) { OptionType = optionType; }
    }

    public enum InputOptionType
    {
        Undefined = 0,
        Value,
        Reference,
        Parseable
    }
}
