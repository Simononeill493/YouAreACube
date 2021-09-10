using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class InGameTypeUtils
    {
        public const string Variable = "Variable";
        public const string VariableList = "List<Variable>";
        public const string ListStart = "List<";

        public const string AnyCube = "AnyCube";

        public static Dictionary<string,InGameType> InGameTypes;
        public static void Init()
        {
            InGameTypes = new Dictionary<string, InGameType>();
            _addInGameType<CardinalDirection>(nameof(CardinalDirection));
            _addInGameType<RelativeDirection>(nameof(RelativeDirection));
            _addInGameType<CubeTemplate>(nameof(CubeTemplate));
            _addInGameType<CubeMode>(nameof(CubeMode));
            _addInGameType<Tile>(nameof(Tile));
            _addInGameType<int>("int");
            _addInGameType<bool>("bool");
            _addInGameType<string>("string");
            _addInGameType<Keys>(nameof(Keys));
            _addInGameType<Cube>(AnyCube);
            _addInGameType<object>(Variable);
            _addInGameType<List<object>>(VariableList);
        }

        private static void _addInGameType<TType>(string name)
        {
            InGameTypes[name] = new InGameType(name,default(TType));
        }

        public static bool IsValidInputFor(string top, List<string> bottom) => bottom.Any(b => IsValidInputFor(top, b));
        public static bool IsValidInputFor(string top, string bottom)
        {
            if (top == null)
            {
                return false;
            }
            if (bottom.Equals(top) | bottom.Equals(Variable))
            {
                return true;
            }
            else if (bottom.Equals(AnyCube) & top.Equals(nameof(Cube)) | top.Equals(nameof(SurfaceCube)) | top.Equals(nameof(GroundCube)) | top.Equals(nameof(EphemeralCube)))
            {
                return true;
            }
            else if (bottom.Equals(VariableList) & top.StartsWith(ListStart))
            {
                return true;
            }
            else if (bottom.Equals(nameof(CubeTemplate)) & top.Equals(nameof(CubeTemplateMainPlaceholder)))
            {
                return true;
            }

            return false;
        }

        public static string GetTypeArgument(string inputType, string selectedType)
        {
            if (inputType.Equals(Variable))
            {
                return selectedType;
            }
            if (inputType.Equals(AnyCube) & (selectedType.Equals(AnyCube) | selectedType.Equals(nameof(Cube)) | selectedType.Equals(nameof(SurfaceCube)) | selectedType.Equals(nameof(GroundCube)) | selectedType.Equals(nameof(EphemeralCube))))
            {
                return selectedType;
            }
            if (inputType.StartsWith(ListStart) & selectedType.StartsWith(ListStart))
            {
                var selectedSnipped = selectedType.Substring(5, selectedType.Length - 6);
                return selectedSnipped;
            }

            throw new Exception("Block has a generic input type that hasn't been handled.");
        }


        public static object ParseType(List<string> inGameTypes, string asString) => TypeUtils.ParseType(inGameTypes.Select(t => InGameTypeToRealType(t)), asString);
        public static bool IsEnum(string typeName) => InGameTypeToRealType(typeName).IsEnum;
        public static List<object> GetEnumValues(string typeName) => InGameTypeToRealType(typeName).GetEnumValues().Cast<object>().ToList();

        public static bool IsDiscreteType(string dataType) => IsEnum(dataType) | dataType.Equals("bool");
        public static bool IsTextEntryType(string dataType) => dataType.Equals("int") | dataType.Equals("string") | dataType.Equals("Keys");


        public static bool IsGeneric(string typeName)
        {
            if (typeName.Contains(Variable))
            {
                return true;
            }
            if (typeName.Contains(AnyCube))
            {
                return true;
            }

            return false;
        }

        public static string GetDefaultTypeArgument(string typeName)
        {
            if (typeName.Contains(Variable))
            {
                return "Object";
            }
            if (typeName.Contains(AnyCube))
            {
                return nameof(SurfaceCube);
            }

            return null;
        }

        public static string RealTypeToInGameType(Type type)
        {
            if (type.Equals(typeof(int)))
            {
                return "int";
            }
            if (type.Equals(typeof(bool)))
            {
                return "bool";
            }

            return type.Name;
        }
        public static Type InGameTypeToRealType(string name)
        {
            if (name.Equals("int"))
            {
                return typeof(int);
            }
            if (name.Equals("bool"))
            {
                return typeof(bool);
            }
            if (name.Equals(VariableList))
            {
                return typeof(List<object>);
            }
            if (name.Equals(Variable))
            {
                return typeof(object);
            }
            if (name.Equals(AnyCube))
            {
                return typeof(Cube);
            }
            if (name.Equals("object"))
            {
                return typeof(object);
            }

            return TypeUtils.AllTypes[name];
        }
    }


    [Serializable()]
    public class InGameType
    {
        public string Name;
        public object DefaultValue;
        public InGameType(string name,object defaultValue)
        {
            Name = name;
            DefaultValue = defaultValue;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
