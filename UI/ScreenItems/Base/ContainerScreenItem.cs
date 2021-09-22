using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class ContainerScreenItem : ScreenItem
    {
        public override IntPoint GetBaseSize() => IntPoint.One;

        public ContainerScreenItem(IHasDrawLayer parentDrawLayer) : base(parentDrawLayer) { }
    }
}
