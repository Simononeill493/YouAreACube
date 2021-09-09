using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockInputOption
    {
        public InputOptionType InputOptionType;
       
        public BlockModel Block;
        public TemplateVariable Variable;
        public object Value;
        public string Message;
        public string ToParse;

        public string GetStoredType()
        {
            switch (InputOptionType)
            {
                case InputOptionType.Value:
                    return TypeUtils.GetTypeDisplayName(Value.GetType());
                case InputOptionType.Reference:
                    return Block.GetCurrentOutputType();
                case InputOptionType.Variable:
                    return Variable.VariableType.Name;
                default:
                    throw new NotImplementedException();
            }
        }

        public string GetDisplayValue()
        {
            switch (InputOptionType)
            {
                case InputOptionType.Value:
                    return Value.ToString();
                case InputOptionType.Reference:
                    return Block.Name;
                case InputOptionType.Variable:
                    return Variable.VariableName;
                case InputOptionType.Unparseable:
                    return ToParse;
                case InputOptionType.SubMenu:
                    return Message;
                case InputOptionType.Undefined:
                    return "";
            }

            throw new Exception();
        }

        public BlockInputOption() { }

        public override string ToString() => GetDisplayValue();

        public static BlockInputOption CreateValue(object value) => new BlockInputOption() { InputOptionType = InputOptionType.Value, Value = value };
        public static BlockInputOption CreateReference(BlockModel block) => new BlockInputOption() { InputOptionType = InputOptionType.Reference, Block = block};
        public static BlockInputOption CreateVariable(TemplateVariable variable) => new BlockInputOption() { InputOptionType = InputOptionType.Variable, Variable = variable};
        public static BlockInputOption CreateUnparseable(string stringRep) => new BlockInputOption() { InputOptionType = InputOptionType.Unparseable, ToParse = stringRep };
        public static BlockInputOption CreateSubMenu(string message) => new BlockInputOption() { InputOptionType = InputOptionType.SubMenu, Message = message };
        public static BlockInputOption Undefined => new BlockInputOption() { InputOptionType = InputOptionType.Undefined };
    }
}
