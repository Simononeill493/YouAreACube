using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ChipDropdownUtils
    {
        public static List<ChipInputOption> GetDefaultItems(List<string> dataTypes) => dataTypes.SelectMany(dataType => GetDefaultItems(dataType)).ToList();
        public static List<ChipInputOption> GetDefaultItems(string dataType)
        {
            if (dataType.Contains('|'))
            {
                return GetDefaultItems(dataType.Split('|').ToList());
            }
            if (IsDiscreteType(dataType))
            {
                return _getBasicSelections(dataType).Cast<ChipInputOption>().ToList();
            }
            else if (dataType.Equals(nameof(BlockTemplate)))
            {
                var templates = _createOptionsFromItems(Templates.Database.Values.SelectMany(v=>v.Versions).ToList());
                return templates.Cast<ChipInputOption>().ToList();
            }
            else
            {
                return new List<ChipInputOption>();
            }
        }


        private static List<ChipInputOptionValue> _getBasicSelections(string dataType)
        {
            if(TypeUtils.IsEnum(dataType))
            {
                return _createOptionsFromItems(TypeUtils.GetEnumValues(dataType));
            }
            else if (dataType.Equals("bool"))
            {
                return _createOptionsFromItems(new List<bool> { true, false }.ToList());
            }

            throw new Exception();
        }
        private static List<ChipInputOptionValue> _createOptionsFromItems<T>(List<T> items) => items.Select(item => new ChipInputOptionValue(item)).ToList();
        private static List<ChipInputOptionValue> _createOptionsFromItems(List<object> items) => items.Select(item => new ChipInputOptionValue(item)).ToList();


        public static bool IsDiscreteType(string dataType) => TypeUtils.IsEnum(dataType) | dataType.Equals("bool");
        public static bool IsTextEntryType(string dataType) => dataType.Equals("int") | dataType.Equals("string") | dataType.Equals("Keys");
    }
}
