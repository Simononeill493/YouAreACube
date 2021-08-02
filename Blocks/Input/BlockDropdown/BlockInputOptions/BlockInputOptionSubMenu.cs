using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockInputOptionSubMenu : BlockInputOption
    {
        public string Name;
        public override string BaseType => null;

        private Action _onOptionClicked;

        public BlockInputOptionSubMenu(string name, Action onOptionClicked) : base(InputOptionType.SubMenu)
        {
            Name = name;
            _onOptionClicked = onOptionClicked;
        }

        public override void OptionClicked() => _onOptionClicked();

        public override string ToString() => Name;

    }
}
