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
            if(_canThisBlockBeCreated(creator, template,blockType, targetPosition))
            {
                _create(creator, template, blockType, targetPosition);                    
                return true;                
            }

            return false;
        }

        private void _create(Block creator, BlockTemplate template, BlockType blockType, Tile targetPosition)
        {
            var newBlock = template.Generate(blockType);
            newBlock.BeCreatedBy(creator);

            _currentSector.AddBlockToSector(newBlock, targetPosition);
        }

        private bool _canThisBlockBeCreated(Block creator, BlockTemplate template, BlockType blockType, Tile targetPosition)
        {
            switch (blockType)
            {
                case BlockType.Surface:
                    return _canCreateSurface(creator, template, targetPosition);
                case BlockType.Ground:
                    return _canCreateGround(creator, template, targetPosition);
                case BlockType.Ephemeral:
                    return _canCreateEphemeral(creator, template, targetPosition);
            }

            throw new NotImplementedException("Creating unrecognized block type");
        }
        private bool _canCreateEphemeral(Block creator, BlockTemplate template, Tile targetPosition)
        {
            return !targetPosition.HasEphemeral & creator.IsInCentreOfBlock & (creator.Energy>= template.InitialEnergy);
        }
        private bool _canCreateSurface(Block creator, BlockTemplate template, Tile targetPosition)
        {
            return !targetPosition.HasSurface;
        }
        private bool _canCreateGround(Block creator, BlockTemplate template, Tile targetPosition)
        {
            throw new NotImplementedException("Haven't implemented ground creation");
        }
    }
}