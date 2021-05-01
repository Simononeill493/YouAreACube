using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public static class ChipDatabase
    {
        public static Dictionary<string,ChipData> BuiltInChips;
        public static Dictionary<string, Type> _assemblyChipTypes;


        public static void Load()
        {
            BuiltInChips = _getBuiltInChipsFromFile();
            _assemblyChipTypes = TypeUtils.GetAssemblyChipTypes();

            foreach(var data in BuiltInChips.Values)
            {
                var instance = GenerateChipFromData(data);
            }
        }

        public static ChipData GetChipDataFromChip(IChip chip)
        {
            var chipName = chip.GetType().Name;
            var chipStringLocation = chipName.IndexOf("Chip");
            var chipNameNoChip = chipName.Substring(0, chipStringLocation);
            return BuiltInChips[chipNameNoChip];
        }

        public static IChip GenerateChipFromData(ChipData data,string typeArgument = "Object")
        {
            if(data.IsGeneric)
            {
                var genericChipType = _assemblyChipTypes.FirstOrDefault(c => c.Value.Name.Equals(data.Name + "Chip`1")).Value;
                var genericRuntimeType = genericChipType.MakeGenericType(TypeUtils.GetTypeByName(typeArgument));
                IChip genericInstance = (IChip)Activator.CreateInstance(genericRuntimeType);

                return genericInstance;
            }
            var chipType = _assemblyChipTypes.FirstOrDefault(c => c.Value.Name.Equals(data.Name + "Chip")).Value;

            IChip instance = (IChip)Activator.CreateInstance(chipType);
            return instance;
        }


        private static Dictionary<string, ChipData> _getBuiltInChipsFromFile()
        {
            var data = FileUtils.LoadJson(ConfigFiles.ChipsFilePath);
            return BuiltInChipParser.ParseChips(data["chips"]);
        }


        public static IEnumerable<ChipData> SearchChips(string searchTerm) => BuiltInChips.Values.Where(c => c.NameLower.Contains(searchTerm.ToLower()));
    }
}
