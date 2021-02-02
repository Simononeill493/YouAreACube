using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class ActionManager
    {
        private MoveManager _moveManager;
        private CreationManager _creationManager;

        public ActionManager(MoveManager moveManager,CreationManager creationManager)
        {
            _moveManager =  moveManager;
            _creationManager =  creationManager;
        }

        public void ProcessActions(Sector sectorToProcess,ActionsList effects)
        {
            _creationManager.SetSector(sectorToProcess);
            _moveManager.TickCurrentMoves();

            foreach(var effect in effects.Actions)
            {
                switch (effect.ActionType)
                {
                    case ActionType.CardinalMovement:
                        _moveManager.TryStartMoving(effect.Actor, effect.CardinalDir,effect.MoveSpeed);
                        break;
                    case ActionType.RelativeMovement:
                        _moveManager.TryStartMoving(effect.Actor, effect.RelativeDir,effect.MoveSpeed);
                        break;
                    case ActionType.Rotation:
                        effect.Actor.Rotate(effect.Rotation);
                        break;
                    case ActionType.CardinalCreation:
                        _creationManager.TryCreate(effect.Actor, effect.BlockTemplate, effect.BlockType, effect.CardinalDir);
                        break;
                }
            }
        }
    }
}