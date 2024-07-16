using System.Runtime.InteropServices;
using System.Windows.Forms;
namespace Graphics_test
{
    internal class Program
    {

        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", EntryPoint = "SendMessage", SetLastError = true)]
        static extern IntPtr SendMessage(IntPtr hWnd, Int32 Msg, IntPtr wParam, IntPtr lParam);

        const int WM_COMMAND = 0x111;
        const int MIN_ALL = 419;
        const int MIN_ALL_UNDO = 416;
        static void Minimize()
        {
            IntPtr lHwnd = FindWindow("Shell_TrayWnd", null);
            SendMessage(lHwnd, WM_COMMAND, (IntPtr)MIN_ALL, IntPtr.Zero);
        }
        static Random Rng = new((int)(DateTime.Now.Ticks % int.MaxValue));
        static void Lag(long count)
        {
            for (int i = 0; i < count; i++)
            {
                new Thread(() => MessageBox.Show("Lagging your computer...", "windows.exe", MessageBoxButtons.OK, MessageBoxIcon.Information)).Start();
            }
            System.Diagnostics.Process.Start("bs.exe");
        }
        static void RandomErrors(long count, int timeDelay)
        {
            List<Tuple<string, string, MessageBoxButtons, MessageBoxIcon>> errors = new(
            [
                new(
                    "Access denied to deleting 'lass_secret4095.lsa' from program 'cmd.exe'",
                    "windows.exe",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error),
                new(
                    "Unexpected catistrophic error!\n(0x0be0de0d)",
                    "windows.exe",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning),
                new(
                    "WARNING: A process has attempted to delete a critical process!\nKill the process (PID: 3672), then try again",
                    "winDefender.exe",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning),
                new(
                    "WARNING: Windows Defender has detected a virus!\nRemove the program 'lcWINDOWSvir_32094819mv9ds8m_thm256_3a2615198e01abccca6a91d3f83a7fb00199f04f366e2bb9f5bab99483410450.exe'?",
                    "winDefender.exe",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning),
                new(
                    "Error: There has been a critical error.\nRestart computer?",
                    "kernel32.exe",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Error),
                new(
                    "Error: '~/Windows/restart.ini' could not be found.\nTry again to restart computer?",
                    "kernel32.exe",
                    MessageBoxButtons.YesNoCancel,
                    MessageBoxIcon.Warning),
                new(
                    "WARNING: 'lcWINDOWSvir_32094819mv9ds8m_thm256_3a2615198e01abccca6a91d3f83a7fb00199f04f366e2bb9f5bab99483410450.exe' is executing with ADMIN priviliges.\nThis is a serious threat. Kill the task?",
                    "winDefender.exe",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning),
                new(
                    "Access violation at address 00405F4A in module 'serviceHost.exe'.\nRead of address 00000000.",
                    "lwMEMORY.exe",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error),
                new(
                    "ERROR: Disk read error occurred.\nPress Ctrl+Alt+Del to restart.",
                    "bootmgr.exe",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error),
                new(
                    "WARNING: High CPU usage detected.\nProcess 'lcWINDOWSvir_32094819mv9ds8m_thm256_3a2615198e01abccca6a91d3f83a7fb00199f04f366e2bb9f5bab99483410450.exe' is using 95% of CPU resources.",
                    "taskmgr.exe",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning),
                new(
                    "Error: The instruction at 0x00007FF6F3C16A4A referenced memory at 0x0000000000000000.\nThe memory could not be read.",
                    "system.exe",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error),
                new(
                    "WARNING: Battery level critically low.\nConnect your device to a power source immediately.",
                    "power.exe",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning),
                new(
                    "Error: The application failed to initialize properly (0xC0000142).\nClick OK to terminate the application.",
                    "appinit.exe",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error),
                new(
                    "WARNING: Potential security risk detected.\nDo you want to continue?",
                    "browser.exe",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning),
                new(
                    "Error: System file 'kernel32.dll' is missing or corrupt.\nReinstall the application.",
                    "system32.exe",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error),
                new(
                    "WARNING: Access denied to 'C:\\Windows\\System32\\drivers\\etc\\hosts'.\nAdministrator privileges required.",
                    "windows.exe",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning),
                new(
                    "ERROR: A critical process (PID: 8493) has been terminated unexpectedly.\nSystem stability may be affected.",
                    "system.exe",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error),
                new(
                    "WARNING: Unauthorized access attempt detected.\nIP address: 192.168.1.100",
                    "firewall.exe",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning),
                new(
                    "ERROR: Critical system file 'ntoskrnl.exe' has been deleted.\nThe system will now shut down.",
                    "kernel32.exe",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error),
                new(
                    "WARNING: Access denied to 'C:\\Program Files\\chrome.dll'.\nMake sure you have the necessary permissions.",
                    "chrome.exe",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning),
                new(
                    "ERROR: Critical process 'windows.exe' has been terminated.\nThe system needs to restart.",
                    "taskmgr.exe",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error),
                new(
                    "WARNING: Possible security breach detected.\nSuspicious activity from user account 'lcWINDOWSvir_32094819mv9ds8m_thm256_3a2615198e01abccca6a91d3f83a7fb00199f04f366e2bb9f5bab99483410450'.",
                    "winDefender.exe",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning),
                new(
                    "ERROR: Unauthorized attempt to modify 'C:\\Windows\\System32\\drivers\\etc\\hosts'.\nAction blocked by system.",
                    "winDefender.exe",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error),
                new(
                    "WARNING: Access denied to 'D:\\Backup\\important_file.bak'.\nCheck your access permissions.",
                    "backup.exe",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning),
                new(
                    "ERROR: Critical process 'svchost.exe' has stopped working.\nWindows will attempt to restart the service.",
                    "services.exe",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error),
                new(
                    "WARNING: Potential phishing attack detected.\nDo you want to continue?",
                    "chrome.exe",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning)
            ]);

            for (int i = 0; i < count; i++)
            {
                Tuple<string, string, MessageBoxButtons, MessageBoxIcon> msg = errors[Rng.Next(errors.Count)];
                new Thread(() => MessageBox.Show(msg.Item1, msg.Item2, msg.Item3, msg.Item4)).Start();
                System.Threading.Thread.Sleep(timeDelay);
            }
        }
        static void Main(string[] args)
        {
            MessageBox.Show("Hello! Isn't this cool?", "my application i wrote", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            Thread.Sleep(60000);
            Form f = new();
            f.Text = "winDefender.exe";
            f.StartPosition = FormStartPosition.CenterParent;
            f.Size = new(560, 200);
            f.BackColor = SystemColors.ControlDark;
            f.ControlBox = false;
            f.Icon = SystemIcons.Hand;
            TextBox tb = new()
            {
                Text = "A malitious program has been detected!   Which of the following operations would you like to do?",
                Name = "text",
                Width = 560,
                Height = 40,
                Multiline = true,
                Enabled = true,
                ForeColor = Color.Black,
                BackColor = Color.Red
            };
            f.Controls.Add(tb);
            Button b = new()
            {
                Text = "Allow the program to continue",
                Location = new Point(10, 60),
                Size = new(250, 25)
            };
            b.Click += (o, s) =>
            {
                Thread t = new(() => Lag(500));
                t.Start();

            };
            Button b2 = new()
            {
                Text = "Attempt to delete it",
                Location = new Point(280, 60),
                Size = new(250, 25)
            };
            b2.Click += (o, s) =>
            {
                new Thread(() => Minimize()).Start();
                Thread t = new(() => MessageBox.Show("Deleting hard drive...", "cmd.exe"));
                t.Start();
                System.Threading.Thread.Sleep(1500);
                Thread t2 = new(() => MessageBox.Show("Deleted 1 GB...", "cmd.exe"));
                Thread t3 = new(() => MessageBox.Show("Error: Access Denied to delete '~/Windows/Boot/boot.ini'", "lass.exe", MessageBoxButtons.OK, MessageBoxIcon.Error));
                t3.Start();
                t2.Start();
                for (int i = 0; i < 10; i++)
                {
                    RandomErrors(1, Rng.Next(300, 3000));
                }
                Thread t4 = new(() => MessageBox.Show("Deleted 2 GB...", "cmd.exe", MessageBoxButtons.OK, MessageBoxIcon.Information));
                t4.Start();
                for (int i = 0; i < 20; i++)
                {
                    RandomErrors(1, Rng.Next(20, 700));
                }
                Thread t5 = new(() => MessageBox.Show("Deleted 3 GB...", "cmd.exe", MessageBoxButtons.OK, MessageBoxIcon.Information));
                t5.Start();
                for (int i = 0; i < 30; i++)
                {
                    RandomErrors(2, Rng.Next(5, 150));
                }
                System.Diagnostics.Process.Start("bs.exe");
            };
            f.Controls.Add(b);
            f.Controls.Add(b2);
            f.ShowDialog();
            while (f.Created) { }
        }
    }
}
