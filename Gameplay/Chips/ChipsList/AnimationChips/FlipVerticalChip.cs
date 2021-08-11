using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    class FlipVerticalChip :IChip
    {
        public string Name { get; set; }

        public void Run(Cube actor, UserInput userInput, ActionsList actions)
        {
            actor.SpriteData.VerticalFlip = !actor.SpriteData.VerticalFlip;
        }
    }
}
