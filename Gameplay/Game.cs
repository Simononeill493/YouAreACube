using System;
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
        private EffectManager _effectManager;

        public Game(Save save)
        {
            _save = save;
            _tickCounter = new TickCounter();
            _effectManager = new EffectManager();
        }

        public void Update(UserInput input)
        {
            var effects = World.Update(input, _tickCounter);
            _effectManager.ProcessEffects(effects);

            _tickCounter.Tick();
        }
    }
}
