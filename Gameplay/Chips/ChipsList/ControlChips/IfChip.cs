﻿using System;
using System.Collections.Generic;

namespace IAmACube
{
    [Serializable()]
    internal class IfChip : IControlChip, InputPin<bool>
    {
        public string Name { get; set; }

        public bool ChipInput1 { get; set; }

        public ChipBlock Yes;
        public ChipBlock No;

        public void Run(Block actor,UserInput input,ActionsList actions)
        {
            if(ChipInput1)
            {
                Yes.Execute(actor, input, actions);
            }
            else
            {
                No.Execute(actor, input, actions);
            }
        }

        public List<ChipBlock> GetSubBlocks()
        {
            var output = new List<ChipBlock>();

            if(Yes!=null) 
            {
                output.AddRange(Yes.GetBlockAndSubBlocks());
            }
            if (No != null)
            {
                output.AddRange(No.GetBlockAndSubBlocks());
            }

            return output;
        }
    }
}