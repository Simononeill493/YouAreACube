﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class Game
    {
        public World World => _save.World;
        private Save _save;

        private TickCounter _tickCounter;
        private WorldUpdater _updater;

        public Game(Save save)
        {
            _save = save;
            _tickCounter = new TickCounter();
            _updater = new WorldUpdater();
        }

        public void Update(UserInput input)
        {
            _updater.UpdateWorld(World, input, _tickCounter);
            _tickCounter.Tick();
        }
    }
}
