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
        private MoveManager _moveManager;

        public Game(Save save)
        {
            _save = save;
            _tickCounter = new TickCounter();
            _moveManager = new MoveManager();
        }

        public void Update(UserInput input)
        {
            var effects = World.Update(input, _tickCounter);
            _moveManager.ProcessMoves(effects.ToMove);

            _tickCounter.Tick();
        }
    }
}
