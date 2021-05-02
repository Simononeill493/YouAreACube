using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    partial class SurfaceBlock
    {
        public int Health { get; private set; }
        public int MaxHealth => Template.MaxHealth;
        public float HealthRemainingPercentage => ((float)Health) / MaxHealth;


        public void DealDamage(int amount)
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
    }
}
