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
        List<Tuple<Keys, ChipBlock>> KeyEffects = new List<Tuple<Keys, ChipBlock>>();

        public void AddKeyEffect(Keys key,ChipBlock effect)
        {
            KeyEffects.Add(new Tuple<Keys, ChipBlock>(key, effect));
        }

        public void ExecuteOutput(Block actor, UserInput input, EffectsList effects)
        {
            foreach(var keyEffect in KeyEffects)
            {
                if(input.IsKeyDown(keyEffect.Item1))
                {
                    keyEffect.Item2.Execute(actor, input, effects);
                }
            }
        }

        public void Run(Block actor, UserInput userInput, EffectsList effects) {}
    }
}
