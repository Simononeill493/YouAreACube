using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class BlockSwitchSection_2 : SpriteMenuItem
    {
        public BlockSwitchSection_2(IHasDrawLayer parent) : base(parent, BuiltInMenuSprites.BlockBottom)
        {
            var leftButton = new SpriteMenuItem(this, BuiltInMenuSprites.IfBlockSwitchButton);
            leftButton.SetLocationConfig(0, 0, CoordinateMode.ParentPixelOffset);
            //leftButton.OnMouseReleased += (i) => _buttonClicked(leftButton);
            AddChild(leftButton);

            var rightButton = new SpriteMenuItem(this, BuiltInMenuSprites.IfBlockSwitchButton);
            rightButton.SetLocationConfig(70, 0, CoordinateMode.ParentPixelOffset);
            //rightButton.OnMouseReleased += (i) => _buttonClicked(rightButton);
            AddChild(rightButton);

            var switchArrowButtonLeft = new SpriteMenuItem(this, BuiltInMenuSprites.SwitchBlockSideArrow);
            switchArrowButtonLeft.SetLocationConfig(140, 0, CoordinateMode.ParentPixelOffset);
            //switchArrowButtonLeft.OnMouseReleased += (i) => { UpdateButtonOffset(-1); };
            AddChild(switchArrowButtonLeft);

            var switchArrowButtonRight = new SpriteMenuItem(this, BuiltInMenuSprites.SwitchBlockSideArrow);
            switchArrowButtonRight.FlipHorizontal = true;
            switchArrowButtonRight.SetLocationConfig(140 + switchArrowButtonLeft.GetBaseSize().X, 0, CoordinateMode.ParentPixelOffset);
            //switchArrowButtonRight.OnMouseReleased += (i) => { UpdateButtonOffset(1); };
            AddChild(switchArrowButtonRight);

        }
    }
}
