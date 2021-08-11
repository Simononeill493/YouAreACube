using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public abstract class HealthCube : Cube
    {
        public bool Dormant = false;
        public bool Invincible { get; set; }
        public int Health { get; private set; }
        public int MaxHealth => Template.MaxHealth;
        public float HealthRemainingPercentage => ((float)Health) / MaxHealth;

        protected HealthCube(CubeTemplate template, Kernel source, CubeMode blockType) : base(template,source,blockType)
        {
            Health = MaxHealth;
            Invincible = template.Invincible;
        }


        public override void DealDamage(int amount)
        {
            if(Dormant || Invincible)
            {
                return;
            }

            Health -= amount;

            if (Health <= 0) 
            { 
                Health = 0;
                GoDormant();
            }
        }

        public void GoDormant()
        {
            Dormant = true;
            SpriteData.ColorMask = XnaColors.DeadCubeColor;
        }

        public override bool ToBeDeleted() => (Dormant & !Active);
        public override bool CanUpdate => !Dormant;
    }
}
