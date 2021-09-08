using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace IAmACube
{
    public interface IChip
    {
        string Name { get; set; }
        void Run(Cube actor,UserInput userInput,ActionsList actions);
    }

    public class ChipInputValue : Tuple<InputOptionType, object>
    {
        public InputOptionType OptionType => Item1;
        public object OptionValue => Item2;

        public ChipInputValue(InputOptionType item1, object item2) : base(item1, item2) { }
    }

    public class ChipInputValues
    {
        public List<ChipInputValue> List => _list;
        List<ChipInputValue> _list = new List<ChipInputValue>();

        public void Add(InputOptionType optionType,object optionValue)
        {
            _list.Add(new ChipInputValue(optionType, optionValue));
        }

    }

    public static class IChipUtils
    {
        public static BlockData GetBlockData(this IChip chip)
        {
            var chipTypeFullName = chip.GetType().Name;
            var chipTypeName = chipTypeFullName.Substring(0, chipTypeFullName.IndexOf("Chip"));

            return BlockDataDatabase.BlockDataDict[chipTypeName];
        }

        public static bool IsControlChip(this IChip chip) => typeof(IControlChip).IsAssignableFrom(chip.GetType());

        public static ChipInputValues GetInputPinValues(this IChip chip)
        {
            var chipData = chip.GetBlockData();
            var output = new ChipInputValues();
            var chipType = chip.GetType();
            var properties = chipType.GetProperties();

            for (int i = 1; i < chipData.NumInputs+1; i++)
            {
                var inputTypeProperty = properties.Where(p => p.Name.Equals("InputType" + i)).FirstOrDefault();
                InputOptionType inputType = (InputOptionType)inputTypeProperty.GetValue(chip);
                object inputValue = null;

                switch (inputType)
                {
                    case InputOptionType.Value:
                        var valueProperty = properties.Where(p => p.Name.Equals("InputValue" + i)).FirstOrDefault();
                        inputValue = valueProperty.GetValue(chip);
                        break;
                    case InputOptionType.Reference:
                        var referenceProperty = properties.Where(p => p.Name.Equals("InputReference" + i)).FirstOrDefault();
                        inputValue = referenceProperty.GetValue(chip);
                        break;
                    case InputOptionType.Variable:
                        var variableProperty = properties.Where(p => p.Name.Equals("InputVariable" + i)).FirstOrDefault();
                        inputValue = variableProperty.GetValue(chip);
                        break;

                    default:
                        throw new Exception();
                }

                output.Add(inputType, inputValue);
            }

            return output;
        }

        public static void SetReferenceProperty(this IChip chip,IChip referencedChip,int pin)
        {
            var referenceProperty = chip.GetType().GetProperties().Where(p => p.Name.Equals("InputReference" + (pin+1))).FirstOrDefault();
            referenceProperty.SetValue(chip, referencedChip);
        }

        public static void SetValueProperty(this IChip chip, object value, int pin)
        {
            var valueProperty = chip.GetType().GetProperties().Where(p => p.Name.Equals("InputValue" + (pin+1))).FirstOrDefault();
            valueProperty.SetValue(chip, value);
        }

        public static void SetVariableProperty(this IChip chip, int variableIndex, int pin)
        {
            var variableProperty = chip.GetType().GetProperties().Where(p => p.Name.Equals("InputVariable" + (pin + 1))).FirstOrDefault();
            variableProperty.SetValue(chip, variableIndex);
        }


        public static List<string> GetTypeArgumentNames(this IChip chip)
        {
            var genericArguments = chip.GetType().GenericTypeArguments;
            var names =  genericArguments.Select(ta => TypeUtils.GetTypeDisplayName(ta)).ToList();

            return names;
        }
    }
}