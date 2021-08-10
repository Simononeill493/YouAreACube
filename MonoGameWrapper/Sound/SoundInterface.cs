using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace IAmACube
{
    class SoundInterface
    {
        public static bool Initialized;

        public static ContentManager content;
        public static SoundEffect wind;
        public static SoundEffect pop1;
        public static SoundEffect pop2;
        public static SoundEffect pop3;

        public static SoundEffect[] pops;

        public static void LoadContent(ContentManager contentManager)
        {
            try
            {
                content = contentManager;

                wind = contentManager.Load<SoundEffect>("Sounds/wind");
                pops = new SoundEffect[3];

                pop1 = contentManager.Load<SoundEffect>("Sounds/pop1");
                pop2 = contentManager.Load<SoundEffect>("Sounds/pop2");
                pop3 = contentManager.Load<SoundEffect>("Sounds/pop3");

                pops[0] = pop1;
                pops[1] = pop2;
                pops[2] = pop3;

                Initialized = true;
            }
            catch(NoAudioHardwareException e)
            {
                Console.WriteLine("No sound device detected. Sounds will not play.");
            }
        }

        public static void PlayWind()
        {
            if(!Initialized) { return; }

            var instance = wind.CreateInstance();
            instance.IsLooped = true;
            instance.Volume = 0.25f;
            instance.Play();            
        }

        public static void PlayPop()
        {
            if (!Initialized) { return; }

            pop1.Play();
        }

    }
}
