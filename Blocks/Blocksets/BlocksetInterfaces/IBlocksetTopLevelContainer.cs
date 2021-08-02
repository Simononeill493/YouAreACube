using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public interface IBlocksetTopLevelContainer
    {
        Blockset CreateBlockset(string name);

        void OpenInputSubMenu(InputOptionMenu menu, BlockInputSection section);
    }

    public class DummyBlocksetContainer : IBlocksetTopLevelContainer
    {
        public Blockset CreateBlockset(string name)
        {
            return new Blockset(name, ManualDrawLayer.Zero, 1.0f, (a1, a2, a3) => { });
        }

        public void OpenInputSubMenu(InputOptionMenu menu, BlockInputSection section) => throw new NotImplementedException();
    }

}
