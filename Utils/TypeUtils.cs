﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class TypeUtils
    {
        private static Dictionary<string, Type> AllTypes;

        public static bool IsType(string name)
        {
            return AllTypes.ContainsKey(name) | name.Equals("Int32");
        }

        public static Type GetTypeByName(string name)
        {
            if(name.Equals("int"))
            {
                return typeof(int);
            }
            else
            {
                return AllTypes[name];
            }
        }

        public static void Load()
        {
            AllTypes = _getAllTypes();
        }

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

            return null;
        }

        private static Dictionary<string, Type> _getAllTypes()
        {
            var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes());

            var dict = new Dictionary<string, Type>();
            foreach (var type in types)
            {
                dict[type.Name] = type;
            }

            return dict;
        }

        public static Dictionary<string, Type> GetAssemblyChipTypes()
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