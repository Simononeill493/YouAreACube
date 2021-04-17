using System;
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
            _destructionManager = new DestructionManager(sector,_moveManager);
            _actionManager = new ActionManager(_moveManager,_creationManager);
        }

        public void Update(ActionsList actions)
        {
            _actionManager.ProcessActions(actions);
            _destructionManager.DestroyDoomedBlocks();
        }

        public void AddToMoving(Block block)
        {
            _moveManager.AddFromOutOfSector(block);
        }

        public (List<(Block,Point)> movedOut, List<(Block, Point)> createdOut) GetSectorEmmigrants()
        {
            var movedOutOutput = new List<(Block, Point)>();
            movedOutOutput.AddRange(_moveManager.MovedOutOfSector);

            var createdOutOutput = new List<(Block, Point)>();
            createdOutOutput.AddRange(_creationManager.CreatedOutOfSector);

            return (movedOutOutput,createdOutOutput);
        }
        public void ClearSectorEmmigrants()
        {
            _moveManager.MovedOutOfSector.Clear();
            _creationManager.CreatedOutOfSector.Clear();
        }
    }
}
