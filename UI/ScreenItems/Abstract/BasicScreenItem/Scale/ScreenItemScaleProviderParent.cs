namespace IAmACube
{
    public class ScreenItemScaleProviderParent : ScreenItemScaleProvider
    {
        public override int GetScale(ScreenItem item) => (item._parent == null) ? 1 : (int)(item._parent.Scale * Multiplier);
    }
}