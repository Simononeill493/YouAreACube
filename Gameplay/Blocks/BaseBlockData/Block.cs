using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public abstract partial class Block
    {
        public BlockTemplate Template;
        public Tile Location;
        public BlockMode BlockType;

        public int SpeedOffset;
        public Orientation Orientation { get; private set; }

        public int Energy { get; private set; }

        public string Sprite => Template.Sprite;
        public bool Active => Template.Active;
        public int Speed => Template.Speed;
        public int EnergyCap => Template.InitialEnergy;
        public float EnergyRemainingPercentage => ((float)Energy) / EnergyCap;

        public bool MovingBetweenSectors;

        public Block(BlockTemplate template)
        {
            Template = template;
            SpeedOffset = RandomUtils.R.Next(0, Config.TickCycleLength);

            Orientation = Orientation.Top;
            Energy = template.InitialEnergy;

            Location = Tile.Dummy;
        }

        public virtual void Update(UserInput input,ActionsList actions)
        {
            Template.Chips.Execute(this, input,actions);
        }
        public void Rotate(int rotation)
        {
            Orientation = Orientation.Rotate(rotation);
        }
        public abstract bool ShouldBeDestroyed();
        public virtual void BeCreatedBy(Block creator)
        {
            this.SpeedOffset = creator.SpeedOffset + 1;
        }
        public bool InSector(Sector sector)
        {
            return Location.InSector(sector);
        }
        public void AddEnergy(int amount)
        {
            Energy += amount;
            if (Energy > EnergyCap) { Energy = EnergyCap; }
        }
        public void TakeEnergy(int amount)
        {
            Energy -= amount;
            if (Energy < 0)
            {
                throw new Exception("Took more energy from a block than it has - this shouldn't ever be permitted.");
            }
        }

        public void SetTemplateToMain()=> Template = Template.Versions.Main;
        public void SetTemplateToRuntime()=> Template = Templates.GetRuntimeVersion(Template);       
    }
}
