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
                var cubeTemplateSelect = new BlockInputOptionSubMenu("Select Template...",InputOptionMenu.CubeTemplate);

                return new List<BlockInputOption>() { cubeTemplateSelect };
                //var templates = _createOptionsFromItems(Templates.Database.Values.SelectMany(v=>v.Versions).ToList());
                //return templates.Cast<BlockInputOption>().ToList();
            }
            else
            {
                return new List<BlockInputOption>();
            }
        }


        private static List<BlockInputOptionValue> _getBasicSelections(string dataType)
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
        private static List<BlockInputOptionValue> _createOptionsFromItems<T>(List<T> items) => items.Select(item => new BlockInputOptionValue(item)).ToList();
        private static List<BlockInputOptionValue> _createOptionsFromItems(List<object> items) => items.Select(item => new BlockInputOptionValue(item)).ToList();


        public static bool IsDiscreteType(string dataType) => TypeUtils.IsEnum(dataType) | dataType.Equals("bool");
        public static bool IsTextEntryType(string dataType) => dataType.Equals("int") | dataType.Equals("string") | dataType.Equals("Keys");
    }
}
