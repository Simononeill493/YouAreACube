using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockInputOptionParseable : BlockInputOption
    {
        public override string BaseType => TypeUtils.GetTypeDisplayNameOfStringRepresentation(StringRepresentation, _baseTypes);
        public string StringRepresentation;
        private List<Type> _baseTypes;

        public BlockInputOptionParseable(string stringRepresentation,List<Type> baseTypes) : base(InputOptionType.Parseable)
        {
            StringRepresentation = stringRepresentation;
            _baseTypes = baseTypes;
        }

        public override string ToJSONRep() => StringRepresentation;
        public override string ToString() => StringRepresentation;
    }
}
