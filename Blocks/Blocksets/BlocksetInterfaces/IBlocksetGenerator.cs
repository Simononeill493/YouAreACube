using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public interface IBlocksetGenerator
    {
        Blockset CreateBlockset(string name);
    }

    public class DummyBlocksetGenerator : IBlocksetGenerator
    {
        public Blockset CreateBlockset(string name) => new Blockset(name, ManualDrawLayer.Zero, 1.0f, (a1, a2, a3) => { });
    }

}
