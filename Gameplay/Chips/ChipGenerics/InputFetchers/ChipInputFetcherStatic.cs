using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ChipInputFetcherStatic<TInputType> : IChipInputFetcher<TInputType>
    {
        private TInputType _input;

        public ChipInputFetcherStatic() { } // Reflection only (I know, I know)

        public ChipInputFetcherStatic(TInputType input)
        {
            _input = input;
        }

        public TInputType Value => _input;
        public string AsString =>_input.ToString()
    }

    public static class ChipInputStatic
    {
        public static ChipInputFetcherStatic<CubeMode> Ephemeral => new ChipInputFetcherStatic<CubeMode>(CubeMode.Ephemeral);
        public static ChipInputFetcherStatic<CubeMode> Ground => new ChipInputFetcherStatic<CubeMode>(CubeMode.Ground);
        public static ChipInputFetcherStatic<CubeMode> Surface => new ChipInputFetcherStatic<CubeMode>(CubeMode.Surface);

        public static ChipInputFetcherStatic<int> Integer(int num) => new ChipInputFetcherStatic<int>(num);

        public static ChipInputFetcherStatic<CardinalDirection> Direction(CardinalDirection direction) => new ChipInputFetcherStatic<CardinalDirection>(direction);
        public static ChipInputFetcherStatic<RelativeDirection> Direction(RelativeDirection direction) => new ChipInputFetcherStatic<RelativeDirection>(direction);

        public static object CreateDynamically(object staticObject)
        {
            var objectType = staticObject.GetType();
            if (objectType.Equals(typeof(CubeTemplateMainPlaceholder)))
            {
                objectType = typeof(CubeTemplate);
            }

            var genericChipType = TypeUtils.GetTypeByName(IChipInputFetcherName.StaticReferenceName + "`1");
            var typeArgumentsAsType = new List<Type>() { objectType }.ToArray();
            var genericRuntimeType = genericChipType.MakeGenericType(typeArgumentsAsType);
            var genericInstance = Activator.CreateInstance(genericRuntimeType);

            var valueMethod = genericRuntimeType.GetField("_input", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
            valueMethod.SetValue(genericInstance, staticObject);

            return genericInstance;

        }
    }
}
