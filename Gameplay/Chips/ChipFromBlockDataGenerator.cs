using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    static class ChipFromBlockDataGenerator
    {
        public static IChip GenerateChip(this BlockData data, List<string> typeArguments = null, List<string> inputMappings = null)
        {
            if(typeArguments== null)
            {
                typeArguments = data.DefaultTypeArguments;
            }

            if(data.IsMappedToSubBlocks)
            {
                BlockData mappedData;
                if (inputMappings == null)
                {
                    mappedData = data.InputMappings[0];
                }
                else
                {
                    mappedData = data.InputMappings.First(sub => sub.Inputs.SequenceEqual(inputMappings));
                }

                return mappedData.GenerateChip(typeArguments);
            }

            if (data.IsGeneric)
            {
                return _generateGenericChipFromBlockData(data, typeArguments);
            }

            return _generateNonGenericChipFromBlockData(data);
        }

        private static IChip _generateGenericChipFromBlockData(BlockData data, List<string> typeArguments)
        {
            var genericChipType = TypeUtils.GetChipTypeByName(data.Name + "Chip`1");
            var typeArgumentsAsType = typeArguments.Select(ta => TypeUtils.GetTypeByDisplayName(ta)).ToArray();
            var genericRuntimeType = genericChipType.MakeGenericType(typeArgumentsAsType);
            var genericInstance = (IChip)Activator.CreateInstance(genericRuntimeType);

            return genericInstance;
        }

        private static IChip _generateNonGenericChipFromBlockData(BlockData data)
        {
            var chipType = TypeUtils.GetChipTypeByName(data.Name + "Chip");
            var instance = (IChip)Activator.CreateInstance(chipType);

            return instance;
        }
    }
}
