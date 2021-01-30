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
        private CreationManager _creationManager;

        public EffectManager()
        {
            _moveManager = new MoveManager();
            _creationManager = new CreationManager();
        }

        public void ProcessEffects(Sector sectorToProcess,EffectsList effects)
        {
            _creationManager.SetSector(sectorToProcess);
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
                    case EffectType.CardinalCreation:
                        _creationManager.TryCreate(effect.Actor, effect.BlockTemplate, effect.BlockType, effect.CardinalDir);
                        break;
                }
            }
        }
    }
}