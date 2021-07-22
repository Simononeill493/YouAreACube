using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class CreationManager
    {
        private Sector _creatorSector;
        public List<(Cube, IntPoint)> ToPlaceOutsideOfSector = new List<(Cube, IntPoint)>();

        public CreationManager(Sector sector)
        {
            _creatorSector = sector;
        }

        public bool TryCreate(Cube creator,CubeTemplate template,CubeMode blockType,CardinalDirection direction)
        {
            if(!creator.Location.DirectionIsValid(direction))
            {
                return false;
            }

            return TryCreate(creator, template, blockType, direction, creator.Location.Adjacent[direction]);
        }
        private bool TryCreate(Cube creator, CubeTemplate template, CubeMode blockType, CardinalDirection direction,Tile targetPosition)
        {
            if(_canThisBlockBeCreated(creator, template, targetPosition,blockType))
            {
                _create(creator, template, blockType, direction, targetPosition);                    
                return true;                
            }

            return false;
        }
        private void _create(Cube creator, CubeTemplate template, CubeMode blockType, CardinalDirection direction, Tile targetPosition)
        {
            var newBlock = template.Generate(blockType);
            newBlock.BeCreatedBy(creator);
            newBlock.SetOrientation((Orientation)direction);
            newBlock.EnterLocation(targetPosition);

            _addToTargetSector(newBlock, targetPosition);
        }

        private void _addToTargetSector(Cube newBlock, Tile targetPosition)
        {
            if (targetPosition.InSector(_creatorSector))
            {
                _creatorSector.AddBlockToSector(newBlock);
            }
            else
            {
                ToPlaceOutsideOfSector.Add((newBlock, targetPosition.SectorID));
                newBlock.IsMovingBetweenSectors = true;
            }
        }
        private bool _canThisBlockBeCreated(Cube creator, CubeTemplate template, Tile targetPosition, CubeMode blockType)
        {
            switch (blockType)
            {
                case CubeMode.Surface:
                    return _canCreateSurface(creator, template, targetPosition);
                case CubeMode.Ground:
                    return _canCreateGround(creator, template, targetPosition);
                case CubeMode.Ephemeral:
                    return _canCreateEphemeral(creator, template, targetPosition);
            }

            throw new NotImplementedException("Creating unrecognized block type");
        }
        private bool _canCreateEphemeral(Cube creator, CubeTemplate template, Tile targetPosition) => creator.IsInCentreOfTile & (creator.Energy >= template.MaxEnergy);
        private bool _canCreateSurface(Cube creator, CubeTemplate template, Tile targetPosition) => !targetPosition.HasSurface;
        private bool _canCreateGround(Cube creator, CubeTemplate template, Tile targetPosition) => !targetPosition.HasGround;
    }
}