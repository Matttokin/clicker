using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace clicker
{

    static class mouseClick
    {
        [DllImport("user32.dll", CharSet = CharSet.Auto, CallingConvention = CallingConvention.StdCall)]
        public static extern void mouse_event(uint dwFlags, uint dx, uint dy, uint dwData, UIntPtr dwExtraInfo);

        [Flags]
        public enum MouseEventFlags : uint
        {
            LEFTDOWN = 0x00000002,
            LEFTUP = 0x00000004,
            MIDDLEDOWN = 0x00000020,
            MIDDLEUP = 0x00000040,
            MOVE = 0x00000001,
            ABSOLUTE = 0x00008000,
            RIGHTDOWN = 0x00000008,
            RIGHTUP = 0x00000010
        }
        private static bool left = true;

        public static bool Left { get => left; set => left = value; }

        public static void DoMouseClick(uint X, uint Y)
        {
            if (left)
            {
                mouse_event((uint)(MouseEventFlags.LEFTDOWN | MouseEventFlags.LEFTUP), X, Y, 0, UIntPtr.Zero);
            } else
            {
                mouse_event((uint)(MouseEventFlags.RIGHTDOWN | MouseEventFlags.RIGHTUP), X, Y, 0, UIntPtr.Zero);
            }
        }
    }
}


