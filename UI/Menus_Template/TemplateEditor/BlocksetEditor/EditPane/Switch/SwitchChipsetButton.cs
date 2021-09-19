using Microsoft.Xna.Framework;

namespace IAmACube
{
    public class SwitchChipsetButton : SpriteScreenItem
    {
        public int Index;
        public bool ButtonCurrentlyActive;
        public SwitchChipsetButton(IHasDrawLayer parent,int index) : base(parent, MenuSprites.IfBlockSwitchButton) 
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
