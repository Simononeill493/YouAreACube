using Microsoft.Xna.Framework;
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

        public (int, int, int, int) ColorMask = (255, 255, 255, 255);
        public string Sprite => Template.Sprite;

        public bool Active => Template.Active;
        public int Speed => Template.Speed;
        public int EnergyCap => Template.EnergyCap;
        public float EnergyRemainingPercentage => ((float)Energy) / EnergyCap;

        public int _id;

        public Block(BlockTemplate template)
        {
            Template = template;
            SpeedOffset = RandomUtils.R.Next(0, Config.TickCycleLength);
            _id = RandomUtils.R.Next(0, 9999);

            Orientation = Orientation.Top;
            Energy = template.EnergyCap;

            Location = Tile.Dummy;
        }

        public void AddEnergy(int amount)
        {
            Energy += amount;
            if (Energy > EnergyCap) { Energy = EnergyCap; }
        }
        public virtual void TakeEnergy(int amount)
        {
            Energy -= amount;
            if (Energy < 0)
            {
                throw new Exception("Took more energy from a block than it has - this shouldn't ever be permitted.");
            }
        }
        public virtual BlockEnergyTransferResult TryTakeEnergyFrom(Block source, int amount)
        {
            if (amount > source.Energy)
            {
                amount = source.Energy;
            }

            this.AddEnergy(amount);
            source.TakeEnergy(amount);

            return BlockEnergyTransferResult.Success;
        }

        public virtual void Update(UserInput input, ActionsList actions) => Template.Chips.Execute(this, input, actions);

        public void SetOrientation(Orientation orientation) => Orientation = orientation;
        public void Rotate(int rotation) => Orientation = Orientation.Rotate(rotation);
        public virtual void BeCreatedBy(Block creator) => this.SpeedOffset = creator.SpeedOffset + 1;
        public bool InSector(Sector sector) => Location.InSector(sector);
        public void SetTemplateToMain() => Template = Template.Versions.Main;
        public void SetTemplateToRuntime() => Template = Templates.GetRuntimeVersion(Template);
        public virtual bool CanUpdate => true;
        public abstract bool ShouldBeDestroyed();
    }

    public enum BlockEnergyTransferResult
    {
        Success,
        Failure_SourceIsDyingEphemeral,
    }
}
