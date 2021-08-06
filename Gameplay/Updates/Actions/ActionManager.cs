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
            foreach (var action in actions.Actions)
            {
                switch (action.ActionType)
                {
                    case ActionType.CardinalMovement:
                        _moveManager.TryStartMovement(action.Actor, action.CardinalDir,action.MoveSpeed);
                        break;
                    case ActionType.RelativeMovement:
                        var cardinal1 = DirectionUtils.ToCardinal(action.Actor.Orientation, action.RelativeDir);
                        _moveManager.TryStartMovement(action.Actor, cardinal1, action.MoveSpeed);
                        break;
                    case ActionType.Rotation:
                        action.Actor.Rotate(action.Rotation);
                        break;
                    case ActionType.CardinalCreation:
                        var templateRuntimeVersion1 = action.Template.GetRuntimeTemplate();
                        _creationManager.TryCreate(action.Actor, templateRuntimeVersion1, action.BlockType, action.CardinalDir);
                        break;
                    case ActionType.RelativeCreation:
                        var cardinal2 = DirectionUtils.ToCardinal(action.Actor.Orientation, action.RelativeDir);
                        var templateRuntimeVersion2 = action.Template.GetRuntimeTemplate();
                        _creationManager.TryCreate(action.Actor, templateRuntimeVersion2, action.BlockType, cardinal2);
                        break;
                    case ActionType.CardinalGiveEnergy:
                        _energyTransferManager.TryGiveEnergy(action.Actor, action.CardinalDir, action.BlockType, action.EnergyAmount);
                        break;
                    case ActionType.RelativeGiveEnergy:
                        var cardinal3 = DirectionUtils.ToCardinal(action.Actor.Orientation, action.RelativeDir);
                        _energyTransferManager.TryGiveEnergy(action.Actor, cardinal3, action.BlockType,action.EnergyAmount);
                        break;
                    case ActionType.CardinalSapEnergy:
                        _energyTransferManager.TrySapEnergy(action.Actor, action.CardinalDir, action.BlockType);
                        break;
                    case ActionType.RelativeSapEnergy:
                        var cardinal4 = DirectionUtils.ToCardinal(action.Actor.Orientation, action.RelativeDir);
                        _energyTransferManager.TrySapEnergy(action.Actor, cardinal4, action.BlockType);
                        break;

                    case ActionType.Zap:
                        _zapManager.TryZap(action.Actor, action.BlockType);
                        break;
                    case ActionType.ApproachBlock:
                        if(action.TargetBlock==null)
                        {
                            continue;
                        }
                        if(action.Actor.Location.AbsoluteLocation== action.TargetBlock.Location.AbsoluteLocation)
                        {
                            continue;
                        }
                        var approachBlockDir = action.Actor.Location.AbsoluteLocation.ApproachDirection(action.TargetBlock.Location.AbsoluteLocation);
                        _moveManager.TryStartMovement(action.Actor, approachBlockDir, action.MoveSpeed);
                        break;
                    case ActionType.ApproachTile:
                        if (action.Actor.Location.AbsoluteLocation == action.TargetTile.AbsoluteLocation)
                        {
                            continue;
                        }
                        var approachTileDir = action.Actor.Location.AbsoluteLocation.ApproachDirection(action.TargetTile.AbsoluteLocation);
                        _moveManager.TryStartMovement(action.Actor, approachTileDir, action.MoveSpeed);
                        break;
                }
            }

            _moveManager.Tick();
            //this was above process originally so if there are move bugs try undoing that
        }
    }
}