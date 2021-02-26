﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace IAmACube
{
    [Serializable()]
    internal class FirstOfListChip<T> : OutputPin<T>, InputPin<List<T>>
    {
        public List<T> ChipInput { get; set; }

        public override void Run(Block actor, UserInput input, ActionsList actions)
        {
            SetOutput(ChipInput.FirstOrDefault());
        }
    }
}