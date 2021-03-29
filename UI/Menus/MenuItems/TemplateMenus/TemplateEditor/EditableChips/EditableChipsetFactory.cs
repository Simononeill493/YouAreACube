using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class EditableChipsetFactory
    {
        public static EditableChipset Create(ChipEditPane container,List<ChipTop> chips,float scaleMultiplier)
        {
            var chipset = new EditableChipset(container, scaleMultiplier);

            chipset.UpdateDimensions(container.ActualLocation, container.GetCurrentSize());
            chipset.AppendChips(chips, 0);

            //chipset.LiftChipsCallback = liftChipsCallback;
            //chipset.OnEndDrag += (i) => dropChipsCallback(chipset, i);

            return chipset;
        }
    }
}
