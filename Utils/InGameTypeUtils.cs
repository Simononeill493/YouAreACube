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
        public const string AllVariablesType = "CubeVar";

        public static Dictionary<string,InGameType> InGameTypes;
        public static void Init()
        {
            InGameTypes = new Dictionary<string, InGameType>();
            _addInGameType<CardinalDirection>("CardinalDirection");
            _addInGameType<RelativeDirection>("RelativeDirection");
            _addInGameType<CubeTemplate>("CubeTemplate");
            _addInGameType<CubeMode>("CubeMode");
            _addInGameType<Tile>("Tile");
            _addInGameType<int>("int");
            _addInGameType<bool>("bool");
            _addInGameType<string>("string");
            _addInGameType<Keys>("keys");
            _addInGameType<Cube>("AnyCube");
            _addInGameType<object>("Variable");
            _addInGameType<List<object>>("List<Variable>");
        }

        private static void _addInGameType<TType>(string name)
        {
            InGameTypes[name] = new InGameType(name,default(TType));
        }

        public static bool IsValidInputFor(string top, string bottom)
        {
            if (bottom.Equals(top) | bottom.Equals("Variable"))
            {
                return true;
            }
            else if (bottom.Equals("AnyCube") & top.Equals(nameof(Cube)) | top.Equals(nameof(SurfaceCube)) | top.Equals(nameof(GroundCube)) | top.Equals(nameof(EphemeralCube)))
            {
                return true;
            }
            else if (bottom.Equals("List<Variable>") & top.StartsWith("List<"))
            {
                return true;
            }
            else if (bottom.Equals(nameof(CubeTemplate)) & top.Equals(nameof(CubeTemplateMainPlaceholder)))
            {
                return true;
            }

            return false;
        }

        public static bool IsDiscreteType(string dataType) => TypeUtils.IsEnum(dataType) | dataType.Equals("bool");
        public static bool IsTextEntryType(string dataType) => dataType.Equals("int") | dataType.Equals("string") | dataType.Equals("Keys");
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
