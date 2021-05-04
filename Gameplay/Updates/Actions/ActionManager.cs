using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class ActionManager
    {
        private MoveManager _moveManager;
        private CreationManager _creationManager;

        public ActionManager(MoveManager moveManager,CreationManager creationManager)
        {
            _moveManager =  moveManager;
            _creationManager =  creationManager;
        }

        public void ProcessActions(ActionsList actions)
        {
            foreach (var effect in actions.Actions)
            {
                switch (effect.ActionType)
                {
                    case ActionType.CardinalMovement:
                        _moveManager.TryStartMovement(effect.Actor, effect.CardinalDir,effect.MoveSpeed);
                        break;
                    case ActionType.RelativeMovement:
                        var cardinal1 = DirectionUtils.ToCardinal(effect.Actor.Orientation, effect.RelativeDir);
                        _moveManager.TryStartMovement(effect.Actor, cardinal1, effect.MoveSpeed);
                        break;
                    case ActionType.Rotation:
                        effect.Actor.Rotate(effect.Rotation);
                        break;
                    case ActionType.CardinalCreation:
                        var templateRuntimeVersion1 = Templates.Database[effect.Template.Name][effect.Version];
                        _creationManager.TryCreate(effect.Actor, templateRuntimeVersion1, effect.BlockType, effect.CardinalDir);
                        break;
                    case ActionType.RelativeCreation:
                        var cardinal2 = DirectionUtils.ToCardinal(effect.Actor.Orientation, effect.RelativeDir);
                        var templateRuntimeVersion2 = Templates.Database[effect.Template.Name][effect.Version];
                        _creationManager.TryCreate(effect.Actor, templateRuntimeVersion2, effect.BlockType, cardinal2);
                        break;

                }
            }

            _moveManager.Tick();
            //this was above process originally so if there are move bugs try undoing that
        }
    }
}