using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{

    public class ChipJSONInputData
    {
        public string InputType;
        public string InputValue;

        public ChipJSONInputData(string inputType, string inputValue)
        {
            InputType = inputType;
            InputValue = inputValue;
        }
    }
}
