using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class EffectsList
    {
        public List<Effect> Effects = new List<Effect>();

        public void StartMove(Block block,CardinalDirection direction)
        {
            var moveCardinalEffect = new Effect(block, EffectType.CardinalMovement) { CardinalDir = direction };
            Effects.Add(moveCardinalEffect);
        }

        public void StartMove(Block block, MovementDirection direction)
        {
            var moveRelativeEffect = new Effect(block, EffectType.RelativeMovement) { RelativeDir = direction };
            Effects.Add(moveRelativeEffect);
        }

        public void StartRotation(Block block, int rotation)
        {
            var rotationEffect = new Effect(block, EffectType.Rotation) { Rotation = rotation };
            Effects.Add(rotationEffect);
        }
    }
}
