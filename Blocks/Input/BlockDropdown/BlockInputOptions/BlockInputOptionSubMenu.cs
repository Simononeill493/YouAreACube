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

        public InputOptionMenu MenuToOpen;

        public BlockInputOptionSubMenu(string name,InputOptionMenu menuToOpen) : base(InputOptionType.SubMenu)
        {
            Name = name;
            MenuToOpen = menuToOpen;
        }

        public override string ToString() => Name;
    }

    public enum InputOptionMenu
    {
        CubeTemplate
    }
}
