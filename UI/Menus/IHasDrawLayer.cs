using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public interface IHasDrawLayer
    {
        float DrawLayer { get; }
    }

    public class ManualDrawLayer : IHasDrawLayer
    {
        public float DrawLayer { get; }

        private ManualDrawLayer(float drawLayer ) { DrawLayer = drawLayer; }

        public static ManualDrawLayer Create(float layer) => new ManualDrawLayer(layer);

        public static ManualDrawLayer InFrontOf(IHasDrawLayer parent,int steps) => new ManualDrawLayer(parent.DrawLayer - ((DrawLayers.MinLayerDistance)*steps));
    }
}
