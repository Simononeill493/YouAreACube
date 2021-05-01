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
        private Sector _sector;
        public List<(Block, IntPoint)> PlacedOutOfSector = new List<(Block, IntPoint)>();

        public CreationManager(Sector sector)
        {
            _sector = sector;
        }


        public bool TryCreate(Block creator,BlockTemplate template,BlockMode blockType,CardinalDirection direction)
        {
            if(!creator.Location.DirectionIsValid(direction))
            {
                return false;
            }

            var targetPos = creator.Location.Adjacent[direction];
            return TryCreate(creator, template, blockType, targetPos, direction);
        }
        private bool TryCreate(Block creator, BlockTemplate template, BlockMode blockType, Tile targetPosition,CardinalDirection direction)
        {
            if(_canThisBlockBeCreated(creator, template,blockType, targetPosition))
            {
                _create(creator, template, blockType, targetPosition,direction);                    
                return true;                
            }

            return false;
        }

        private void _create(Block creator, BlockTemplate template, BlockMode blockType, Tile targetPosition, CardinalDirection direction)
        {
            var newBlock = template.Generate(blockType);

            newBlock.BeCreatedBy(creator);
            newBlock.SetOrientation((Orientation)direction);
            newBlock.EnterLocation(targetPosition);

            if(targetPosition.InSector(_sector))
            {
                _sector.AddBlockToSector(newBlock);
            }
            else
            {
                newBlock.IsMovingBetweenSectors = true;
                PlacedOutOfSector.Add((newBlock, targetPosition.SectorID));
            }
        }




        private bool _canThisBlockBeCreated(Block creator, BlockTemplate template, BlockMode blockType, Tile targetPosition)
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
        private bool _canCreateEphemeral(Block creator, BlockTemplate template, Tile targetPosition) => creator.IsInCentreOfBlock & (creator.Energy >= template.EnergyCap);
        private bool _canCreateSurface(Block creator, BlockTemplate template, Tile targetPosition) => !targetPosition.HasSurface;
        private bool _canCreateGround(Block creator, BlockTemplate template, Tile targetPosition) => throw new NotImplementedException("Haven't implemented ground creation");
    }
}