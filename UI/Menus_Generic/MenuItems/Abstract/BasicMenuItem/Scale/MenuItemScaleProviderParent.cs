namespace IAmACube
{
    public class MenuItemScaleProviderParent : MenuItemScaleProvider
    {
        public override int GetScale(MenuItem item) => (item._parent == null) ? 1 : (int)(item._parent.Scale * Multiplier);
    }
}