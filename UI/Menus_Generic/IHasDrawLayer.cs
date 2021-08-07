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

        public static ManualDrawLayer Zero => new ManualDrawLayer(0.0f);
        public static ManualDrawLayer Dialog => new ManualDrawLayer(DrawLayers.MenuDialogLayer);
        public static ManualDrawLayer Dropdown => new ManualDrawLayer(DrawLayers.MenuDropdownLayer);

        public static ManualDrawLayer Create(float layer) => new ManualDrawLayer(layer);
        public static ManualDrawLayer InFrontOf(IHasDrawLayer parent,int steps = 1) => new ManualDrawLayer(parent.DrawLayer - ((DrawLayers.MinLayerDistance)*steps));
    
    
    
    }
}
