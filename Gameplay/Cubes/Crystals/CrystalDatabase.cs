using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class CrystalDatabase
    {
        public static Dictionary<string, CubeTemplate> CrystalTemplates;

        public static void Load()
        {
            CrystalTemplates = new Dictionary<string, CubeTemplate>();

            var purpleTemplate = new CrystalTemplate("Purple");
            purpleTemplate.Sprite = "CrystalPurple";
            CrystalTemplates["Purple"] = purpleTemplate;
        }
    }
}
