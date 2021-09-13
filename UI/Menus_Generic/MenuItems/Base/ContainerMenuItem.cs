using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ContainerMenuItem : MenuItem
    {
        public override IntPoint GetBaseSize() => IntPoint.One;

        public ContainerMenuItem(IHasDrawLayer parentDrawLayer) : base(parentDrawLayer) { }
    }
}
