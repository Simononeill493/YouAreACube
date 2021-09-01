using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public interface IBlocksetTopLevelContainer : IBlocksetGenerator
    {
        void OpenInputSubMenu(InputOptionMenu menu, BlockInputSection section);
    }

    public interface IBlocksetGenerator
    {
        Blockset CreateBlockset(string name);
    }

    public class DummyBlocksetContainer: IBlocksetTopLevelContainer
    {
        public Blockset CreateBlockset(string name)
        {
            return new Blockset(name, ManualDrawLayer.Zero, 1.0f, (a1, a2, a3) => { },null);
        }

        public void OpenInputSubMenu(InputOptionMenu menu, BlockInputSection section) => throw new NotImplementedException();
    }
}
