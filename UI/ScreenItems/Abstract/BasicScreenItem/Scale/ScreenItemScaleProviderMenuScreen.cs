﻿namespace IAmACube
{
    public class ScreenItemScaleProviderMenuScreen : ScreenItemScaleProvider
    {
        public override int GetScale(ScreenItem item) => (int)(MenuScreen.Scale * Multiplier);
    }
}