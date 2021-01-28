using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class EffectManager
    {
        private MoveManager _moveManager;

        public EffectManager()
        {
            _moveManager = new MoveManager();
        }

        public void ProcessEffects(EffectsList effects)
        {
            _moveManager.TickCurrentMoves();

            foreach(var effect in effects.Effects)
            {
                switch (effect.EffectType)
                {
                    case EffectType.CardinalMovement:
                        _moveManager.ProcessNewMoveRequest(effect.Actor, effect.CardinalDir);
                        break;
                    case EffectType.RelativeMovement:
                        _moveManager.ProcessNewMoveRequest(effect.Actor, effect.RelativeDir);
                        break;
                    case EffectType.Rotation:
                        effect.Actor.Rotate(effect.Rotation);
                        break;
                }
            }
        }
    }
}