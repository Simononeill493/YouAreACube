using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    public class BlockInputOptionValue : BlockInputOption
    {
        public object BaseObject;
        public override string BaseType => _getBaseTypeName();

        public BlockInputOptionValue(object baseObject) : base(InputOptionType.Value) 
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
            var asTemplate = (BaseObject as CubeTemplate);
            if (asTemplate!= null)
            {
                return asTemplate.ToJsonRep();
            }

            return BaseObject.ToString();
        }


        public override string ToJSONRep() => ToString();

    }
}
