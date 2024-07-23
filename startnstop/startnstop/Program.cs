using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Xml.Linq;

namespace startnstop
{
    internal class Program
    {
        const UInt32 WM_KEYDOWN = 0x0100;
        const UInt32 WM_CHAR = 0x102;
        const UInt32 WM_SETTEXT = 0xC;
        const UInt32 WM_KEYUP = 0x101;
        const UInt32 WM_LBUTTONDOWN = 0x201;
        const UInt32 WM_LBUTTONUP = 0x202;
        const UInt32 WM_CLOSE = 0x10;
        const UInt32 WM_COMMAND = 0x111;
        const UInt32 WM_CLEAR = 0x303;
        const UInt32 WM_DESTROY = 0x2;
        const UInt32 WM_GETTEXT = 0xD;
        const UInt32 WM_GETTEXTLENGTH = 0xE;
        const UInt32 WM_LBUTTONDBLCLK = 0x203;
        const int VK_ABNT_C1 = 0xC1;
        const int VK_ABNT_C2 = 0xC2;
        const int VK_ADD = 0x6B;
        const int VK_ATTN = 0xF6;
        const int VK_BACK = 0x08;
        const int VK_CANCEL = 0x03;
        const int VK_CLEAR = 0x0C;
        const int VK_CRSEL = 0xF7;
        const int VK_DECIMAL = 0x6E;
        const int VK_DIVIDE = 0x6F;
        const int VK_EREOF = 0xF9;
        const int VK_ESCAPE = 0x1B;
        const int VK_EXECUTE = 0x2B;
        const int VK_EXSEL = 0xF8;
        const int VK_ICO_CLEAR = 0xE6;
        const int VK_ICO_HELP = 0xE3;
        const int VK_KEY_0 = 0x30;
        const int VK_KEY_1 = 0x31;
        const int VK_KEY_2 = 0x32;
        const int VK_KEY_3 = 0x33;
        const int VK_KEY_4 = 0x34;
        const int VK_KEY_5 = 0x35;
        const int VK_KEY_6 = 0x36;
        const int VK_KEY_7 = 0x37;
        const int VK_KEY_8 = 0x38;
        const int VK_KEY_9 = 0x39;
        const int VK_KEY_A = 0x41;
        const int VK_KEY_B = 0x42;
        const int VK_KEY_C = 0x43;
        const int VK_KEY_D = 0x44;
        const int VK_KEY_E = 0x45;
        const int VK_KEY_F = 0x46;
        const int VK_KEY_G = 0x47;
        const int VK_KEY_H = 0x48;
        const int VK_KEY_I = 0x49;
        const int VK_KEY_J = 0x4A;
        const int VK_KEY_K = 0x4B;
        const int VK_KEY_L = 0x4C;
        const int VK_KEY_M = 0x4D;
        const int VK_KEY_N = 0x4E;
        const int VK_KEY_O = 0x4F;
        const int VK_KEY_P = 0x50;
        const int VK_KEY_Q = 0x51;
        const int VK_KEY_R = 0x52;
        const int VK_KEY_S = 0x53;
        const int VK_KEY_T = 0x54;
        const int VK_KEY_U = 0x55;
        const int VK_KEY_V = 0x56;
        const int VK_KEY_W = 0x57;
        const int VK_KEY_X = 0x58;
        const int VK_KEY_Y = 0x59;
        const int VK_KEY_Z = 0x5A;
        const int VK_MULTIPLY = 0x6A;
        const int VK_NONAME = 0xFC;
        const int VK_NUMPAD0 = 0x60;
        const int VK_NUMPAD1 = 0x61;
        const int VK_NUMPAD2 = 0x62;
        const int VK_NUMPAD3 = 0x63;
        const int VK_NUMPAD4 = 0x64;
        const int VK_NUMPAD5 = 0x65;
        const int VK_NUMPAD6 = 0x66;
        const int VK_NUMPAD7 = 0x67;
        const int VK_NUMPAD8 = 0x68;
        const int VK_NUMPAD9 = 0x69;
        const int VK_OEM_1 = 0xBA;
        const int VK_OEM_102 = 0xE2;
        const int VK_OEM_2 = 0xBF;
        const int VK_OEM_3 = 0xC0;
        const int VK_OEM_4 = 0xDB;
        const int VK_OEM_5 = 0xDC;
        const int VK_OEM_6 = 0xDD;
        const int VK_OEM_7 = 0xDE;
        const int VK_OEM_8 = 0xDF;
        const int VK_OEM_ATTN = 0xF0;
        const int VK_OEM_AUTO = 0xF3;
        const int VK_OEM_AX = 0xE1;
        const int VK_OEM_BACKTAB = 0xF5;
        const int VK_OEM_CLEAR = 0xFE;
        const int VK_OEM_COMMA = 0xBC;
        const int VK_OEM_COPY = 0xF2;
        const int VK_OEM_CUSEL = 0xEF;
        const int VK_OEM_ENLW = 0xF4;
        const int VK_OEM_FINISH = 0xF1;
        const int VK_OEM_FJ_LOYA = 0x95;
        const int VK_OEM_FJ_MASSHOU = 0x93;
        const int VK_OEM_FJ_ROYA = 0x96;
        const int VK_OEM_FJ_TOUROKU = 0x94;
        const int VK_OEM_JUMP = 0xEA;
        const int VK_OEM_MINUS = 0xBD;
        const int VK_OEM_PA1 = 0xEB;
        const int VK_OEM_PA2 = 0xEC;
        const int VK_OEM_PA3 = 0xED;
        const int VK_OEM_PERIOD = 0xBE;
        const int VK_OEM_PLUS = 0xBB;
        const int VK_OEM_RESET = 0xE9;
        const int VK_OEM_WSCTRL = 0xEE;
        const int VK_PA1 = 0xFD;
        const int VK_PACKET = 0xE7;
        const int VK_PLAY = 0xFA;
        const int VK_PROCESSKEY = 0xE5;
        const int VK_RETURN = 0x0D;
        const int VK_SELECT = 0x29;
        const int VK_SEPARATOR = 0x6C;
        const int VK_SPACE = 0x20;
        const int VK_SUBTRACT = 0x6D;
        const int VK_TAB = 0x09;
        const int VK_ZOOM = 0xFB;
        const int VK__none_ = 0xFF;
        const int VK_ACCEPT = 0x1E;
        const int VK_APPS = 0x5D;
        const int VK_BROWSER_BACK = 0xA6;
        const int VK_BROWSER_FAVORITES = 0xAB;
        const int VK_BROWSER_FORWARD = 0xA7;
        const int VK_BROWSER_HOME = 0xAC;
        const int VK_BROWSER_REFRESH = 0xA8;
        const int VK_BROWSER_SEARCH = 0xAA;
        const int VK_BROWSER_STOP = 0xA9;
        const int VK_CAPITAL = 0x14;
        const int VK_CONVERT = 0x1C;
        const int VK_DELETE = 0x2E;
        const int VK_DOWN = 0x28;
        const int VK_END = 0x23;
        const int VK_F1 = 0x70;
        const int VK_F10 = 0x79;
        const int VK_F11 = 0x7A;
        const int VK_F12 = 0x7B;
        const int VK_F13 = 0x7C;
        const int VK_F14 = 0x7D;
        const int VK_F15 = 0x7E;
        const int VK_F16 = 0x7F;
        const int VK_F17 = 0x80;
        const int VK_F18 = 0x81;
        const int VK_F19 = 0x82;
        const int VK_F2 = 0x71;
        const int VK_F20 = 0x83;
        const int VK_F21 = 0x84;
        const int VK_F22 = 0x85;
        const int VK_F23 = 0x86;
        const int VK_F24 = 0x87;
        const int VK_F3 = 0x72;
        const int VK_F4 = 0x73;
        const int VK_F5 = 0x74;
        const int VK_F6 = 0x75;
        const int VK_F7 = 0x76;
        const int VK_F8 = 0x77;
        const int VK_F9 = 0x78;
        const int VK_FINAL = 0x18;
        const int VK_HELP = 0x2F;
        const int VK_HOME = 0x24;
        const int VK_ICO_00 = 0xE4;
        const int VK_INSERT = 0x2D;
        const int VK_JUNJA = 0x17;
        const int VK_KANA = 0x15;
        const int VK_KANJI = 0x19;
        const int VK_LAUNCH_APP1 = 0xB6;
        const int VK_LAUNCH_APP2 = 0xB7;
        const int VK_LAUNCH_MAIL = 0xB4;
        const int VK_LAUNCH_MEDIA_SELECT = 0xB5;
        const int VK_LBUTTON = 0x01;
        const int VK_LCONTROL = 0xA2;
        const int VK_LEFT = 0x25;
        const int VK_LMENU = 0xA4;
        const int VK_LSHIFT = 0xA0;
        const int VK_LWIN = 0x5B;
        const int VK_MBUTTON = 0x04;
        const int VK_MEDIA_NEXT_TRACK = 0xB0;
        const int VK_MEDIA_PLAY_PAUSE = 0xB3;
        const int VK_MEDIA_PREV_TRACK = 0xB1;
        const int VK_MEDIA_STOP = 0xB2;
        const int VK_MODECHANGE = 0x1F;
        const int VK_NEXT = 0x22;
        const int VK_NONCONVERT = 0x1D;
        const int VK_NUMLOCK = 0x90;
        const int VK_OEM_FJ_JISHO = 0x92;
        const int VK_PAUSE = 0x13;
        const int VK_PRINT = 0x2A;
        const int VK_PRIOR = 0x21;
        const int VK_RBUTTON = 0x02;
        const int VK_RCONTROL = 0xA3;
        const int VK_RIGHT = 0x27;
        const int VK_RMENU = 0xA5;
        const int VK_RSHIFT = 0xA1;
        const int VK_RWIN = 0x5C;
        const int VK_SCROLL = 0x91;
        const int VK_SLEEP = 0x5F;
        const int VK_SNAPSHOT = 0x2C;
        const int VK_UP = 0x26;
        const int VK_VOLUME_DOWN = 0xAE;
        const int VK_VOLUME_MUTE = 0xAD;
        const int VK_VOLUME_UP = 0xAF;
        const int VK_XBUTTON1 = 0x05;
        const int VK_XBUTTON2 = 0x06;

        [DllImport("user32.dll")]
        public static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);
        [DllImport("user32.dll")]
        static extern bool PostMessage(IntPtr hWnd, UInt32 Msg, int wParam, int lParam);
        [DllImport("user32.dll")]
        static extern int FindWindow(string lpClassName, String lpWindowName);
        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);
        [DllImport("User32.dll")]
        static extern int SetForegroundWindow(IntPtr point);

        public static void SendText(string text)
        {
            Process p = Process.GetProcessesByName("notepad").FirstOrDefault();
            //Process p = Process.GetProcessById(52240);
            if (p != null)
            {
                IntPtr h = p.MainWindowHandle;
                Console.WriteLine($"Notepad window handle is {h}");
                IntPtr hWndEdit = FindWindowEx(h, IntPtr.Zero, "RichEditD2DPT", null);
                if (hWndEdit != IntPtr.Zero)
                {
                    PostMessage(hWndEdit, WM_CHAR, (int)'a', 0);
                    Console.WriteLine($"Posted message to {hWndEdit.ToString()}");
                }
                else
                {
                    Console.WriteLine("Could not find the \"Edit\" window");
                }
            }
            else
            {
                Console.WriteLine("Could not find \"notepad\".");
            }
        }
        static void Main()
        {
            SendText("HelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHelloHello");
        }
    }
}
