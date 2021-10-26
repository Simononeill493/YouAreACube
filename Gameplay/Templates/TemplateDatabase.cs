using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    [Serializable()]
    public class TemplateDatabase : Dictionary<string, TemplateVersionDictionary>
    {
        public List<CubeTemplate> GetAllVersionsOfAllTemplates()
        {
            var output = new List<CubeTemplate>();
            foreach(var versionList in GetAllVersionLists())
            {
                output.AddRange(versionList.Versions);
            }
            return output;
        }

        public bool Exists(string name, int version) => this.ContainsKey(name) && this[name].ContainsKey(version);

        public List<TemplateVersionDictionary> GetAllVersionLists() => Values.ToList();
    }
}
