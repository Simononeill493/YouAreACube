﻿using Microsoft.Xna.Framework.Input;
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

        public List<(Keys Key, Chipset Chipset)> KeyEffects = new List<(Keys, Chipset)>();
        public void AddKeyEffect(Keys key, Chipset effect) => KeyEffects.Add((key, effect));


        public void Run(Cube actor, UserInput input, ActionsList actions)
        {
            foreach (var keyEffect in KeyEffects)
            {
                if (input.IsKeyDown(keyEffect.Key))
                {
                    keyEffect.Chipset.Execute(actor, input, actions);
                }
            }
        }


        public List<Chipset> GetSubChipsets()
        {
            var output = new List<Chipset>();

            foreach(var block in KeyEffects.Select(k=>k.Chipset))
            {
                output.AddRange(block.GetChipsetAndSubChipsets());
            }

            return output;
        }

        public List<(string, Chipset)> GetBaseSubChipsets() => KeyEffects.Select(k => (k.Key.ToString(), k.Chipset)).ToList();


        public List<(string,string)> KeyEffectsToString()
        {
            var keysAndEffects = new List<(string, string)>();

            foreach (var keyBlock in KeyEffects)
            {
                keysAndEffects.Add((keyBlock.Key.ToString(), keyBlock.Chipset.Name));
            }

            return keysAndEffects;
        }
    }
}
