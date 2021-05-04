﻿namespace IAmACube
{
    public interface IChip
    {
        string Name { get; set; }
        void Run(Block actor,UserInput userInput,ActionsList actions);
    }

    public static class IChipUtils
    {
        public static GraphicalChipData GetChipData(this IChip chip)
        {
            var chipTypeName = chip.GetType().Name;
            var chipName = chipTypeName.Substring(0, chipTypeName.IndexOf("Chip"));

            return GraphicalChipDatabase.GraphicalChips[chipName];
        }

        public static bool IsControlChip(this IChip chip) => typeof(IControlChip).IsAssignableFrom(chip.GetType());
    }
}