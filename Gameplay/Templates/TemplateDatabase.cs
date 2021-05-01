using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    [Serializable()]
    public class TemplateDatabase : Dictionary<string, TemplateVersionDictionary>
    {
        public List<BlockTemplate> GetAllVersionsOfAllTemplates()
        {
            var output = new List<BlockTemplate>();
            foreach(var versionList in GetAllVersionLists())
            {
                output.AddRange(versionList.Versions);
            }
            return output;
        }

        public List<TemplateVersionDictionary> GetAllVersionLists() => Values.ToList();
    }
}
