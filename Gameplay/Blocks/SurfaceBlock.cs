using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class SurfaceBlock : Block
    {
        #region health
        public int Health { get; private set; }
        public int HealthCap;
        public float HealthRemainingPercentage => ((float)Health) / HealthCap;

        public bool Dead = false;

        public void Damage(int amount)
        {
            Health -= amount;
            if(Health<0) { Health = 0; }

            if(Health==0)
            {
                Die();
            }
        }

        public void Die()
        {
            Dead = true;
            ColorMask = (128,128,128,255);
        }
        #endregion

        public SurfaceBlock(BlockTemplate template): base(template)
        {
            BlockType = BlockMode.Surface;
            Health = HealthCap = 100;
        }

        public override void EnterLocation(Tile destination)
        {
            if (destination.HasThisSurface(this))
            {
                throw new Exception("Tried to add a ground to a location it already exists in");
            }

            if (!Location.HasThisSurface(this))
            {
                throw new Exception("Surface block is being moved, but its current tile does not register it as present.");
            }

            Location.Surface = null;
            destination.Surface = this;
            this.Location = destination;
        }

        public override bool CanOccupyDestination(Tile destination)=>!destination.HasSurface;
        public override bool ShouldBeDestroyed() => false;
        public override bool CanUpdate => !Dead;
    }
}
