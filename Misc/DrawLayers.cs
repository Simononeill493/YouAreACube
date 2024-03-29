﻿using System;
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
        public const float BackgroundDecorationLayer = 0.99f;

        public const float TileLayer = 0.95f;
        public const float GroundLayer = 0.9f;
        public const float SurfaceLayer = 0.8f;
        public const float EphemeralLayer = 0.7f;
        public const float MouseTileHoverLayer = 0.69f;

        public const float BlockInfoLayer_Back = 0.67f;
        public const float BlockInfoLayer_Front = 0.66f;

        public const float GameSectorOverlayLayer = 0.65f;
        public const float GameTileDebugLayer = 0.64f;

        public const float HUDLayer = 0.6f;


        //public const float MenuBehindLayer = 0.5f;
        public const float MenuBlockLayer = 0.45f;

        public const float MenuBaseLayer = 0.4f;
        public const float MenuDialogLayer = 0.35f;

        public const float MenuDropdownLayer = 0.02f;
        public const float MenuHoverLayer = 0.01f;

        public const float MenuDragOffset = 0.4f;
    }
}
