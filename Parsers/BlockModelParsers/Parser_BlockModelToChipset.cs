using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ChipsDict = System.Collections.Generic.Dictionary<System.String, IAmACube.IChip>;
using ChipsetsDict = System.Collections.Generic.Dictionary<System.String, IAmACube.Chipset>;


namespace IAmACube
{ 
    static class Parser_BlockModelToChipset
    {
        public static Chipset ToChipset(this FullModel fullmodel)
        {
            var chips = new ChipsDict();
            var chipsets = new ChipsetsDict();

            foreach(var block in fullmodel.Blocks.Values)
            {
                var chip = block.GetChipBlockData().GenerateChip(typeArguments: block.GetTypeArguments());
                chip.Name = block.Name;

                chips[chip.Name] = chip;
            }

            foreach (var blockset in fullmodel.Blocksets.Values)
            {
                var chipset = new Chipset(blockset.Name);
                chipset.Name = blockset.Name;
                chipset.AddChips(blockset.Blocks.Select(b => chips[b.Name]).ToList());

                chipsets[chipset.Name] = chipset;
            }

            foreach(var block in fullmodel.Blocks.Values)
            {
                var chip = chips[block.Name];

                if (block.SubBlocksets.Any())
                {
                    var controlChip = (IControlChip)chip;
                    controlChip.SetSubChipsets(block.SubBlocksets.Select(s => (s.Item1, chipsets[s.Item2.Name])).ToList());
                }

                for(int i=0;i<block.Inputs.Count;i++)
                {
                    chip.SetInputProperty(block.Inputs[i].InputOption, i, chips);
                }
            }

            var initial = chipsets.Values.Where(c => c.Name == "_Initial");
            if(initial.Count()>1) 
            {
                throw new Exception("Multiple initials?");
            }
            if (initial.Count() == 0)
            {
                throw new Exception("No initials");
            }
            return initial.First();
        }

        public static void SetInputProperty(this IChip chip,BlockInputOption inputOption,int pin,ChipsDict chipsDict)
        {
            switch (inputOption.InputOptionType)
            {
                case InputOptionType.Value:
                    chip.SetValueProperty(inputOption.Value, pin);
                    break;
                case InputOptionType.Reference:
                    chip.SetReferenceProperty(chipsDict[inputOption.Block.Name], pin);
                    break;
                case InputOptionType.Variable:
                    chip.SetVariableProperty(inputOption.Variable.VariableNumber, pin);
                    break;
                case InputOptionType.MetaVariable:
                    chip.SetValueProperty(inputOption.Value, pin);
                    break;
                default:
                    throw new Exception();
            }
        }
    }
}