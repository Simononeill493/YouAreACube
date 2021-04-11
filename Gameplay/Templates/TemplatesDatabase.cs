﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    [Serializable()]
    public class TemplatesDatabase : Dictionary<string, TemplateVersionList>
    {
        public List<BlockTemplate> GetAllVersionsOfAllTemplates()
        {
            var output = new List<BlockTemplate>();
            foreach(var template in Values)
            {
                output.AddRange(template.Values);
            }
            return output;
        }

        public List<TemplateVersionList> GetAllTemplates()
        {
            return Values.ToList();
        }

    }
}
