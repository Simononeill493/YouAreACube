using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class Effect
    {
        public EffectType EffectType;
        public Block Actor;

        public CardinalDirection CardinalDir;
        public MovementDirection RelativeDir;
        public int Rotation;

        public BlockType BlockType;
        public BlockTemplate BlockTemplate;

        public Effect(Block actor,EffectType effectType)
        {
            EffectType = effectType;
            Actor = actor;
        }
    }
}
