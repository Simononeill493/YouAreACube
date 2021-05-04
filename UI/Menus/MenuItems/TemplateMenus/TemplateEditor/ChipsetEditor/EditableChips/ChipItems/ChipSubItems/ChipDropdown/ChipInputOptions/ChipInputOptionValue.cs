using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    public class ChipInputOptionValue : ChipInputOption
    {
        public object BaseObject;
        public override string BaseType => TypeUtils.GetTypeDisplayName(BaseObject.GetType());

        public ChipInputOptionValue(object baseObject) : base(InputOptionType.Value) 
        {
            BaseObject = baseObject;
        }

        public override string ToString() 
        {
            return BaseObject.ToString();
        } 
    }
}
