﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube.Gameplay.Chips.ChipsList.ActionChips
{
    [Serializable()]
    class ApproachTileChip : InputPin<Tile>
    {
        public string Name { get; set; }
        public Tile ChipInput1 { get; set; }

        public void Run(Block actor, UserInput userInput, ActionsList actions)
        {
            actions.AddMoveAction(actor, actor.Location.AbsoluteLocation.ApproachDirection(ChipInput1.AbsoluteLocation));
        }
    }
}