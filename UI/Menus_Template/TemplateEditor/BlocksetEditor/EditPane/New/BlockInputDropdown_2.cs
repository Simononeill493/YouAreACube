using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockInputDropdown_2 : DropdownMenuItem<BlockInputOption_2>
    {
        public BlockInputModel Model;
        public BlockModel Block;

        public List<string> InputTypes;

        public override bool Dropped
        {
            get { return _dropped; }
            set
            {
                if (value) { PopulateItems(); }
                base.Dropped = value;
            }
        }

        public BlockInputDropdown_2(IHasDrawLayer parent, List<string> inputTypes,BlockInputModel model) : base(parent)
        {
            InputTypes = inputTypes;
            Model = model;
        }

        public void PopulateItems()
        {
            ClearItems();
            AddItems(BlocksetEditPane_2.Model.GetInputsFromModel(Block, InputTypes));
            AddItems(BlockDropdownUtils.GetDefaultItems(InputTypes));
        }
    }
}
