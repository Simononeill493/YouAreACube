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
            foreach (var data in GraphicalChipDatabase.GraphicalChips.Values)
            {
                var instance = GenerateIChipFromChipData(data);
            }
        }

        public static IChip GenerateIChipFromChipData(GraphicalChipData data, string typeArgument = "Object", List<string> inputMappings = null)
        {
            if(data.IsMappedToSubChips)
            {
                GraphicalChipData mappedData;
                if (inputMappings == null)
                {
                    mappedData = data.InputMappings[0];
                }
                else
                {
                    mappedData = data.InputMappings.First(sub => sub.Inputs.SequenceEqual(inputMappings));
                }

                return GenerateIChipFromChipData(mappedData, typeArgument);
            }

            if (data.IsGeneric)
            {
                return _generateGenericChipFromChipData(data, typeArgument, inputMappings);
            }

            return _generateNonGenericChipFromChipData(data, inputMappings);
        }

        private static IChip _generateGenericChipFromChipData(GraphicalChipData data, string typeArgument, List<string> inputMappings)
        {
            var genericChipType = TypeUtils.GetChipTypeByName(data.Name + "Chip`1");
            var genericRuntimeType = genericChipType.MakeGenericType(TypeUtils.GetTypeByName(typeArgument));
            var genericInstance = (IChip)Activator.CreateInstance(genericRuntimeType);

            return genericInstance;
        }

        private static IChip _generateNonGenericChipFromChipData(GraphicalChipData data, List<string> inputMappings)
        {
            var chipType = TypeUtils.GetChipTypeByName(data.Name + "Chip");
            var instance = (IChip)Activator.CreateInstance(chipType);

            return instance;
        }
    }
}
