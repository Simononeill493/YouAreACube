using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockInputOption_2
    {
        public InputOptionType InputOptionType;

        public BlockModel Block;
        public TemplateVariable Variable;
        public object Value;
        public string Message;
        public string ToParse;

        public string GetStoredValue()
        {
            switch (InputOptionType)
            {
                case InputOptionType.Value:
                    return Value.ToString();
                case InputOptionType.Reference:
                    return Block.Name;
                case InputOptionType.Variable:
                    return Variable.VariableNumber.ToString();
                case InputOptionType.Parseable:
                    throw new NotImplementedException();
            }

            throw new Exception();
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
                case InputOptionType.Parseable:
                    return ToParse;
                case InputOptionType.SubMenu:
                    return Message;
                case InputOptionType.Undefined:
                    return "";
            }

            throw new Exception();
        }

        public BlockInputOption_2() { }

        public BlockInputOption_2(InputOptionType optionType,object optionValue)
        {

        }

        public override string ToString() => GetDisplayValue();

        public static BlockInputOption_2 CreateValue(object value) => new BlockInputOption_2() { InputOptionType = InputOptionType.Value, Value = value };
        public static BlockInputOption_2 CreateReference(BlockModel block) => new BlockInputOption_2() { InputOptionType = InputOptionType.Reference, Block = block};
        public static BlockInputOption_2 CreateVariable(TemplateVariable variable) => new BlockInputOption_2() { InputOptionType = InputOptionType.Variable, Variable = variable};
        public static BlockInputOption_2 CreateParseable(string stringRep) => new BlockInputOption_2() { InputOptionType = InputOptionType.Parseable, ToParse = stringRep };
        public static BlockInputOption_2 CreateSubMenu(string message) => new BlockInputOption_2() { InputOptionType = InputOptionType.SubMenu, Message = message };


        public static BlockInputOption_2 Undefined => new BlockInputOption_2() { InputOptionType = InputOptionType.Undefined };
    }
}
