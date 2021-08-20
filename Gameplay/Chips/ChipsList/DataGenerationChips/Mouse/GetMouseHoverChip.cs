using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class GetMouseHoverChip : OutputPin<Tile>
    {
        public string Name { get; set; }
        public Tile Value { get; set; }

        public void Run(Cube actor, UserInput userInput, ActionsList actions)
        {
            Value = (userInput.MouseHoverTile);
        }
    }
}
