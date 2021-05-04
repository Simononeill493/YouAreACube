using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ChipInputOptionParseable : ChipInputOption
    {
        public string StringRepresentation;
        public List<Type> BaseTypes;

        public override string BaseType => _tryResolveTypeOfString();

        public ChipInputOptionParseable(string stringRepresentation,List<Type> baseTypes) : base(InputOptionType.Parseable)
        {
            StringRepresentation = stringRepresentation;
            BaseTypes = baseTypes;
        }


        private string _tryResolveTypeOfString()
        {
            foreach(var type in BaseTypes)
            {
                if(TypeUtils.ParseType(type,StringRepresentation)!=null)
                {
                    return type.Name;
                }
            }

            throw new Exception("Cannot resolve type of string");
        }


        public override string ToString() => StringRepresentation;
    }
}
