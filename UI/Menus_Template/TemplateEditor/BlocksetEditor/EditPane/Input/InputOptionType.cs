namespace IAmACube
{
    /*public abstract class BlockInputOption 
    {
        public InputOptionType OptionType { get; protected set; }
        public abstract string BaseType { get; }
        public BlockInputOption(InputOptionType optionType) { OptionType = optionType; }

        public virtual void OptionClicked() { }

        public abstract string ToJSONRep();
    }*/

    public enum InputOptionType
    {
        Undefined = 0,
        Value,
        Reference,
        Variable,
        MetaVariable,
        Chipset,
        SubMenu,
        Unparseable
    }
}
