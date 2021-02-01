using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class CreationManager
    {
        private Sector _currentSector;
        public void SetSector(Sector sector)
        {
            _currentSector = sector;
        }

        public bool TryCreate(Block creator,BlockTemplate template,BlockType blockType,CardinalDirection direction)
        {
            if(!creator.Location.DirectionIsValid(direction))
            {
                return false;
            }

            //TODO - sector management here
            var targetPos = creator.Location.Adjacent[direction];
            return TryCreate(creator, template, blockType, targetPos);
        }

        public bool TryCreate(Block creator, BlockTemplate template, BlockType blockType, Tile targetPosition)
        {
            if(!targetPosition.ContainsBlock(blockType))
            {
                var newBlock = template.Generate(blockType);
                _currentSector.AddBlockToSector(newBlock, targetPosition);
                return true;
            }

            return false;
        }
    }
}
