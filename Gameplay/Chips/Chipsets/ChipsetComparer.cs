using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    static class ChipsetComparer
    {
        public static bool Equivalent(this Chipset b1, Chipset b2)
        {
            if (!b1.Name.Equals(b2.Name)) { return false; }
            if (b1.Chips.Count != b2.Chips.Count) { return false; }
            if (b1.ControlChips.Count != b2.ControlChips.Count) { return false; }

            for (int i = 0; i < b1.Chips.Count; i++)
            {
                var chip1 = b1.Chips[i];
                var chip2 = b2.Chips[i];
                if (!chip1.Equivalent(chip2))
                {
                    return false;
                }
            }

            return true;
        }

        public static bool Equivalent(this IChip chip1,IChip chip2)
        {
            if (!chip1.Name.Equals(chip2.Name))
            {
                return false;
            }

            if(chip1.GetType()!=chip2.GetType())
            {
                return false;
            }

            if (!chip1.GetBlockData().Name.Equals(chip2.GetBlockData().Name))
            {
                return false;
            }


            var inputs1 = chip1.GetInputPinValues();
            var inputs2 = chip2.GetInputPinValues();

            if(!inputs1.Equivalent(inputs2))
            {
                return false;
            }

            if (chip1 is IControlChip)
            {
                var chipsets1 = ((IControlChip)chip1).GetSubChipsets();
                var chipsets2 = ((IControlChip)chip2).GetSubChipsets();

                if (chipsets1.Count() != chipsets2.Count()) { return false; }

                for (int i = 0; i < chipsets1.Count(); i++)
                {
                    if(!chipsets1[i].Item1.Equals(chipsets2[i].Item1))
                    {
                        return false;
                    }
                    if (!(chipsets1[i].Item2).Equivalent(chipsets2[i].Item2))
                    {
                        return false;
                    }
                }

            }

            return true;

        }

        public static bool Equivalent(this ChipInputValues inputs1,ChipInputValues inputs2)
        {
            if(inputs1.List.Count() != inputs2.List.Count())
            {
                return false;
            }

            for(int i=0;i<inputs1.List.Count;i++)
            {
                var input1 = inputs1.List[i];
                var input2 = inputs2.List[i];

                if(input1.OptionType!=input2.OptionType)
                {
                    return false;
                }

                if(input1.OptionValue.ToString()!=input2.OptionValue.ToString())
                {
                    return false;
                }
            }

            return true;
        }
    }
}
