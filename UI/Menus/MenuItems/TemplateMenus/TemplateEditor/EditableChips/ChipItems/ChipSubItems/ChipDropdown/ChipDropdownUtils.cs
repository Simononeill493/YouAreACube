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
        public static List<ChipInputOption> GetDefaultItems(string dataType)
        {
            if (IsDiscreteType(dataType))
            {
                return GetBasicSelections(dataType).Cast<ChipInputOption>().ToList();
            }
            else if (dataType.Equals("Template"))
            {
                var templates = _createOptionsFromItems(Templates.BlockTemplates.Values.ToList());
                return templates.Cast<ChipInputOption>().ToList();
            }
            else
            {
                return new List<ChipInputOption>();
            }
        }

        public static List<ChipInputOptionBase> GetBasicSelections(string dataType)
        {
            if (dataType.Equals(nameof(CardinalDirection)))
            {
                return _createOptionsFromItems(typeof(CardinalDirection).GetEnumValues().Cast<CardinalDirection>().ToList());
            }
            else if (dataType.Equals(nameof(RelativeDirection)))
            {
                return _createOptionsFromItems(typeof(RelativeDirection).GetEnumValues().Cast<RelativeDirection>().ToList());
            }
            else if (dataType.Equals(nameof(BlockMode)))
            {
                return _createOptionsFromItems(typeof(BlockMode).GetEnumValues().Cast<BlockMode>().ToList());
            }
            else if (dataType.Equals("bool"))
            {
                return _createOptionsFromItems(new List<bool> { true, false });
            }

            throw new Exception();
        }
        private static List<ChipInputOptionBase> _createOptionsFromItems<T>(List<T> items)
        {
            var output = new List<ChipInputOptionBase>();
            foreach (var item in items)
            {
                var toAdd = new ChipInputOptionBase() { BaseObject = item };
                output.Add(toAdd);
            }

            return output;
        }

        public static Type GetTypeOfDataType(string dataType)
        {
            if (dataType.Equals(nameof(CardinalDirection))) { return typeof(CardinalDirection); }
            if (dataType.Equals(nameof(RelativeDirection))) { return typeof(RelativeDirection); }
            if (dataType.Equals(nameof(BlockMode))) { return typeof(BlockMode); }
            if (dataType.Equals("bool")) { return typeof(bool); }
            if (dataType.Equals("int")) { return typeof(int); }
            if (dataType.Equals("string")) { return typeof(string); }
            if (dataType.Equals("Template")) { return typeof(BlockTemplate); }
            if (dataType.Equals("Tile")) { return typeof(Tile); }
            if (dataType.Equals("List<Variable>")) { return typeof(List<object>); }
            if (dataType.Equals("Keys")) { return typeof(Keys); }
            if (dataType.Equals(nameof(SurfaceBlock))) { return typeof(SurfaceBlock); }
            if (dataType.Equals(nameof(GroundBlock))) { return typeof(GroundBlock); }
            if (dataType.Equals(nameof(EphemeralBlock))) { return typeof(EphemeralBlock); }

            throw new Exception();
        }
        public static bool IsDiscreteType(string dataType)
        {
            return dataType.Equals(nameof(CardinalDirection)) | dataType.Equals(nameof(RelativeDirection)) | dataType.Equals(nameof(BlockMode)) | dataType.Equals("bool");
        }
        public static bool IsTextEntryType(string dataType)
        {
            return dataType.Equals("int") | dataType.Equals("string");
        }
    }
}
