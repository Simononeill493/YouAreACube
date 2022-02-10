using Microsoft.Xna.Framework.Input;
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

            if (dataType.Equals(nameof(CubeTemplate)))
            {
                return new List<BlockInputOption>() { BlockInputOption.CreateSubMenu("Select Template...", InputOptionSubmenuType.TemplateSelect) };
            }
            else if (InGameTypeUtils.IsDiscreteType(dataType))
            {
                var output = new List<BlockInputOption>();
                if (dataType.Equals(nameof(Keys)))
                {
                    output.Add(BlockInputOption.CreateSubMenu("Enter key...", InputOptionSubmenuType.KeyEntry));
                }

                output.AddRange(_getBasicSelections(dataType).Cast<BlockInputOption>().ToList());

                return output;
            }
            else
            {
                return new List<BlockInputOption>();
            }
        }

        private static List<BlockInputOption> _getBasicSelections(string dataType)
        {
            if (InGameTypeUtils.IsEnum(dataType))
            {
                return _createOptionsFromItems(InGameTypeUtils.GetEnumValues(dataType));
            }
            else if (dataType.Equals("bool"))
            {
                return _createOptionsFromItems(new List<bool> { true, false }.ToList());
            }

            throw new Exception();
        }
        private static List<BlockInputOption> _createOptionsFromItems<T>(List<T> items) => items.Select(item => BlockInputOption.CreateValue(item)).ToList();
        private static List<BlockInputOption> _createOptionsFromItems(List<object> items) => items.Select(item => BlockInputOption.CreateValue(item)).ToList();


    }
}
