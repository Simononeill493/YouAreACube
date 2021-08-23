using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class InGameTypeUtils
    {
        public static List<InGameType> InGameTypes;
        public static void Init()
        {
            InGameTypes = new List<InGameType>();
            InGameTypes.Add(new InGameType("CardinalDirection"));
            InGameTypes.Add(new InGameType("RelativeDirection"));
            InGameTypes.Add(new InGameType("CubeTemplate"));
            InGameTypes.Add(new InGameType("CubeMode"));
            InGameTypes.Add(new InGameType("Tile"));
            InGameTypes.Add(new InGameType("int"));
            InGameTypes.Add(new InGameType("bool"));
            InGameTypes.Add(new InGameType("string"));
            InGameTypes.Add(new InGameType("keys"));

            InGameTypes.Add(new InGameType("AnyCube"));
            InGameTypes.Add(new InGameType("Variable"));
            InGameTypes.Add(new InGameType("List<Variable>"));
        }
    }


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
