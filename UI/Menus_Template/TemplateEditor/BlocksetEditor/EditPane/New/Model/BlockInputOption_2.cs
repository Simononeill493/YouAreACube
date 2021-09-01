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
                case InputOptionType.SubMenu:
                    return Message;
            }

            throw new Exception();
        }

        public override string ToString() => GetDisplayValue();

        public static BlockInputOption_2 CreateValue(object value) => new BlockInputOption_2() { InputOptionType = InputOptionType.Value, Value = value };
        public static BlockInputOption_2 CreateReference(BlockModel block) => new BlockInputOption_2() { InputOptionType = InputOptionType.Reference, Block = block};
        public static BlockInputOption_2 CreateSubMenu(string message) => new BlockInputOption_2() { InputOptionType = InputOptionType.SubMenu, Message = message };
    }
}
