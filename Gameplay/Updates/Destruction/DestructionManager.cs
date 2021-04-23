using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class DestructionManager
    {
        private Sector _sector;
        private MoveManager _moveManager;
        public DestructionManager(Sector sector,MoveManager moveManager)
        {
            _sector = sector;
            _moveManager = moveManager;
        }

        public void DestroyDoomedBlocks()
        {
            foreach(var block in _sector.DoomedBlocks.ToList())
            {
                if(block.IsMovingBetweenSectors)
                {
                    continue;
                }

                _clearFromTile(block);
                _sector.RemoveFromSectorLists(block);

                if(block.IsMoving)
                {
                    _moveManager.DestroyBlock(block);
                }

                Console.WriteLine("Block " + block._id + " destroyed.");
            }
        }

        private void _clearFromTile(Block block)
        {
            block.Location.ClearBlock(block);
            block.Location = null;
        }
    }
}
