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
        public static IntPoint GetWASDDirection(UserInput input)
        {
            if (input.IsKeyDown(Keys.W))//up
            {
                return IntPoint.Up;
            }
            if (input.IsKeyDown(Keys.S))//down
            {
                return IntPoint.Down;
            }
            if (input.IsKeyDown(Keys.A))//left
            {
                return IntPoint.Left;
            }
            if (input.IsKeyDown(Keys.D))//right
            {
                return IntPoint.Right;
            }

            return IntPoint.Zero;
        }
        public static IntPoint GetRightKeypadDirection(UserInput input)
        {
            if (input.IsKeyDown(Keys.Home))//up
            {
                return IntPoint.Up;
            }
            if (input.IsKeyDown(Keys.End))//down
            {
                return IntPoint.Down;
            }
            if (input.IsKeyDown(Keys.Delete))//left
            {
                return IntPoint.Left;
            }
            if (input.IsKeyDown(Keys.PageDown))//right
            {
                return IntPoint.Right;
            }

            return IntPoint.Zero;
        }


        public static char KeyToChar(Keys key)
        {
            var keyboard = Keyboard.GetState();
            var caps = CapsLock;
            var shift = keyboard.IsKeyDown(Keys.LeftShift);

            if (key == Keys.OemMinus)
            {
                return '-';
            }

            var str = key.ToString();
            if ((caps & !shift) | (!caps & shift))
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
            else if (IsNumeric(key))
            {
                return str[1];
            }
            else
            {
                return str[0];
            }
        }
        public static bool IsAlphabetical(Keys key) => (key >= Keys.A && key <= Keys.Z);
        public static bool IsNumeric(Keys key) => (key >= Keys.D0 && key <= Keys.D9);
        public static bool IsAlphanumeric(Keys key) => IsAlphabetical(key) | IsNumeric(key);
        public static bool IsTypeable(Keys key) => IsAlphanumeric(key) | key.Equals(Keys.OemMinus);



        public static bool CapsLock => (((ushort)GetKeyState(0x14)) & 0xffff) != 0;
        public static bool NumLock => (((ushort)GetKeyState(0x90)) & 0xffff) != 0;
        public static bool ScrollLock => (((ushort)GetKeyState(0x91)) & 0xffff) != 0;

        [DllImport("user32.dll", CharSet = CharSet.Auto, ExactSpelling = true, CallingConvention = CallingConvention.Winapi)]
        public static extern short GetKeyState(int keyCode);

    }

}
