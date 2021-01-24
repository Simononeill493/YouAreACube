using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public class EffectsList
    {
        public List<Tuple<Block, Direction>> ToMove = new List<Tuple<Block, Direction>>();

        public void StartMove(Block block,Direction direction)
        {
            ToMove.Add(new Tuple<Block, Direction>(block, direction));
        }
    }
}
