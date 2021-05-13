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
        public List<(Block, IntPoint)> ToPlaceOutsideOfSector = new List<(Block, IntPoint)>();

        public CreationManager(Sector sector)
        {
            _creatorSector = sector;
        }

        public bool TryCreate(Block creator,BlockTemplate template,BlockMode blockType,CardinalDirection direction)
        {
            if(!creator.Location.DirectionIsValid(direction))
            {
                return false;
            }

            return TryCreate(creator, template, blockType, direction, creator.Location.Adjacent[direction]);
        }
        private bool TryCreate(Block creator, BlockTemplate template, BlockMode blockType, CardinalDirection direction,Tile targetPosition)
        {
            if(_canThisBlockBeCreated(creator, template, targetPosition,blockType))
            {
                _create(creator, template, blockType, direction, targetPosition);                    
                return true;                
            }

            return false;
        }
        private void _create(Block creator, BlockTemplate template, BlockMode blockType, CardinalDirection direction, Tile targetPosition)
        {
            var newBlock = template.Generate(blockType);
            newBlock.BeCreatedBy(creator);
            newBlock.SetOrientation((Orientation)direction);
            newBlock.EnterLocation(targetPosition);

            _addToTargetSector(newBlock, targetPosition);
        }

        private void _addToTargetSector(Block newBlock, Tile targetPosition)
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
        private bool _canThisBlockBeCreated(Block creator, BlockTemplate template, Tile targetPosition, BlockMode blockType)
        {
            switch (blockType)
            {
                case BlockMode.Surface:
                    return _canCreateSurface(creator, template, targetPosition);
                case BlockMode.Ground:
                    return _canCreateGround(creator, template, targetPosition);
                case BlockMode.Ephemeral:
                    return _canCreateEphemeral(creator, template, targetPosition);
            }

            throw new NotImplementedException("Creating unrecognized block type");
        }
        private bool _canCreateEphemeral(Block creator, BlockTemplate template, Tile targetPosition) => creator.IsInCentreOfBlock & (creator.Energy >= template.MaxEnergy);
        private bool _canCreateSurface(Block creator, BlockTemplate template, Tile targetPosition) => !targetPosition.HasSurface;
        private bool _canCreateGround(Block creator, BlockTemplate template, Tile targetPosition) => !targetPosition.HasGround;
    }
}