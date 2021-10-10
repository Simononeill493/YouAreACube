namespace IAmACube
{
    class ScreenItemScaleProviderMenuScreen : ScreenItemScaleProvider
    {
        public override int GetScale(ScreenItem item) => (int)(Screen.UniversalScale * Multiplier);
    }
}