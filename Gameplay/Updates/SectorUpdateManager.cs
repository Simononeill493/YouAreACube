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
        public ActionManager ActionManager;
        public MoveManager MoveManager;
        public CreationManager CreationManager;
        public DestructionManager DestructionManager;

        public SectorUpdateManager(Sector sector)
        {
            MoveManager = new MoveManager(sector);
            CreationManager = new CreationManager(sector);
            DestructionManager = new DestructionManager(sector,MoveManager);
            ActionManager = new ActionManager(MoveManager,CreationManager);
        }

        public void Update(ActionsList actions)
        {
            ActionManager.ProcessActions(actions);
            DestructionManager.DestroyDoomedBlocks();
        }
    }
}
