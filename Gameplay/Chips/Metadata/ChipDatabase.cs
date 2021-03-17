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
            _assemblyChipTypes = _getAssemblyChipTypes();

            foreach(var data in BuiltInChips.Values)
            {
                var instance = GenerateChipFromData(data);
            }
        }

        public static IChip GenerateChipFromData(ChipData data)
        {
            if(data.IsGeneric)
            {
                var genericChipType = _assemblyChipTypes.FirstOrDefault(c => c.Value.Name.Equals(data.Name + "Chip`1")).Value;
                var genericRuntimeType = genericChipType.MakeGenericType(typeof(object));
                IChip genericInstance = (IChip)Activator.CreateInstance(genericRuntimeType);

                return genericInstance;
            }
            var chipType = _assemblyChipTypes.FirstOrDefault(c => c.Value.Name.Equals(data.Name + "Chip")).Value;

            IChip instance = (IChip)Activator.CreateInstance(chipType);
            return instance;
        }


        private static Dictionary<string, ChipData> _getBuiltInChipsFromFile()
        {
            var data = FileUtils.LoadJson(Config.ChipsFile);
            return ChipParser.ParseChips(data["chips"]);
        }

        private static Dictionary<string, Type> _getAssemblyChipTypes()
        {
            var allIChips = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
                 .Where(x => typeof(IChip).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).ToList();

            var dict = new Dictionary<string, Type>();
            foreach(var iChip in allIChips)
            {
                dict[iChip.Name] = iChip;
            }

            return dict;
        }

        public static IEnumerable<ChipData> SearchChips(string searchTerm) => BuiltInChips.Values.Where(c => c.NameLower.Contains(searchTerm.ToLower()));
    }
}
