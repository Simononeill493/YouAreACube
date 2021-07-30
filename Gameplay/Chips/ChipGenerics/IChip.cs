using System.Collections;
using System.Reflection;

namespace IAmACube
{
    public interface IChip
    {
        string Name { get; set; }
        void Run(Cube actor,UserInput userInput,ActionsList actions);
    }

    public static class IChipUtils
    {
        public static BlockData GetBlockData(this IChip chip)
        {
            var chipTypeName = chip.GetType().Name;
            var chipName = chipTypeName.Substring(0, chipTypeName.IndexOf("Chip"));

            return BlockDataDatabase.BlockDataDict[chipName];
        }

        public static bool IsControlChip(this IChip chip) => typeof(IControlChip).IsAssignableFrom(chip.GetType());

        public static IList GetTargetsList(this IChip chip, int targetIndex)
        {
            var inputChipType = chip.GetType();
            var targetField = inputChipType.GetField("Targets" + (targetIndex + 1).ToString());
            return (IList)targetField.GetValue(chip);
        }
        public static void AddTarget(this IChip chip, int targetIndex,object toAdd)
        {
            var targetsList = chip.GetTargetsList(targetIndex);
            targetsList.Add(toAdd);
        }


        public static PropertyInfo GetInputProperty(this IChip chip, int inputIndex)
        {
            return chip.GetType().GetProperty("ChipInput" + (inputIndex + 1).ToString());
        }
        public static object GetInputPropertyValue(this IChip chip, int inputIndex)
        {
            return GetInputProperty(chip,inputIndex).GetValue(chip);
        }
        public static string GetInputPinValue(this IChip chip, int pinIndex)
        {
            var value = chip.GetInputPropertyValue(pinIndex);
            if (value.GetType() == typeof(CubeTemplate))
            {
                var template = (CubeTemplate)value;
                return template.Versions.Name + '|' + template.Version;
            }
            return value.ToString();
        }

        public static void SetInputProperty(this IChip chip, int inputIndex,object toSet)
        {
            var property =  chip.GetType().GetProperty("ChipInput" + (inputIndex + 1).ToString());
            property.SetValue(chip,toSet);
        }

    }
}