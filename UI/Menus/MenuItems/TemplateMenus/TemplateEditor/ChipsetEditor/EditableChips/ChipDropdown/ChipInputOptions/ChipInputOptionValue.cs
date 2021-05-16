using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    public class ChipInputOptionValue : ChipInputOption
    {
        public object BaseObject;
        public override string BaseType => _getBaseTypeName();

        public ChipInputOptionValue(object baseObject) : base(InputOptionType.Value) 
        {
            BaseObject = baseObject;
        }

        private string _getBaseTypeName()
        {
            var name = TypeUtils.GetTypeDisplayName(BaseObject.GetType());
            return name;
        }

        public override string ToString()
        {
            if (BaseObject.GetType() == typeof(BlockTemplate))
            {
                var template = (BlockTemplate)BaseObject;
                return template.Versions.Name + '|' + template.Version;
            }

            return BaseObject.ToString();
        }
    }
}
