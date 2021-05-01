using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    class KeySwitchChip : IControlChip, IChip
    {
        public string Name { get; set; }

        public List<(Keys Key, ChipBlock Block)> KeyEffects = new List<(Keys, ChipBlock)>();
        public void AddKeyEffect(Keys key, ChipBlock effect) => KeyEffects.Add((key, effect));


        public void Run(Block actor, UserInput input, ActionsList actions)
        {
            foreach (var keyEffect in KeyEffects)
            {
                if (input.IsKeyDown(keyEffect.Key))
                {
                    keyEffect.Block.Execute(actor, input, actions);
                }
            }
        }


        public List<ChipBlock> GetSubBlocks()
        {
            var output = new List<ChipBlock>();

            foreach(var block in KeyEffects.Select(k=>k.Block))
            {
                output.AddRange(block.GetBlockAndSubBlocks());
            }

            return output;
        }
    }
}
