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
            foreach(var block in _sector.GetDoomedBlocks().ToList())
            {
                //if (block.IsMovingBetweenSectors) { continue; }
                //Console.WriteLine("Destroying block " + block._id);

                _clearFromTile(block);
                _sector.RemoveBlockFromSector(block);
            }
        }

        private void _clearFromTile(Cube block)
        {
            block.Location.ClearBlock(block);
            block.ClearLocation();
        }
    }
}
