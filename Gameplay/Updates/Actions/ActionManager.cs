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
            //_moveManager.Tick();

            foreach (var effect in actions.Actions)
            {
                switch (effect.ActionType)
                {
                    case ActionType.CardinalMovement:
                        _moveManager.TryStartMovement(effect.Actor, effect.CardinalDir,effect.MoveTotalTicks);
                        break;
                    case ActionType.RelativeMovement:
                        var cardinal = DirectionUtils.ToCardinal(effect.Actor.Orientation, effect.RelativeDir);
                        _moveManager.TryStartMovement(effect.Actor, cardinal, effect.MoveTotalTicks);
                        break;
                    case ActionType.Rotation:
                        effect.Actor.Rotate(effect.Rotation);
                        break;
                    case ActionType.CardinalCreation:
                        var templateRuntimeVersion = Templates.BlockTemplates[effect.BlockTemplate.Name][effect.TemplateVersion];
                        _creationManager.TryCreate(effect.Actor, templateRuntimeVersion, effect.BlockType, effect.CardinalDir);
                        break;
                }
            }

            _moveManager.Tick();
            //this was above process originally so if there are move bugs try undoing that
        }
    }
}