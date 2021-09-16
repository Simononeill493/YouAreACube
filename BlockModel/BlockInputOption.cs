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
        public BlocksetModel Blockset;
        public TemplateVariable Variable;
        public object Value;
        public string Message;
        public InputOptionSubmenuType SubMenu;
        public string ToParse;

        public string GetStoredType()
        {
            switch (InputOptionType)
            {
                case InputOptionType.Value:
                    return InGameTypeUtils.RealTypeToInGameType(Value.GetType());
                case InputOptionType.Reference:
                    return Block.GetOutputType();
                case InputOptionType.Variable:
                    return Variable.VariableType.Name;
                case InputOptionType.MetaVariable:
                    return "int";
                case InputOptionType.Chipset:
                    return "int";
                default:
                    return null;
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
                case InputOptionType.MetaVariable:
                    return BlocksetEditPane.VariableProvider.GetVariable((int)Value).VariableName;
                case InputOptionType.Chipset:
                    return Blockset.Name;
                case InputOptionType.Unparseable:
                    return ToParse;
                case InputOptionType.SubMenu:
                    return Message;
                case InputOptionType.Undefined:
                    return "";
            }

            throw new Exception();
        }

        public string GetJsonValue()
        {
            switch (InputOptionType)
            {
                case InputOptionType.Value:
                    if(Value as CubeTemplate != null)
                    {
                        return ((CubeTemplate)Value).ToJsonRep();
                    }
                    return Value.ToString();
                case InputOptionType.Reference:
                    return Block.Name;
                case InputOptionType.Variable:
                    return Variable.VariableNumber.ToString();
                case InputOptionType.MetaVariable:
                    return ((int)Value).ToString();
                case InputOptionType.Chipset:
                    return Blockset.Name + " [" + Blockset.ModeIndex + "]";
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
        public static BlockInputOption CreateMetaVariable(int variableIndex) => new BlockInputOption() { InputOptionType = InputOptionType.MetaVariable, Value = variableIndex };
        public static BlockInputOption CreateChipset(BlocksetModel chipset) => new BlockInputOption() { InputOptionType = InputOptionType.Chipset, Blockset = chipset };

        public static BlockInputOption CreateUnparseable(string stringRep) => new BlockInputOption() { InputOptionType = InputOptionType.Unparseable, ToParse = stringRep };
        public static BlockInputOption CreateSubMenu(string message,InputOptionSubmenuType subMenu) => new BlockInputOption() { InputOptionType = InputOptionType.SubMenu, Message = message, SubMenu = subMenu };
        public static BlockInputOption Undefined => new BlockInputOption() { InputOptionType = InputOptionType.Undefined };
    }

    public enum InputOptionSubmenuType
    {
        TemplateSelect
    }

}
