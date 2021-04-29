using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public interface IChipsetGenerator
    {
        EditableChipset CreateChipset(string name);
    }

    public class DummyChipsetGenerator : IChipsetGenerator
    {
        public EditableChipset CreateChipset(string name) => new EditableChipset(name, ManualDrawLayer.Zero, 1.0f, (a1, a2, a3) => { });
    }

}
