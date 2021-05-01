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
        public override void Run(Block actor, UserInput userInput, ActionsList actions)
        {
            SetOutput(userInput.MouseHoverTile);
        }
    }
}
