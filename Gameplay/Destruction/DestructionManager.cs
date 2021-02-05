using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class DestructionManager
    {
        private MoveManager _moveManager;
        public DestructionManager(MoveManager moveManager)
        {
            _moveManager = moveManager;
        }

        public void DestroyDoomedBlocks(Sector sector)
        {
            foreach(var block in sector.DoomedBlocks.ToList())
            {
                sector.RemoveBlockFromSector(block);
                _moveManager.ManuallyCancelMovement(block);
            }
        }
    }
}
