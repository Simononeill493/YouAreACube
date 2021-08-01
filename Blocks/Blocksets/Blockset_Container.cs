using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public partial class Blockset
    {
        private IBlocksetContainer _currentContainer;

        public void SetContainer(IBlocksetContainer newContainer)
        {
            if (newContainer == _currentContainer) { return; }

            ClearContainer();

            newContainer.AddBlockset(this);
            _currentContainer = newContainer;
        }
        public void ClearContainer()
        {
            if (_currentContainer != null)
            {
                _currentContainer.RemoveBlockset(this);
            }

            _currentContainer = null;
        }
        public override void Dispose()
        {
            if (_currentContainer != null)
            {
                throw new Exception("Tried to dispose a chipset that is still contained!");
            }

            base.Dispose();
        }
    }
}
