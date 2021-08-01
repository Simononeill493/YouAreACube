using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public interface IBlocksetContainer
    {
        void AddBlockset(Blockset blockset);
        void RemoveBlockset(Blockset blockset);
    }
}
