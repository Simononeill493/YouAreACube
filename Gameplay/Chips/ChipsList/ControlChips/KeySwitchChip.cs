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

        public List<Tuple<Keys, ChipBlock>> KeyEffects = new List<Tuple<Keys, ChipBlock>>();

        public void AddKeyEffect(Keys key,ChipBlock effect)
        {
            KeyEffects.Add(new Tuple<Keys, ChipBlock>(key, effect));
        }

        public void Run(Block actor, UserInput input, ActionsList actions)
        {
            foreach (var keyEffect in KeyEffects)
            {
                if (input.IsKeyDown(keyEffect.Item1))
                {
                    keyEffect.Item2.Execute(actor, input, actions);
                }
            }
        }

        public List<ChipBlock> GetSubBlocks()
        {
            var output = new List<ChipBlock>();

            foreach(var block in KeyEffects.Select(k=>k.Item2))
            {
                output.AddRange(block.GetBlockAndSubBlocks());
            }

            return output;
        }
    }
}
