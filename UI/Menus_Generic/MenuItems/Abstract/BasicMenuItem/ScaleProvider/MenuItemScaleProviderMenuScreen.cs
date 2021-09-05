namespace IAmACube
{
    public class MenuItemScaleProviderMenuScreen : MenuItemScaleProvider
    {
        public override int GetScale(MenuItem item) => (int)(MenuScreen.Scale * Multiplier);
    }
}