using System;
using System.Diagnostics;
using System.Drawing;
using System.Windows.Forms;

namespace MovingSquare
{
    public struct Obsticle
    {
        public int Ytop, Ybottom;
        public int X;
    }
    public partial class Form1 : Form
    {
        private System.Windows.Forms.Timer timer;
        private int squareSize = 50;
        private int x, y;
        private int xSpeed = 0;
        private int ySpeed = 0;
        private int accel = 1;
        private int ticks = 0;
        private List<Obsticle> obsticles = new();
        private Random rng = new(DateTime.Now.Microsecond);
        private int speed = 2;
        private bool shodebug = false;
        private bool invincable = false;
        private bool cheated = false;
        private bool running = true;

        public Form1()
        {
            InitializeComponent();

            this.DoubleBuffered = true;
            this.KeyPreview = true;
            this.Paint += new PaintEventHandler(this.Form1_Paint);
            this.KeyUp += new KeyEventHandler(this.Form1_Keydown);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            timer = new System.Windows.Forms.Timer();
            timer.Interval = 30; // Set the timer interval (ms)
            timer.Tick += new EventHandler(this.Timer_Tick);
            MessageBox.Show("Press OK to start.", "Flappy Square", MessageBoxButtons.OK, MessageBoxIcon.Question);
            timer.Start();

            x = this.ClientSize.Width / 2 - squareSize / 2;
            y = this.ClientSize.Height / 2 - squareSize / 2;
        }
        private int Abs(int n)
        {
            if(n > 0)
            {
                return n;
            }
            return -n;
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            // 
            // Form1
            // 
            this.ClientSize = new System.Drawing.Size(800, 600);
            this.Name = "Flappy Square";
            this.ResumeLayout(false);
        }
        private bool CheckForCollision(Obsticle o)
        {
            Rectangle rtop = new Rectangle(o.X-25, 0, 50, o.Ytop);
            Rectangle rbottom = new Rectangle(o.X-25, o.Ybottom, 50, 600 - o.Ybottom);
            Rectangle square = new Rectangle(x, y, squareSize, squareSize);
            return (rtop.IntersectsWith(square) || rbottom.IntersectsWith(square));
        }
        [STAThread]
        private void Timer_Tick(object sender, EventArgs e)
        {
            if (!running)
            {
                return;
            }
            x += xSpeed;
            y += ySpeed;
            ySpeed += accel;

            // Check for boundaries and reverse direction if needed
            if (x < 0 || x + squareSize > this.ClientSize.Width)
            {
                xSpeed = -xSpeed;
            }

            if (ticks % 500 == 0 && ticks > 1)
            {
                ++speed;
            }

            if (obsticles.Count == 0 || obsticles[obsticles.Count-1].X <= 600)
            {
                // Draw new obstacle
                int sz = rng.Next(125, 300);
                int pos = rng.Next((580 - sz)/2) + rng.Next((580 - sz)/2);
                Obsticle o = new Obsticle();
                o.X = 800;
                o.Ytop = pos;
                o.Ybottom = pos + sz;
                obsticles.Add(o);
            }

            if (y < 0) {
                ySpeed = Abs(ySpeed);
                this.Form1_lose();
            }
            if( y + squareSize > this.ClientSize.Height)
            {
                ySpeed = -Abs(ySpeed);
                this.Form1_lose();
            }

            // List to keep track of obstacles to remove
            List<Obsticle> obstaclesToRemove = new List<Obsticle>();

            // Check for collision and mark obstacles for removal
            List<Obsticle> cpy = obsticles;

            for (int i = 0; i < cpy.Count; ++i)
            {
                var o = cpy[i];
                if (CheckForCollision(o))
                {
                    this.Form1_lose();
                }
                if (o.X < -50)
                {
                    obstaclesToRemove.Add(o);
                }
            }

            // Remove obstacles after the iteration is complete
            foreach (var o in obstaclesToRemove)
            {
                obsticles.Remove(o);
            }

            // Update positions of remaining obstacles
            for (int i = 0; i < obsticles.Count; i++)
            {
                Obsticle tmp = obsticles[i];
                tmp.X -= speed;
                obsticles[i] = tmp;
            }

            this.Size = new Size(800, 600);
            this.Invalidate(); // Refresh the form
            ++ticks;
        }

        [STAThread]
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            if (!running)
            {
                return;
            }
            Graphics g = e.Graphics;
            g.FillRectangle(Brushes.Red, x, y, squareSize, squareSize);
            foreach (var ob in obsticles)
            {

                Rectangle rtop = new Rectangle(ob.X - 25, 0, 50, ob.Ytop);
                Rectangle rbottom = new Rectangle(ob.X - 25, ob.Ybottom, 50, 600 - ob.Ybottom);
                g.FillRectangle(Brushes.Green, rtop);
                g.FillRectangle(Brushes.Green, rbottom);
            }
            if (shodebug)
            {
                g.DrawString($"Score: {ticks}\nySpeed: {ySpeed}\nticks: {ticks}\nInvincibiliy: {invincable}\nxPos: {x}\nyPos: {y}\nSpeed: {speed}\nAcceleration: {accel}", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new PointF(10f, 10f));
            }
            else
            {
                g.DrawString($"Score: {ticks}", new Font("Arial", 10, FontStyle.Bold), Brushes.Black, new PointF(10f, 10f));
            }
        }
        private void Form1_Keydown(object sender, KeyEventArgs e)
        {
            if (!running)
            {
                return;
            }
            if(e.KeyCode == Keys.W || e.KeyCode == Keys.Up)
            {
                ySpeed = -10;
            }
            if(e.KeyCode == Keys.F7)
            {
                // show debug
                this.shodebug = !shodebug;
            }
            if(shodebug && e.KeyCode == Keys.M)
            {
                invincable = !invincable;
                cheated = true;
            }
        }
        private void Form1_lose()
        {
            if (!invincable)
            {
                running = false;
                timer.Stop();
                try
                {
                    if (!File.Exists("savefile.txt"))
                    {
                        File.Create("savefile.txt");
                    }
                    StreamReader save = new("savefile.txt");
                    string score = save.ReadLine();
                    int iscore = 0;
                    if (score != null)
                    {
                        iscore = Convert.ToInt32(score);
                    }
                    MessageBox.Show($"You lost! Your score was {ticks}.\nThis game was cheated? {{{cheated}}}\nWARNING: High scores may have been tampered with\nPrevious high score: {iscore}", "Flappy Square", MessageBoxButtons.OKCancel, MessageBoxIcon.Information);
                    save.Close();
                    save.Dispose();
                    if (!cheated && ticks > iscore)
                    {
                        StreamWriter write = new("savefile.txt");
                        write.WriteLine(Convert.ToString(ticks));
                        write.Close();
                        write.Dispose();
                    }
                    if (MessageBox.Show("Try again?", "Flappy Square", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
                    {
                        y = 300;
                        while (obsticles.Count > 0)
                        {
                            obsticles.RemoveAt(0);
                        }
                        ySpeed = 0;
                        shodebug = false;
                        invincable = false;
                        ticks = 0;
                        speed = 2;
                        MessageBox.Show("Press OK to start.", "Flappy Square", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        timer.Start();
                        running = true;
                    }
                    else
                    {
                        timer.Dispose();
                        this.Close();
                    }
                }
                catch (Exception ex) {
                    MessageBox.Show($"There was an error:\n{ex.Message}", "Flappy Square", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
    }
}
