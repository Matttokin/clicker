using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace clicker
{
    class hook
    {
        private const int WH_KEYBOARD_LL = 13;

        private LowLevelKeyboardProcDelegate m_callback;

        private IntPtr m_hHook;

        private bool needStart = true;
        private bool needSetSetting = true;
        private Timer timerClicker;

        Label statusLabel; 
        Label countClick;
        Button saveSettingButton;

        public bool NeedSetSetting { get => needSetSetting; set => needSetSetting = value; }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProcDelegate lpfn, IntPtr hMod, int dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("Kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetModuleHandle(IntPtr lpModuleName);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        public hook(Label statusLabel, Label countClick,Button saveSettingButton)
        {
            this.statusLabel = statusLabel;
            this.countClick = countClick;
            this.saveSettingButton = saveSettingButton;
        }
        public void createTimer(bool left,int time)
        {
            mouseClick.Left = left;
            timerClicker = new Timer();
            timerClicker.Interval = time;
            timerClicker.Tick += new EventHandler(timerClicker_Tick);
        }
        void timerClicker_Tick(object sender, EventArgs e)
        {
            uint X = (uint)Cursor.Position.X;
            uint Y = (uint)Cursor.Position.Y;
            mouseClick.DoMouseClick(X, Y);
            int countClickInt = Convert.ToInt32(countClick.Text);
            countClickInt++;
            countClick.Text = countClickInt.ToString();
        }
        private IntPtr LowLevelKeyboardHookProc(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode < 0)
            {
                return CallNextHookEx(m_hHook, nCode, wParam, lParam);
            }
            else
            {
                var khs = (KeyboardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyboardHookStruct));
                Debug.Print("Hook: Code: {0}, WParam: {1},{2},{3},{4} ", nCode, wParam, lParam, khs.VirtualKeyCode, khs.ScanCode, khs.Flags, khs.Time);
                Debug.Print(khs.VirtualKeyCode.ToString());
                if (khs.VirtualKeyCode == 116 && wParam.ToInt32() == 260 && khs.ScanCode == 63) //Q
                {
                    if (!needSetSetting)
                    {
                        if (needStart)
                        {
                            countClick.Text = "0";
                            timerClicker.Start();
                            statusLabel.Text = "Запущен";
                            statusLabel.ForeColor = Color.Green;
                            saveSettingButton.Enabled = false;
                        }
                        else
                        {
                            timerClicker.Stop();
                            statusLabel.Text = "Остановлен";
                            statusLabel.ForeColor = Color.Orange;
                            saveSettingButton.Enabled = true;
                        }

                        needStart = !needStart;
                    } else
                    {
                        MessageBox.Show("Настройки не установлены!");
                    }

                    IntPtr val = new IntPtr(1);
                    return val;
                }

                return CallNextHookEx(m_hHook, nCode, wParam, lParam);
            }
        }

        [StructLayout(LayoutKind.Sequential)]

        private struct KeyboardHookStruct
        {
            public readonly int VirtualKeyCode;
            public readonly int ScanCode;
            public readonly int Flags;
            public readonly int Time;
            public readonly IntPtr ExtraInfo;

        }

        private delegate IntPtr LowLevelKeyboardProcDelegate(int nCode, IntPtr wParam, IntPtr lParam);
        public void SetHook()
        {
            m_callback = LowLevelKeyboardHookProc;
            m_hHook = SetWindowsHookEx(WH_KEYBOARD_LL, m_callback, GetModuleHandle(IntPtr.Zero), 0);
        }

        public void Unhook()
        {
            UnhookWindowsHookEx(m_hHook);
        }
    }
}
