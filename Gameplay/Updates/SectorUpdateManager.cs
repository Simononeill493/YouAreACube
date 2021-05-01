﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class SectorUpdateManager
    {
        private ActionManager _actionManager;
        private MoveManager _moveManager;
        private CreationManager _creationManager;
        private DestructionManager _destructionManager;

        public SectorUpdateManager(Sector sector)
        {
            _moveManager = new MoveManager(sector);
            _creationManager = new CreationManager(sector);
            _destructionManager = new DestructionManager(sector);
            _actionManager = new ActionManager(_moveManager,_creationManager);
        }

        public void Update(ActionsList actions)
        {
            _actionManager.ProcessActions(actions);
            _destructionManager.DestroyDoomedBlocks();
        }

        public (List<(Block,IntPoint)> movedOut, List<(Block, IntPoint)> createdOut) GetSectorEmmigrants()
        {
            var movedOutOutput = new List<(Block, IntPoint)>();
            movedOutOutput.AddRange(_moveManager.MovedOutOfSector);

            var createdOutOutput = new List<(Block, IntPoint)>();
            createdOutOutput.AddRange(_creationManager.PlacedOutOfSector);

            return (movedOutOutput,createdOutOutput);
        }
        public void ClearSectorEmmigrants()
        {
            _moveManager.MovedOutOfSector.Clear();
            _creationManager.PlacedOutOfSector.Clear();
        }

        public void AddBlockToUpdates(Block block)
        {
            if (block.IsMoving)
            {
                _moveManager.AddMovingBlock(block);
            }
        }

        public void RemoveBlockFromUpdates(Block block)
        {
            if (block.IsMoving)
            {
                _moveManager.RemoveMovingBlock(block);
            }
        }
    }
}
