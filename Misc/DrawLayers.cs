using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class DrawLayers
    {
        public const float MinLayerDistance = 0.0001f;

        public const float BackgroundLayer = 1;

        public const float GroundLayer = 0.9f;
        public const float SurfaceLayer = 0.8f;
        public const float EphemeralLayer = 0.7f;
        public const float HUDLayer = 0.6f;

        public const float MenuBehindLayer = 0.5f;
        public const float MenuBaseLayer = 0.4f;

        public const float MenuDropdownLayer = 0.3f;
        public const float MenuHoverLayer = 0.2f;


        public const float MenuDragOffset = 0.3f;
    }
}
