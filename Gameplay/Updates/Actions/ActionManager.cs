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
        private EnergyTransferManager _energyTransferManager;
        private ZapManager _zapManager;


        public ActionManager(MoveManager moveManager,CreationManager creationManager,EnergyTransferManager energyTransferManager, ZapManager zapManager)
        {
            _moveManager =  moveManager;
            _creationManager =  creationManager;
            _energyTransferManager = energyTransferManager;
            _zapManager = zapManager;
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
                    case ActionType.CardinalGiveEnergy:
                        _energyTransferManager.TryGiveEnergy(effect.Actor, effect.CardinalDir, effect.BlockType, effect.EnergyAmount);
                        break;
                    case ActionType.RelativeGiveEnergy:
                        var cardinal3 = DirectionUtils.ToCardinal(effect.Actor.Orientation, effect.RelativeDir);
                        _energyTransferManager.TryGiveEnergy(effect.Actor, cardinal3, effect.BlockType,effect.EnergyAmount);
                        break;
                    case ActionType.Zap:
                        _zapManager.TryZap(effect.Actor, effect.BlockType);
                        break;

                }
            }

            _moveManager.Tick();
            //this was above process originally so if there are move bugs try undoing that
        }
    }
}