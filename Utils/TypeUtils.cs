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
        private static Dictionary<string, Type> _allTypes;
        private static Dictionary<string, Type> _assemblyChipTypes;

        public static void Load()
        {
            _allTypes = _loadAllTypes();
            _assemblyChipTypes = _loadAssemblyChipTypes();
        }

        public static string GetTypeDisplayName(Type type)
        {
            if (type.Equals(typeof(int)))
            {
                return "int";
            }
            if (type.Equals(typeof(bool)))
            {
                return "bool";
            }
            if (type.Equals(typeof(TemplateVersionDictionary)))
            {
                return "Template";
            }

            return type.Name;
        }
        public static Type GetTypeByDisplayName(string name)
        {
            if (name.Equals("int"))
            {
                return typeof(int);
            }
            if (name.Equals("bool"))
            {
                return typeof(bool);
            }
            if (name.Equals("Template"))
            {
                return typeof(TemplateVersionDictionary);
            }
            if (name.Equals("List<Variable>"))
            {
                return typeof(List<object>);
            }
            if (name.Equals("Variable"))
            {
                return typeof(object);
            }
            if (name.Equals("AnyBlock"))
            {
                return typeof(Block);
            }

            return _allTypes[name];
        }
        public static Type GetChipTypeByName(string name) => _assemblyChipTypes.FirstOrDefault(c => c.Value.Name.Equals(name)).Value;


        public static object ParseType(Type t, string asString)
        {
            if (t.IsEnum)
            {
                var parsed = Enum.Parse(t, asString);
                return parsed;
            }

            var parseMethod = t.GetMethod("Parse", new Type[] { typeof(string) }, new ParameterModifier[] { new ParameterModifier(1) });
            if (parseMethod != null)
            {
                var parsed = parseMethod.Invoke(null, new object[] { asString });
                return parsed;
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
        public static string GetTypeOfStringRepresentation(string stringRepresentation,List<Type> possibleTypes)
        {
            foreach (var type in possibleTypes)
            {
                if (ParseType(type, stringRepresentation) != null)
                {
                    return GetTypeDisplayName(type);
                }
            }

            throw new Exception("Cannot resolve type of string");
        }


        public static bool IsEnum(string typeName) => GetTypeByDisplayName(typeName).IsEnum;
        public static List<object> GetEnumValues(string typeName) => GetTypeByDisplayName(typeName).GetEnumValues().Cast<object>().ToList();



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
            var allIChips = _allTypes.Values.Where(x => typeof(IChip).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).ToList();

            var dict = new Dictionary<string, Type>();
            foreach (var iChip in allIChips)
            {
                dict[iChip.Name] = iChip;
            }

            return dict;
        }
    }

}
