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
        public int Health { get; private set; }
        public int MaxHealth => Template.MaxHealth;
        public float HealthRemainingPercentage => ((float)Health) / MaxHealth;

        protected HealthCube(CubeTemplate template, CubeMode blockType) : base(template,blockType)
        {
            Health = MaxHealth;
        }


        public override void DealDamage(int amount)
        {
            if(Dormant)
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
            ColorMask = (128, 128, 128, 255);
        }

        public override bool ToBeDeleted() => (Dormant & !Active);
        public override bool CanUpdate => !Dormant;
    }
}
