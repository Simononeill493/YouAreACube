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
        public bool IsInCentreOfBlock => MovementData.IsInCentreOfBlock;

        public BlockMovementData MovementData;

        public Orientation Orientation;

        public string Sprite => _template.Sprite;
        public bool Active => _template.Active;
        public int Speed => _template.Speed;
        public int EnergyCap => _template.InitialEnergy;
        public float EnergyRemainingPercentage => ((float)Energy) / EnergyCap;

        protected BlockTemplate _template;

        public int Energy { get; private set; }
        public void AddEnergy(int amount)
        {
            Energy += amount;
            if(Energy>EnergyCap) { Energy = EnergyCap; }
        }
        public void TakeEnergy(int amount)
        {
            Energy -= amount;
            if (Energy < 0) 
            {
                Console.WriteLine("Warning: energy has gone negative.");
            }

        }

        public Block(BlockTemplate template)
        {
            _template = template;
            SpeedOffset = RandomUtils.R.Next(0, Config.TickCycleLength);
            MovementData = new BlockMovementData();

            Orientation = Orientation.Top;
            Energy = template.InitialEnergy;
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
        public abstract bool ShouldBeDestroyed();

        public virtual void BeCreatedBy(Block creator)
        {
            this.SpeedOffset = creator.SpeedOffset + 1;
        }
    }
}
