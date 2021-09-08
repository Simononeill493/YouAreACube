using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockDropdownUtils
    {
        public static List<BlockInputOption> GetDefaultItems(List<string> dataTypes) => dataTypes.SelectMany(dataType => GetDefaultItems(dataType)).ToList();
        public static List<BlockInputOption> GetDefaultItems(string dataType)
        {
            if (dataType.Contains('|'))
            {
                return GetDefaultItems(dataType.Split('|').ToList());
            }
            if (IsDiscreteType(dataType))
            {
                return _getBasicSelections(dataType).Cast<BlockInputOption>().ToList();
            }
            else if (dataType.Equals(nameof(CubeTemplate)))
            {
                return new List<BlockInputOption>() { BlockInputOption.CreateSubMenu("Select Template...") };
            }
            else
            {
                return new List<BlockInputOption>();
            }
        }

        private static List<BlockInputOption> _getBasicSelections(string dataType)
        {
            if (TypeUtils.IsEnum(dataType))
            {
                return _createOptionsFromItems(TypeUtils.GetEnumValues(dataType));
            }
            else if (dataType.Equals("bool"))
            {
                return _createOptionsFromItems(new List<bool> { true, false }.ToList());
            }

            throw new Exception();
        }
        private static List<BlockInputOption> _createOptionsFromItems<T>(List<T> items) => items.Select(item => BlockInputOption.CreateValue(item)).ToList();
        private static List<BlockInputOption> _createOptionsFromItems(List<object> items) => items.Select(item => BlockInputOption.CreateValue(item)).ToList();


        public static bool IsDiscreteType(string dataType) => TypeUtils.IsEnum(dataType) | dataType.Equals("bool");
        public static bool IsTextEntryType(string dataType) => dataType.Equals("int") | dataType.Equals("string") | dataType.Equals("Keys");
    }
}
