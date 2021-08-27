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
            _addInGameType<Microsoft.Xna.Framework.Input.Keys>("keys");
            _addInGameType<Cube>("AnyCube");
            _addInGameType<object>("Variable");
            _addInGameType<List<object>>("List<Variable>");
        }

        private static void _addInGameType<TType>(string name)
        {
            InGameTypes[name] = new InGameType(name,default(TType));
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
