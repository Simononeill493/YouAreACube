using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ChipDataDropdown : DropdownMenuItem<ChipInputPinDropdownSelection>
    {
        public Type BaseType;
        public ChipDataDropdown(IHasDrawLayer parent) : base(parent) {}
    }

    public abstract class ChipInputPinDropdownSelection {}

    public class ChipInputPinDropdownSelectionBase : ChipInputPinDropdownSelection
    {
        public object BaseObject;
        public override string ToString()
        {
            return BaseObject.ToString();
        }

        public static List<ChipInputPinDropdownSelectionBase> GetBasicSelections(string dataType)
        {
            if (dataType.Equals("CardinalDirection"))
            {
                return Create(typeof(CardinalDirection).GetEnumValues().Cast<CardinalDirection>().ToList());
            }
            else if (dataType.Equals("RelativeDirection"))
            {
                return Create(typeof(RelativeDirection).GetEnumValues().Cast<RelativeDirection>().ToList());
            }
            else if (dataType.Equals("BlockType"))
            {
                return Create(typeof(BlockType).GetEnumValues().Cast<BlockType>().ToList());
            }
            else if (dataType.Equals("bool"))
            {
                return Create(new List<bool> {true,false});
            }

            throw new Exception();
        }
        public static List<ChipInputPinDropdownSelectionBase> Create<T>(List<T> items)
        {
            var output = new List<ChipInputPinDropdownSelectionBase>();
            foreach(var item in items)
            {
                var toAdd = new ChipInputPinDropdownSelectionBase() { BaseObject = item };
                output.Add(toAdd);
            }

            return output;
        }
    }
}
