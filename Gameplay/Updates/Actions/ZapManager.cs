using System;

namespace IAmACube
{
    [Serializable()]
    public class ZapManager
    {
        public void TryZap(Cube actor, CubeMode blockType)
        {
            ZapUtils.TryZap(actor, blockType);
        }
    }
}