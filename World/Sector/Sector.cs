﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    [Serializable()]
    public class Sector
    {
        public Tile[,] Tiles;

        public Sector(Tile[,] tiles)
        {
            Tiles = tiles;
        }

    }
}
