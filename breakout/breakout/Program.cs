using System.CodeDom;
using System.Runtime.Versioning;
using System.Windows.Forms;

namespace breakout
{
    public class GameForm : Form
    {
        private const int YSZ = 6;
        private const int XSZ = 10;
        private const int YOFFSET = 40;
        private const int YCLIENTSZ = 400;
        private const int XCLIENTSZ = 500;
        private const int BLOCKXSZ = 50;
        private const int BLOCKYSZ = 20;
        private const int PLAYERSZ = 10;
        private const int PADDLEHEIGHT = 20;
        private const int PADDLEY = YCLIENTSZ - PADDLEHEIGHT - 10;
        private const int PADDLEWIDTH = 200;
        private const int MOVEMENTPERPRESS = 10;

        private Random rng = new Random(System.DateTime.Now().;
        private int x = XCLIENTSZ / 2;
        private int y = 300;
        private int speed = 5;
        private double angle = 0;
        private int pos = 50;
        private int tick = 0;
        private List<Brush> colors = new List<Brush> { Brushes.DeepSkyBlue, Brushes.Aqua, Brushes.Green, Brushes.Yellow, Brushes.Orange, Brushes.Red };
        private List<bool> exist = new(XSZ * YSZ);
        private bool debug = false;

        private System.Windows.Forms.Timer timer;
        public GameForm()
        {
            for(int i = 0; i < XSZ * YSZ; ++i)
            {
                exist.Add(true);
            }
            angle = rng.NextDouble() * Math.PI / 2;
            this.DoubleBuffered = true;
            this.ClientSize = new Size(XCLIENTSZ, YCLIENTSZ);
            this.Paint += this.GameFormPaint;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.KeyUp += new KeyEventHandler(this.GameFormKeyUp);
            this.Name = "Break";
            timer = new System.Windows.Forms.Timer();
            timer.Interval = 10;
            timer.Tick += new EventHandler(this.GameFormTick);
            timer.Start();
        }
        private void GameFormTick(object? sender, EventArgs e)
        {
            if (tick % 3 == 0)
            {
                int xSpeed = (int)(Math.Sin(angle) * speed);
                int ySpeed = (int)(Math.Cos(angle) * speed);
                x += xSpeed;
                y += ySpeed;
            }

            // Check for collision with walls
            if(y > this.ClientSize.Height)
            {
                GameFormLose();
            }
            if(y < 0)
            {
                angle = (Math.PI - angle) % (2 * Math.PI);
            }
            else if(x < 0 || x > this.ClientSize.Width - PLAYERSZ)
            {
                angle = (Math.PI - angle) % (2 * Math.PI) + Math.PI;
            }
            // Check for collisions with blocks
            for (int i = 0; i < YSZ; i++)
            {
                for (int j = 0; j < XSZ; j++)
                {
                    Rectangle r = new(BLOCKXSZ * j, YOFFSET + BLOCKYSZ * i, BLOCKXSZ, BLOCKYSZ);
                    Rectangle player = new(x, y, PLAYERSZ, PLAYERSZ);
                    if (player.IntersectsWith(r) && exist[i * XSZ + j])
                    {
                        // Check which position
                    }
                }   
            }
            ++tick;
            this.Invalidate();
        }
        private void GameFormPaint(object? sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            for (int i = 0; i < YSZ; ++i)
            {
                for (int j = 0; j < XSZ; ++j)
                {
                    if (exist[i * YSZ + j])
                    {
                        g.FillRectangle(colors[i], new Rectangle(BLOCKXSZ * j, YOFFSET + BLOCKYSZ * i, BLOCKXSZ - 5, BLOCKYSZ - 5));
                    }
                }
            }
            g.FillRectangle(Brushes.Brown, new Rectangle(x, y, PLAYERSZ, PLAYERSZ));
            g.FillRectangle(Brushes.Aquamarine, new Rectangle(pos, PADDLEY, PADDLEWIDTH, PADDLEHEIGHT));
            if (debug)
            {
                g.DrawString($"x: {x}\ny: {y}\nangle: {(int)(angle * 360 / Math.PI / 2)} degrees\npos: {pos}", new Font("Arial", 10), Brushes.Black, 10f, 10f);
            }
        }
        private void GameFormKeyUp(object? sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.A:
                    // Move left after check
                    if(pos > MOVEMENTPERPRESS)
                    {
                        pos -= MOVEMENTPERPRESS;
                    }
                    break;
                case Keys.D:
                    if (pos + MOVEMENTPERPRESS + PADDLEWIDTH < this.ClientSize.Width)
                    {
                        pos += MOVEMENTPERPRESS;
                    }
                    break;
                case Keys.F7:
                    debug = !debug;
                    break;
            }
        }
        private void GameFormLose()
        {
            timer.Stop();
            MessageBox.Show($"You lose :(\nScore: {Math.Max(0, 0 - tick)}");
            this.Close();
            Application.Exit();
        }
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new GameForm());
        }
    }
}