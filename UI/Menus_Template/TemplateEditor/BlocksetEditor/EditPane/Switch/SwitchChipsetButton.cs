using Microsoft.Xna.Framework;

namespace IAmACube
{
    public class SwitchChipsetButton : SpriteMenuItem
    {
        public int Index;
        public bool ButtonCurrentlyActive;
        public SwitchChipsetButton(IHasDrawLayer parent,int index) : base(parent, BuiltInMenuSprites.IfBlockSwitchButton) 
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
