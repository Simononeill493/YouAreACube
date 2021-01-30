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

        public void TryCreate(Block creator,BlockTemplate template,BlockType blockType,CardinalDirection direction)
        {
            if(!creator.Location.DirectionIsValid(direction))
            {
                return;
            }

            //TODO - sector management here
            var targetPos = creator.Location.Adjacent[direction];
            TryCreate(creator, template, blockType, targetPos);
        }

        public void TryCreate(Block creator, BlockTemplate template, BlockType blockType, Tile targetPosition)
        {
            switch (blockType)
            {
                case BlockType.Surface:
                    TryCreateSurface(creator, template, targetPosition);
                    break;
                case BlockType.Ground:
                    TryCreateGround(creator, template, targetPosition);
                    break;
                case BlockType.Ephemeral:
                    TryCreateEphemeral(creator, template, targetPosition);
                    break;
            }
        }

        public void TryCreateSurface(Block creator,BlockTemplate template,Tile targetPosition)
        {
            if(!targetPosition.HasSurface)
            {
                var newBLock = template.GenerateSurface();
                _currentSector.AddSurfaceToSector(newBLock, targetPosition);
            }
        }

        public void TryCreateEphemeral(Block creator, BlockTemplate template, Tile targetPosition)
        {
            throw new NotImplementedException();
        }

        public void TryCreateGround(Block creator, BlockTemplate template, Tile targetPosition)
        {
            throw new NotImplementedException();
        }
    }
}
