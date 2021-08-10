using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public abstract partial class BlockInputSection : SpriteMenuItem
    {
        public Action<BlockInputSection, BlockInputOption> ItemSelectedCallback;
        public void ItemSelected(BlockInputOption optionSelected) => ItemSelectedCallback(this, optionSelected);

        public List<string> InputBaseTypes;

        public BlockInputSection(IHasDrawLayer parent,List<string> inputTypes,string inputDisplayName) : base(parent, BuiltInMenuSprites.BlockMiddle) 
        {
            InputBaseTypes = inputTypes;

            var textItem = _addTextItem(inputDisplayName, 4, 40, CoordinateMode.ParentPercentageOffset, false);
            textItem.MultiplyScale(0.5f);
            textItem.Color = Color.White;
        }

        public abstract BlockInputOption CurrentlySelected { get; }

        public void ManuallySetInput(BlockInputOption option)
        {
            _manuallySetInput(option);
            ItemSelected(option);
        }
        protected abstract void _manuallySetInput(BlockInputOption option);

        
        public abstract void SetConnectionsFromAbove(List<BlockTop> chipsAbove);
        public abstract void RefreshText();

        protected List<BlockInputOption> _getValidInputsFromAbove(List<BlockTop> chipsAbove)
        {
            var output = new List<BlockInputOption>();
            foreach (var chip in TemplateEditUtils.GetChipsWithOutput(chipsAbove))
            {
                if(_isValidInput(chip))
                {
                    output.Add(new BlockInputOptionReference(chip));
                }
            }

            return output;
        }

        protected bool _isValidInput(BlockTopWithOutput chipAbove)
        {
            var chipAboveOutput = chipAbove.OutputTypeCurrent;
            foreach(var inputType in InputBaseTypes)
            {
                if(TemplateEditUtils.IsValidInputFor(chipAboveOutput,inputType))
                {
                    return true;
                }
            }

            return false;
        }

    }
}
