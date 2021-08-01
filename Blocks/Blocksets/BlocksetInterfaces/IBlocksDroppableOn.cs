using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public interface IBlocksDroppableOn
    {
        void DropBlocksOnThis(List<BlockTop> blocks, UserInput input);
    }
}
