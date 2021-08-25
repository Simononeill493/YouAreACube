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
            _addInGameType("CardinalDirection");
            _addInGameType("RelativeDirection");
            _addInGameType("CubeTemplate");
            _addInGameType("CubeMode");
            _addInGameType("Tile");
            _addInGameType("int");
            _addInGameType("bool");
            _addInGameType("string");
            _addInGameType("keys");
            _addInGameType("AnyCube");
            _addInGameType("Variable");
            _addInGameType("List<Variable>");
        }

        private static void _addInGameType(string name)
        {
            InGameTypes[name] = new InGameType(name);
        }
    }


    [Serializable()]
    public class InGameType
    {
        public string Name;
        public InGameType(string name)
        {
            Name = name;
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
