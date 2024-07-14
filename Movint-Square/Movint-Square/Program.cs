using Microsoft.VisualBasic.Devices;
using System.Windows.Forms;

namespace Moving_Square
{
    internal class Program
    {

        static int xoff = 0;
        static int yoff = 0;
        static int sz = 80;
        static void Main(string[] args)
        {
            Form f = new();
            f.Size = new(500, 500);
            f.BackColor = SystemColors.Window;
            f.ForeColor = SystemColors.WindowText;
            f.ControlBox = true;
            f.Text = "Moving square program";
            f.Icon = SystemIcons.Application;
            f.KeyPreview = true;
            Bitmap bp = new(5000, 5000);
            f.KeyPress += FKeyPress;
            bool fopen = true;
            f.FormClosed += new FormClosedEventHandler((object? o, FormClosedEventArgs e) => fopen = false);
            while (fopen)
            {
                    bp.Dispose();
                    bp = new Bitmap(5000, 5000);
                    for (int i = yoff; i < yoff + sz; ++i)
                    {
                        for (int j = xoff; j < xoff + sz; ++j)
                        {
                            bp.SetPixel(j, i, Color.Red);
                        }
                    }
                f.Close();
                f = new();
                f.Size = new(500, 500);
                f.BackColor = SystemColors.Window;
                f.ForeColor = SystemColors.WindowText;
                f.ControlBox = true;
                f.Text = "Moving square program";
                f.Icon = SystemIcons.Application;
                f.KeyPreview = true;
                f.BackgroundImage = (Image)bp;
                f.KeyPress += FKeyPress;
                f.FormClosed += new FormClosedEventHandler((object? o, FormClosedEventArgs e) => fopen = false);
                f.KeyPreview = true;
                f.Show();
            }
        }

        static private void FKeyPress(object? sender, KeyPressEventArgs e)
        {
            switch (e.KeyChar)
            {
                case 'a':
                    --xoff;
                    break;
                case 'w':
                    --yoff;
                    break;
                case 's':
                    ++yoff;
                    break;
                case 'd':
                    ++xoff;
                    break;
                case 'q':
                    ++sz;
                    break;
                case 'e':
                    --sz;
                    break;
                default:
                    return;
            }
        }
    }
}
