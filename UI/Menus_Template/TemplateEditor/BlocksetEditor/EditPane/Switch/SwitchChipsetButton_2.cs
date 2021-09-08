using Microsoft.Xna.Framework;

namespace IAmACube
{
    public class SwitchChipsetButton_2 : SpriteMenuItem
    {
        public int Index;
        public bool ButtonCurrentlyActive;
        public SwitchChipsetButton_2(IHasDrawLayer parent,int index) : base(parent, BuiltInMenuSprites.IfBlockSwitchButton) 
        {
            Index = index;
        }

        public void Activate()
        {
            ButtonCurrentlyActive = true;
            ColorMask = Color.LightGray;
        }

        public void Deactivate()
        {
            ButtonCurrentlyActive = false;
            ColorMask = Color.White;
        }
    }
}
