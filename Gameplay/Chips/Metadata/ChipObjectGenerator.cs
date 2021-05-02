using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ChipObjectGenerator
    {
        public static void Test()
        {
            foreach (var data in ChipDatabase.GraphicalChips.Values)
            {
                var instance = GenerateIChipFromChipData(data);
            }
        }

        public static IChip GenerateIChipFromChipData(GraphicalChipData data, string typeArgument = "Object")
        {
            if (data.IsGeneric)
            {
                return _generateGenericChipFromChipData(data, typeArgument);
            }

            return _generateNonGenericChipFromChipData(data);
        }

        private static IChip _generateGenericChipFromChipData(GraphicalChipData data, string typeArgument = "Object")
        {
            var genericChipType = TypeUtils.GetChipTypeByName(data.Name + "Chip`1");
            var genericRuntimeType = genericChipType.MakeGenericType(TypeUtils.GetTypeByName(typeArgument));
            var genericInstance = (IChip)Activator.CreateInstance(genericRuntimeType);

            return genericInstance;
        }

        private static IChip _generateNonGenericChipFromChipData(GraphicalChipData data)
        {
            var chipType = TypeUtils.GetChipTypeByName(data.Name + "Chip");
            var instance = (IChip)Activator.CreateInstance(chipType);

            return instance;
        }
    }
}
