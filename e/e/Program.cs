using Microsoft.VisualBasic.Devices;
using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace FakeBSOD
{
    public class BSODForm : Form
    {
        private System.Windows.Forms.Label emojiLabel;
        private System.Windows.Forms.Label bsodLabel;
        private System.Windows.Forms.Timer fadeOutTimer;

        // DLL imports for low-level keyboard hook
        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc lpfn, IntPtr hMod, uint dwThreadId);

        [DllImport("user32.dll", SetLastError = true)]
        [return: MarshalAs(UnmanagedType.Bool)]
        private static extern bool UnhookWindowsHookEx(IntPtr hhk);

        [DllImport("user32.dll")]
        private static extern IntPtr CallNextHookEx(IntPtr hhk, int nCode, IntPtr wParam, IntPtr lParam);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern IntPtr GetModuleHandle(string lpModuleName);

        private static LowLevelKeyboardProc _proc = HookCallback;
        private static IntPtr _hookID = IntPtr.Zero;
        public int abs(int i)
        {
            if(i > 0)
            {
                return i;
            }
            return -i;
        }
        public BSODForm()
        {
            var pos = Cursor.Position;
            while (true)
            {
                if(abs(Cursor.Position.X - pos.X) > 600 || abs(Cursor.Position.Y - pos.Y) > 600)
                {
                    break;
                }
            }
            Thread.Sleep(10000);
            InitializeComponent();
            _hookID = SetHook(_proc);
        }

        private void InitializeComponent()
        {
            this.emojiLabel = new System.Windows.Forms.Label();
            this.bsodLabel = new System.Windows.Forms.Label();
            this.fadeOutTimer = new System.Windows.Forms.Timer();
            this.SuspendLayout();
            // 
            // emojiLabel
            // 
            this.emojiLabel.AutoSize = true;
            this.emojiLabel.Font = new Font("Segoe UI", 72F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.emojiLabel.ForeColor = Color.White;
            this.emojiLabel.Location = new Point(50, 50);
            this.emojiLabel.Name = "emojiLabel";
            this.emojiLabel.Size = new Size(150, 128);
            this.emojiLabel.TabIndex = 0;
            this.emojiLabel.Text = ":(";
            // 
            // bsodLabel
            // 
            this.bsodLabel.AutoSize = true;
            this.bsodLabel.Font = new Font("Segoe UI", 18F, FontStyle.Regular, GraphicsUnit.Point, ((byte)(0)));
            this.bsodLabel.ForeColor = Color.White;
            this.bsodLabel.Location = new Point(50, 200);
            this.bsodLabel.Name = "bsodLabel";
            this.bsodLabel.Size = new Size(0, 16);
            this.bsodLabel.TabIndex = 1;
            this.bsodLabel.Text = "Your PC ran into a problem and needs to restart. We're just collecting some error info, and then we'll restart for you.\n\n\n0% complete\n\nFor more information about this issue and possible fixes, visit https://www.windows.com/stopcode\n\nIf you call a support person, give them this info:\nStop code: CRITICAL_PROCESS_DIED";
            // 
            // fadeOutTimer
            // 
            this.fadeOutTimer.Interval = 50; // Timer interval in milliseconds
            this.fadeOutTimer.Tick += new EventHandler(FadeOutTimer_Tick);
            // 
            // BSODForm
            // 
            this.AutoScaleDimensions = new SizeF(6F, 13F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.BackColor = Color.FromArgb(0, 120, 215);
            this.ClientSize = new Size(800, 450);
            this.Controls.Add(this.emojiLabel);
            this.Controls.Add(this.bsodLabel);
            this.FormBorderStyle = FormBorderStyle.None;
            this.Name = "BSODForm";
            this.StartPosition = FormStartPosition.CenterScreen;
            this.Text = "Fake BSOD";
            this.WindowState = FormWindowState.Maximized;
            this.TopMost = true;
            this.Load += new EventHandler(BSODForm_Load);
            this.ResumeLayout(false);
            this.PerformLayout();
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new BSODForm());
        }

        private void BSODForm_Load(object sender, EventArgs e)
        {
            // Start the fade-out process after 5 seconds
            System.Windows.Forms.Timer startFadeOutTimer = new System.Windows.Forms.Timer();
            startFadeOutTimer.Interval = 5000; // 5 seconds
            startFadeOutTimer.Tick += (s, args) =>
            {
                startFadeOutTimer.Stop();
                fadeOutTimer.Start();
            };
            startFadeOutTimer.Start();
        }

        private void FadeOutTimer_Tick(object sender, EventArgs e)
        {
            if (this.Opacity > 0)
            {
                this.Opacity -= 0.05;
            }
            else
            {
                fadeOutTimer.Stop();
                Application.Exit();
            }
        }

        // Override WndProc to prevent Alt+F4
        protected override void WndProc(ref Message m)
        {
            const int WM_SYSCOMMAND = 0x0112;
            const int SC_CLOSE = 0xF060;

            if (m.Msg == WM_SYSCOMMAND && (int)m.WParam == SC_CLOSE)
            {
                return;
            }
            base.WndProc(ref m);
        }

        private static IntPtr SetHook(LowLevelKeyboardProc proc)
        {
            using (var curProcess = System.Diagnostics.Process.GetCurrentProcess())
            using (var curModule = curProcess.MainModule)
            {
                return SetWindowsHookEx(13, proc, GetModuleHandle(curModule.ModuleName), 0);
            }
        }

        private static IntPtr HookCallback(int nCode, IntPtr wParam, IntPtr lParam)
        {
            if (nCode >= 0)
            {
                int vkCode = Marshal.ReadInt32(lParam);
                // Block Ctrl+Alt+Delete and Alt+Tab
                if ((Control.ModifierKeys & Keys.Control) == Keys.Control &&
                    (Control.ModifierKeys & Keys.Alt) == Keys.Alt &&
                    vkCode == (int)Keys.Delete)
                {
                    return (IntPtr)1; // Block Ctrl+Alt+Delete
                }
                if ((Control.ModifierKeys & Keys.Alt) == Keys.Alt &&
                    vkCode == (int)Keys.Tab)
                {
                    return (IntPtr)1; // Block Alt+Tab
                }
            }
            return CallNextHookEx(_hookID, nCode, wParam, lParam);
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            UnhookWindowsHookEx(_hookID);
            base.OnFormClosing(e);
        }
    }
}
