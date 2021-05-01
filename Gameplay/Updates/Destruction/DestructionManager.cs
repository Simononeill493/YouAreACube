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
        public DestructionManager(Sector sector)
        {
            _sector = sector;
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
                _sector.RemoveBlockFromSector(block);

                //Console.WriteLine("Block " + block._id + " destroyed.");
            }
        }

        private void _clearFromTile(Block block)
        {
            block.Location.ClearBlock(block);
            block.Location = null;
        }
    }
}
