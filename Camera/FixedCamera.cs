using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class FixedCamera : Camera
    {
        public FixedCamera(Kernel kernel) : base(kernel) { }

        protected override void _update(UserInput input) { }

    }
}
