using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace IAmACube
{
    class KeyUtils
    {
        public static bool IsAlphabetical(Keys key)
        {
            return (key >= Keys.A && key <= Keys.Z);
        }

        public static bool IsNumeric(Keys key)
        {
            return (key >= Keys.D0 && key <= Keys.D9);
        }

        public static bool IsAlphanumeric(Keys key)
        {
            return IsAlphabetical(key) | IsNumeric(key);
        }

        public static bool IsTypeable(Keys key)
        {
            return IsAlphanumeric(key) | key.Equals(Keys.OemMinus);
        }


        public static char KeyToChar(Keys key)
        {
            var keyboard = Keyboard.GetState();
            var caps = CapsLock;
            var shift = keyboard.IsKeyDown(Keys.LeftShift);

            if(key == Keys.OemMinus)
            {
                return '-';
            }
            var str = key.ToString();
            if((caps & !shift)|(!caps & shift))
            {
                str = str.ToUpper();
            }
            else
            {
                str = str.ToLower();
            }

            if (IsAlphabetical(key))
            {
                return str[0];
            }
            else if(IsNumeric(key))
            {
                return str[1];
            }
            else
            {
                return str[0];
            }
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        public static extern short GetKeyState(int keyCode);
        public static bool CapsLock => (((ushort)GetKeyState(0x14)) & 0xffff) != 0;
        public static bool NumLock => (((ushort)GetKeyState(0x90)) & 0xffff) != 0;
        public static bool ScrollLock => (((ushort)GetKeyState(0x91)) & 0xffff) != 0;
        public static bool Shift => (((ushort)GetKeyState(0x74)) & 0xffff) != 0;
    }
}
