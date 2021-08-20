using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public interface IChipInputFetcher<TInputType>
    {
        TInputType Value { get; }
        string AsString { get; }
    }

    public static class IChipInputFetcherName
    {
        public const string InterfaceName = "IChipInputFetcher";
        public const string StaticReferenceName = "ChipInputFetcherStatic";

    }

    public static class IChipInputFetcherUtils
    {
        public static bool IsStatic<TInputType>(this IChipInputFetcher<TInputType> inputFetcher)
        {
            return false;
        }

    }


}
