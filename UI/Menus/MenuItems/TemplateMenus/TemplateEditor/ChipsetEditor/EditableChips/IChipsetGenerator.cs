using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public interface IChipsetGenerator
    {
        Blockset CreateChipset(string name);
    }

    public class DummyChipsetGenerator : IChipsetGenerator
    {
        public Blockset CreateChipset(string name) => new Blockset(name, ManualDrawLayer.Zero, 1.0f, (a1, a2, a3) => { });
    }

}
