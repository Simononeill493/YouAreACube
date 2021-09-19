using System;

namespace IAmACube
{
    public abstract class MenuItemScaleProvider
    {
        public abstract int GetScale(MenuItem item);

        public virtual float Multiplier => _manualMultiplier;
        private float _manualMultiplier = 1.0f;

        public void MultiplyManualScale(float multiplier)
        {
            if (Math.Abs(multiplier) < 0.0001f)
            {
                throw new Exception("Scale should never be mutiplied by zero");

            }
            _manualMultiplier *= multiplier;
        }

        public void SetManualScale(float toSetTo)
        {
            if (Math.Abs(toSetTo) < 0.0001f)
            {
                throw new Exception("Scale should never be set to zero");

            }
            _manualMultiplier = toSetTo;
        }

    }
}