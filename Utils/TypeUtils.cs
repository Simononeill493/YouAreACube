using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TypeUtils
    {
        public static Dictionary<string, Type> AllTypes;
        public static Dictionary<string, Type> _assemblyChipTypes;

        public static void Load()
        {
            AllTypes = _loadAllTypes();
            _assemblyChipTypes = _loadAssemblyChipTypes();
        }



        public static Type GetChipTypeByName(string name) => _assemblyChipTypes.FirstOrDefault(c => c.Value.Name.Equals(name)).Value;

        public static T ParseType<T>(string asString) => (T)ParseType(typeof(T), asString);

        public static object ParseType(IEnumerable<Type> types, string asString)
        {
            foreach (var t in types)
            {
                var result = ParseType(t, asString);
                if (result != null)
                {
                    return result;
                }
            }

            return null;
        }
        public static object TryParseType(IEnumerable<Type> types, string asString)
        {
            var result = ParseType(types, asString);
            if (result == null)
            {
                throw new Exception();
            }

            return result;
        }
        public static object ParseType(Type t, string asString)
        {
            if (t.IsEnum)
            {
                if(Enum.IsDefined(t, asString))
                {
                    var parsed = Enum.Parse(t, asString);
                    return parsed;
                }

                return null;
            }

            var tryParseMethod = t.GetMethod("TryParse", new Type[] { typeof(string), t.MakeByRefType() }, new ParameterModifier[] { new ParameterModifier(2) });
            if (tryParseMethod != null)
            {
                var parameters = new object[] { asString, null };
                bool parsed = (bool)tryParseMethod.Invoke(null, parameters);
                if(parsed)
                {
                    return parameters[1];
                }
            }

            if (t == typeof(string))
            {
                return asString;
            }

            if (t == typeof(Microsoft.Xna.Framework.Color))
            {
                var colorObject = t.GetProperty(asString);
                return colorObject.GetValue(null);
            }

            return null;
        }






        private static Dictionary<string, Type> _loadAllTypes()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes());

            var dict = new Dictionary<string, Type>();
            foreach (var type in types)
            {
                dict[type.Name] = type;
            }

            return dict;
        }
        private static Dictionary<string, Type> _loadAssemblyChipTypes()
        {
            var allIChips = AllTypes.Values.Where(x => typeof(IChip).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).ToList();

            var dict = new Dictionary<string, Type>();
            foreach (var iChip in allIChips)
            {
                dict[iChip.Name] = iChip;
            }

            return dict;
        }
    }

}
