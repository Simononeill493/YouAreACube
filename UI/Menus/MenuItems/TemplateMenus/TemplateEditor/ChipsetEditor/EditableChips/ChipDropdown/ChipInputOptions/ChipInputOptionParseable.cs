using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ChipInputOptionParseable : ChipInputOption
    {
        public override string BaseType => TypeUtils.GetTypeOfStringRepresentation(StringRepresentation, _baseTypes);
        public string StringRepresentation;
        private List<Type> _baseTypes;

        public ChipInputOptionParseable(string stringRepresentation,List<Type> baseTypes) : base(InputOptionType.Parseable)
        {
            StringRepresentation = stringRepresentation;
            _baseTypes = baseTypes;
        }

        public override string ToString() => StringRepresentation;
    }
}
