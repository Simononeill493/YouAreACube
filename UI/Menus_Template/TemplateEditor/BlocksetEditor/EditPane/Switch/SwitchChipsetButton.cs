using Microsoft.Xna.Framework;

namespace IAmACube
{
    class SwitchChipsetButton : SpriteScreenItem
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
            Color = Color.LightGray;
        }

        public void Deactivate()
        {
            ButtonCurrentlyActive = false;
            Color = Color.White;
        }
    }
}
