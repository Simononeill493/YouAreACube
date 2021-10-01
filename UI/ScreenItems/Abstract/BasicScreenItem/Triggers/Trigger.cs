using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    public abstract class Trigger
    {
        public abstract bool Check();
    }

    class InstantTrigger : Trigger
    {
        public override bool Check() => true;
    }

    class TimedTrigger : Trigger
    {
        private int _frames;
        public TimedTrigger(int frames)
        {
            _frames = frames;
        }

        public override bool Check()
        {
            if(_frames<1)
            {
                return true;
            }

            _frames--;
            return false;
        }
    }


    static class Triggers
    {
        public static InstantTrigger Instant = new InstantTrigger();
        public static TimedTrigger Timed(int frames) => new TimedTrigger(frames);
    }

}
