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
        public static Type GetTypeByName(string name)
        {
            if (name.Equals("int"))
            {
                return typeof(int);
            }
            if (name.Equals("bool"))
            {
                return typeof(bool);
            }
            else
            {
                return _allTypes[name];
            }
        }
        public static Type GetChipTypeByName(string name) => _assemblyChipTypes.FirstOrDefault(c => c.Value.Name.Equals(name)).Value;

        public static bool IsType(string name) => _allTypes.ContainsKey(name) | name.Equals("Int32");
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



        public static bool IsValidInputFor(string top,string bottom)
        {
            if (top.Equals(bottom))
            {
                return true;
            }
            else if (bottom.Equals("AnyBlock") & (top.Equals("Block") | top.Equals("SurfaceBlock") | top.Equals("GroundBlock") | top.Equals("EphemeralBlock")))
            {
                return true;
            }
            else if (bottom.Equals("Variable"))
            {
                return true;
            }
            else if (top.StartsWith("List<") & bottom.Equals("List<Variable>"))
            {
                return true;
            }

            return false;
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
