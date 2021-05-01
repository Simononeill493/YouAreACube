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
            foreach (var data in ChipDatabase.BuiltInChips.Values)
            {
                var instance = GenerateIChipFromChipData(data);
            }
        }

        public static IChip GenerateIChipFromChipData(ChipData data, string typeArgument = "Object")
        {
            if (data.IsGeneric)
            {
                return _generateGenericChipFromChipData(data, typeArgument);
            }

            return _generateNonGenericChipFromChipData(data);
        }

        private static IChip _generateGenericChipFromChipData(ChipData data, string typeArgument = "Object")
        {
            var genericChipType = TypeUtils.GetAssemblyChipType(data.Name + "Chip`1");
            var genericRuntimeType = genericChipType.MakeGenericType(TypeUtils.GetTypeByName(typeArgument));
            var genericInstance = (IChip)Activator.CreateInstance(genericRuntimeType);

            return genericInstance;
        }

        private static IChip _generateNonGenericChipFromChipData(ChipData data)
        {
            var chipType = TypeUtils.GetAssemblyChipType(data.Name + "Chip");
            var instance = (IChip)Activator.CreateInstance(chipType);

            return instance;
        }
    }
}
