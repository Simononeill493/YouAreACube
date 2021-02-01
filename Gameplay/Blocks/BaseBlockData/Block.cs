using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public abstract class Block
    {
        public Tile Location;
        public BlockType BlockType;
        public int SpeedOffset;

        public bool IsMoving;
        public BlockMovementData MovementData;

        public Orientation Orientation;

        public string Sprite => _template.Sprite;
        public bool Active => _template.Active;
        public int Speed => _template.Speed;
        public int EnergyCap => _template.EnergyCap;

        protected BlockTemplate _template;

        public int Energy;

        public Block(BlockTemplate template)
        {
            _template = template;
            SpeedOffset = RandomUtils.R.Next(0, Config.TickCycleLength);
            MovementData = new BlockMovementData();

            Orientation = Orientation.Top;
            Energy = template.EnergyCap;
        }

        public virtual void Update(UserInput input,ActionsList actions)
        {
            _template.Chips.Execute(this, input,actions);
        }

        public void Rotate(int rotation)
        {
            Orientation = Orientation.Rotate(rotation);
        }

        public bool TryMove(RelativeDirection movementDirection)
        {
            return TryMove(DirectionUtils.ToCardinal(Orientation, movementDirection));
        }
        public bool TryMove(CardinalDirection direction)
        {
            var destination = Location.Adjacent[direction];

            if(CanOccupyDestination(destination))
            {
                _move(destination);
                Energy--;
                return true;
            }

            return false;
        }

        protected abstract void _move(Tile destination);

        public bool CanStartMoving()
        {
            return (!IsMoving) & (Energy > 0);
        }
        public abstract bool CanOccupyDestination(Tile destination);
    }
}
