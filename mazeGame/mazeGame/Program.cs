using System;
using System.Drawing;
using System.Drawing.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace mazeGame
{
    public enum SquareType
    {
        OPEN,
        SPIKE,
        MOVE,
        START,
        END
    }
    public struct GridArea
    {
        public SquareType type;
        public Rectangle area;
        public int top;
        public int bottom;
        public int time;
        public bool up;
    }
    public struct Level
    {
        public List<GridArea> grid;
        public Point start;
        public Point end;
        public String name;
    }
    public class GameForm : Form
    {
        private const int gridSz = 50;
        private const int playerSz = 30;

        private int x = 0;
        private int xSpeed = 0;
        private int y = 0;
        private int ySpeed = 0;
        private Font font = new Font("Arial", 24, FontStyle.Bold);
        private Brush brush = Brushes.Black;
        private List<Level> levels = new();
        private System.Windows.Forms.Timer timer = new();
        private int lvl = 0;
        private int ticks = 0;
        private int hp = 3;
        private bool alive = true;
        private bool debug = false;
        private bool cheated = false;
        private float Max(float a, float b)
        {
            if(a > b)
            {
                return a;
            }
            return b;
        }
        private float Min(float a, float b)
        {
            if(a < b)
            {
                return a;
            }
            return b;
        }
        public GameForm()
        {
            this.DoubleBuffered = true; // Reduce flickering
            this.Paint += new PaintEventHandler(this.GameForm_Paint);
            this.ClientSize = new Size(800, 600);
            this.Name = "A Game";
            this.KeyPreview = true;
            this.KeyDown += new KeyEventHandler(this.GameForm_KeyHandler);
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            GameForm_initAll();
            for (int k = 0; k < levels[lvl].grid.Count; ++k)
            {
                if (levels[lvl].grid[k].type == SquareType.START)
                {
                    x = levels[lvl].grid[k].area.X + levels[lvl].grid[k].area.Width / 2 - playerSz / 2;
                    y = levels[lvl].grid[k].area.Y + levels[lvl].grid[k].area.Height / 2 - playerSz / 2;
                    break;
                }
            }
            timer.Interval = 10; // 10 ms
            timer.Tick += new EventHandler(this.GameForm_Tick);
            timer.Start();
        }
        private void GameForm_initAll()
        {
            GameForm_initBoard1();
            GameForm_initBoard2();
            GameForm_initBoard3();
        }
        private void GameForm_initBoard1()
        {
            Level level1 = new();
            level1.name = "Into to Friction";
            level1.grid = new();
            GridArea border = new();
            border.type = SquareType.SPIKE;
            border.area = new Rectangle(0, 0, 800, 50);
            GridArea border2 = new();
            border2.type = SquareType.SPIKE;
            border2.area = new Rectangle(0, 550, 800, 50);
            GridArea border3 = new();
            border3.type = SquareType.SPIKE;
            border3.area = new Rectangle(750, 0, 50, 600);
            GridArea border4 = new();
            border4.type = SquareType.SPIKE;
            border4.area = new Rectangle(0, 0, 50, 600);
            level1.grid.Add(border);
            level1.grid.Add(border2);
            level1.grid.Add(border3);
            level1.grid.Add(border4);
            GridArea a1 = new();
            a1.type = SquareType.SPIKE;
            a1.area = new Rectangle(0, 200, 700, 50);
            level1.grid.Add(a1);
            GridArea a2 = new();
            a2.type = SquareType.SPIKE;
            a2.area = new Rectangle(650, 200, 50, 300);
            level1.grid.Add(a2);
            GridArea a3 = new();
            a3.type = SquareType.SPIKE;
            a3.area = new Rectangle(200, 300, 50, 50);
            level1.grid.Add(a3);
            GridArea a4 = new();
            a4.type = SquareType.SPIKE;
            a4.area = new Rectangle(300, 320, 50, 50);
            level1.grid.Add(a4);
            GridArea a5 = new();
            a5.type = SquareType.SPIKE;
            a5.area = new Rectangle(100, 390, 50, 50);
            level1.grid.Add(a5);
            GridArea a6 = new();
            a6.type = SquareType.SPIKE;
            a6.area = new Rectangle(180, 470, 50, 50);
            level1.grid.Add(a6);
            GridArea a7 = new();
            a7.type = SquareType.SPIKE;
            a7.area = new Rectangle(360, 250, 50, 50);
            level1.grid.Add(a7);
            GridArea a8 = new();
            a8.type = SquareType.SPIKE;
            a8.area = new Rectangle(50, 480, 50, 50);
            level1.grid.Add(a8);
            GridArea a9 = new();
            a9.type = SquareType.SPIKE;
            a9.area = new Rectangle(230, 380, 50, 50);
            level1.grid.Add(a9);
            GridArea start = new();
            start.type = SquareType.START;
            start.area = new Rectangle(50, 50, 70, 70);
            level1.grid.Add(start);
            GridArea end = new();
            end.type = SquareType.END;
            end.area = new Rectangle(50, 250, 70, 70);
            level1.grid.Add(end);
            levels.Add(level1);
        }
        private void GameForm_initBoard2()
        {
            Level level2 = new();
            level2.grid = new();
            level2.name = "Speedybus";
            GridArea border = new();
            border.type = SquareType.SPIKE;
            border.area = new Rectangle(0, 0, 800, 50);
            GridArea border2 = new();
            border2.type = SquareType.SPIKE;
            border2.area = new Rectangle(0, 550, 800, 50);
            GridArea border3 = new();
            border3.type = SquareType.SPIKE;
            border3.area = new Rectangle(750, 0, 50, 600);
            GridArea border4 = new();
            border4.type = SquareType.SPIKE;
            border4.area = new Rectangle(0, 0, 50, 600);
            level2.grid.Add(border);
            level2.grid.Add(border2);
            level2.grid.Add(border3);
            level2.grid.Add(border4);
            GridArea a1 = new();
            a1.type = SquareType.SPIKE;
            a1.area = new Rectangle(0, 200, 700, 50);
            level2.grid.Add(a1);
            GridArea a2 = new();
            a2.type = SquareType.SPIKE;
            a2.area = new Rectangle(650, 200, 50, 300);
            level2.grid.Add(a2);
            GridArea a3 = new();
            a3.type = SquareType.SPIKE;
            a3.area = new Rectangle(100, 450, 550, 50);
            level2.grid.Add(a3);
            GridArea a4 = new();
            a4.type = SquareType.SPIKE;
            a4.area = new Rectangle(100, 300, 50, 200);
            level2.grid.Add(a4);
            GridArea a5 = new();
            a5.type = SquareType.SPIKE;
            a5.area = new Rectangle(200, 200, 500, 55);
            level2.grid.Add(a5);
            GridArea a6 = new();
            a6.type = SquareType.SPIKE;
            a6.area = new Rectangle(300, 290, 315, 50);
            level2.grid.Add(a6);
            GridArea a7 = new();
            a7.type = SquareType.SPIKE;
            a7.area = new Rectangle(150, 300, 450, 50);
            level2.grid.Add(a7);
            GridArea a8 = new();
            a8.type = SquareType.SPIKE;
            a8.area = new Rectangle(400, 200, 50, 50);
            level2.grid.Add(a8);
            GridArea a9 = new();
            a9.type = SquareType.MOVE;
            a9.area = new Rectangle(400, 0, 500, 50);
            a9.bottom = 10;
            a9.top = 200;
            a9.time = 2;
            level2.grid.Add(a9);
            GridArea start = new();
            start.type = SquareType.START;
            start.area = new Rectangle(50, 50, 70, 70);
            level2.grid.Add(start);
            GridArea end = new();
            end.type = SquareType.END;
            end.area = new Rectangle(150, 350, 70, 70);
            level2.grid.Add(end);
            levels.Add(level2);
        }
        private void GameForm_initBoard3()
        {
            Level level3 = new();
            level3.grid = new();
            level3.name = "Hit-And-Run";
            GridArea border = new();
            border.type = SquareType.SPIKE;
            border.area = new Rectangle(0, 0, 800, 50);
            GridArea border2 = new();
            border2.type = SquareType.SPIKE;
            border2.area = new Rectangle(0, 550, 800, 50);
            GridArea border3 = new();
            border3.type = SquareType.SPIKE;
            border3.area = new Rectangle(750, 0, 50, 600);
            GridArea border4 = new();
            border4.type = SquareType.SPIKE;
            border4.area = new Rectangle(0, 0, 50, 600);
            level3.grid.Add(border);
            level3.grid.Add(border2);
            level3.grid.Add(border3);
            level3.grid.Add(border4);
            GridArea mainBarier = new();
            mainBarier.type = SquareType.SPIKE;
            mainBarier.area = new Rectangle(200, 0, 450, 500);
            level3.grid.Add(mainBarier);
            GridArea a1 = new();
            a1.type = SquareType.MOVE;
            a1.area = new Rectangle(200, 500, 450, 50);
            a1.top = 500;
            a1.bottom = 350;
            a1.time = 1;
            level3.grid.Add(a1);
            GridArea a2 = new();
            a2.type = SquareType.MOVE;
            a2.area = new Rectangle(700, 500, 50, 50);
            a2.top = 500;
            a2.bottom = 350;
            a2.time = 4;
            level3.grid.Add(a2);
            GridArea a3 = new();
            a3.type = SquareType.MOVE;
            a3.area = new Rectangle(700, 300, 50, 50);
            a3.top = 300;
            a3.bottom = 150;
            a3.time = 4;
            level3.grid.Add(a3);
            GridArea a4 = new();
            a4.type = SquareType.MOVE;
            a4.area = new Rectangle(650, 400, 50, 50);
            a4.top = 400;
            a4.bottom = 250;
            a4.time = 4;
            level3.grid.Add(a4);
            GridArea a5 = new();
            a5.type = SquareType.MOVE;
            a5.area = new Rectangle(650, 200, 50, 50);
            a5.top = 200;
            a5.bottom = 50;
            a5.time = 4;
            level3.grid.Add(a5);
            GridArea start = new();
            start.type = SquareType.START;
            start.area = new Rectangle(50, 50, 70, 70);
            level3.grid.Add(start);
            GridArea end = new();
            end.type = SquareType.END;
            end.area = new Rectangle(700, 50, 50, 50);
            level3.grid.Add(end);
            levels.Add(level3);
        }

        private void GameForm_Paint(object sender, PaintEventArgs e)
        {
            if(levels.Count == 0)
            {
            return;
            }
            if (!alive)
            {
                return;
            }
            Graphics g = e.Graphics;
            for (int i = 0; i < levels[lvl].grid.Count; ++i)
            {
                GridArea n = levels[lvl].grid[i];
                if (n.type == SquareType.SPIKE)
                {
                    g.FillRectangle(Brushes.Red, n.area);
                }
                else if (n.type == SquareType.START)
                {
                    g.FillRectangle(Brushes.Blue, n.area);
                }
                else if (n.type == SquareType.END)
                {
                    g.FillRectangle(Brushes.Green, n.area);
                }
                else if(n.type == SquareType.MOVE)
                {
                    g.FillRectangle(Brushes.OrangeRed, n.area);
                }
            }
            g.FillRectangle(Brushes.Cyan, x, y, playerSz, playerSz);
            g.DrawString("Health: ", font, Brushes.Black, 10f, 10f);
            g.DrawString($"Level {lvl + 1}: " + levels[lvl].name, font, Brushes.Black, 300f, 10f);
            for(int i = 0; i < hp; ++i)
            {
                g.DrawIcon(SystemIcons.Shield, 130 + 35 * i, 10);
            }
            if (debug)
            {
                g.DrawString($"x: {x}\ny: {y}\nxSpeed: {xSpeed}\nySpeed: {ySpeed}\nlevel: {lvl}\nticks: {ticks}\nhp: {hp}", new Font("Arial", 10), Brushes.Black, 700f, 10f);
            }
        }

        private void GameForm_Tick(object sender, EventArgs e)
        {
            if(levels.Count == 0)
            {
                return;
            }
            if (!alive)
            {
                return;
                }
            ++ticks;
            if (ticks % 3 == 0)
            {
                x += xSpeed;
                y += ySpeed;
            }
            // Decelerate
            if (ticks % 20 == 0)
            {
                if (xSpeed > 0)
                {
                    xSpeed -= 1;
                }
                else if (xSpeed < 0)
                {
                    xSpeed += 1;
                }
                if (ySpeed > 0)
                {
                    ySpeed -= 1;
                }
                else if (ySpeed < 0)
                {
                    ySpeed += 1;
                }
            }
            Rectangle player = new((int)x, (int)y, playerSz, playerSz);
            for (int i = 0; i < levels[lvl].grid.Count; ++i)
            {
                if (player.IntersectsWith(levels[lvl].grid[i].area))
                {
                    // Check which type
                    if (levels[lvl].grid[i].type == SquareType.SPIKE || levels[lvl].grid[i].type == SquareType.MOVE)
                    {
                        // Lose HP
                        --hp;
                        x = 0;
                        y = 0;
                        for(int k = 0; k < levels[lvl].grid.Count; ++k)
                        {
                            if (levels[lvl].grid[k].type == SquareType.START)
                            {
                                x = levels[lvl].grid[k].area.X + levels[lvl].grid[k].area.Width / 2 - playerSz / 2;
                                y = levels[lvl].grid[k].area.Y + levels[lvl].grid[k].area.Height / 2 - playerSz / 2;
                                break;
                            }
                        }
                        xSpeed = 0;
                        ySpeed = 0;
                        MessageBox.Show("You got hurt!", "A game", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        break;
                    }
                    else if (levels[lvl].grid[i].type == SquareType.END)
                    {
                        // Move to next level
                        if (lvl + 1 < levels.Count)
                        {
                            // Enough levels left
                            ++lvl;
                            for (int k = 0; k < levels[lvl].grid.Count; ++k)
                            {
                                if (levels[lvl].grid[k].type == SquareType.START)
                                {
                                    x = levels[lvl].grid[k].area.X + levels[lvl].grid[k].area.Width / 2 - playerSz / 2;
                                    y = levels[lvl].grid[k].area.Y + levels[lvl].grid[k].area.Height / 2 - playerSz / 2;
                                    break;
                                }
                            }
                            break;
                        }
                        else
                        {
                            // Game end
                            GameForm_End($"You won!\nThis game was not cheated: {!cheated}\nYour time was: {ticks}\nYour score was: {Max(0, (lvl + 1) * 20 - ticks / 1000 + hp*30)}");
                        }
                    }
                }
                if (levels[lvl].grid[i].type == SquareType.MOVE)
                {
                    if (!(ticks % (levels[lvl].grid[i].time) == 0))
                    {
                        continue;
                    }
                    var n = levels[lvl].grid[i];
                    if (n.up)
                    {
                        n.area.Y += 1;
                        if (n.area.Y > n.top)
                        {
                            n.up = false;
                        }
                    }
                    else
                    {
                        n.area.Y -= 1;
                        if (n.area.Y < n.bottom)
                        {
                            n.up = true;
                        }
                    }
                    levels[lvl].grid[i] = n;
                }
            }
            if(hp < 0)
            {
                GameForm_End("You died from a truely horrible death :(");
            }
            this.Invalidate();
        }

        private void GameForm_KeyHandler(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Q:
                    GameForm_End($"You quit.\nThis game was not cheated: {!cheated}\n You got to level: {lvl + 1}\nYour time was: {ticks}\nYour score was: {Max(0, (lvl + 1) * 20 - ticks / 100 + hp*30)}");
                    break;
                case Keys.W:
                    ySpeed -= 1;
                    break;
                case Keys.S:
                    ySpeed += 1;
                    break;
                case Keys.D:
                    xSpeed += 1;
                    break;
                case Keys.A:
                    xSpeed -= 1;
                    break;
                case Keys.F7:
                    debug = !debug;
                    break;
                case Keys.OemMinus:
                    if (debug)
                    {
                        cheated = true;
                        x = Cursor.Position.X - this.Left;
                        xSpeed = 0;
                        y = Cursor.Position.Y - this.Top - 10;
                        ySpeed = 0;
                    }
                    break;
            }
        }

        private void GameForm_End(string ?msg) 
        {
            alive = false;
            timer.Stop();
            MessageBox.Show(msg, "A game", MessageBoxButtons.OK, MessageBoxIcon.Information);
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
