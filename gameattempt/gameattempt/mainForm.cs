using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace game
{
    enum LANDTYPE
    {
        HOUSE,
        FARM,
        FACTORY,
        ROAD
    }

    struct farm
    {
        public int remaining_food;
        public int workers;
        public int maxWorkers;
    }

    struct house
    {
        public int occupation;
        public int maxSz;
    }

    struct factory
    {
        public int workers;
        public int maxWorkers;
    }

    struct road
    {
        public bool developed;
    }

    struct structure
    {
        public LANDTYPE type;
        public int x;
        public int y;
        public int width;
        public int height;
        public house h;
        public farm f;
        public factory fact;
        public road r;
    }
    
    public class disabledLabel : Label
    {
        Color color;
        public void setColor(Color c)
        {
            color = c;
        }
        protected override void OnPaint(PaintEventArgs pevent)
        {
            if (!Enabled)
            {
                TextRenderer.DrawText(pevent.Graphics, this.Text, this.Font, this.ClientRectangle, color, TextFormatFlags.VerticalCenter);
            }
            else
            {
                base.OnPaint(pevent);
            }
        }
    }
    public partial class helpWindow : Form
    {
        private Label title;
        private Label population;
        private Button populationHelp;

        public int populationCnt = 0;

        public helpWindow()
        {
            Initilize_Component();
        }
        private void popHelp_Click(object? sender, EventArgs e)
        {
            MessageBox.Show("Population is a measure of your city's size.\nThe bigger the populationt the better!\nYou earn money based on your population.\nFor example, right now, you would earn " + ((int)(populationCnt * 0.3)).ToString() + " dollars.", "Population Explanation");
        }
        private void Initilize_Component()
        {
            this.SuspendLayout();

            // Form properties
            this.Text = "Farm Game Help Window";
            this.ClientSize = new Size(500, 500);

            title = new Label
            {
                Text = "Farm Game Help Window",
                TextAlign = ContentAlignment.TopCenter,
                ForeColor = Color.Black,
                Font = new Font("Arial", 16),
                Location = new Point(100, 10),
                Size = new Size(300, 30)
            };
            population = new Label
            {
                Text = "Total Population: " + populationCnt.ToString(),
                ForeColor = Color.Black,
                Font = new Font("Arial", 12),
                Location = new Point(10, 50),
                Size = new Size(150, 50)
            };
            populationHelp = new Button
            {
                Text = "More",
                Location = new Point(160, 47),
                ForeColor = Color.Black
            };
            populationHelp.Click += popHelp_Click;

            this.Controls.Add(title);
            this.Controls.Add(population);
            this.Controls.Add(populationHelp);


            this.ResumeLayout(false);
        }
        public void Reload()
        {
            this.SuspendLayout();
            this.Controls.Clear();

            // Form properties
            this.Text = "Farm Game Help Window";
            this.ClientSize = new Size(500, 500);

            title = new Label
            {
                Text = "Farm Game Help Window",
                TextAlign = ContentAlignment.TopCenter,
                ForeColor = Color.Black,
                Font = new Font("Arial", 16),
                Location = new Point(100, 10),
                Size = new Size(300, 30)
            };
            population = new Label
            {
                Text = "Total Population: " + populationCnt.ToString(),
                ForeColor = Color.Black,
                Font = new Font("Arial", 12),
                Location = new Point(10, 50),
                Size = new Size(150, 50)
            };
            populationHelp = new Button
            {
                Text = "More",
                Location = new Point(160, 47),
                ForeColor = Color.Black
            };
            populationHelp.Click += popHelp_Click;

            this.Controls.Add(title);
            this.Controls.Add(population);
            this.Controls.Add(populationHelp);


            this.ResumeLayout(false);
        }
    }


    public partial class MainForm : Form
    {
        List<structure> assets = new();
        private PictureBox pictureBox;
        //private Bitmap image;
        private Bitmap farmImage;
        private Bitmap houseImage;
        private Bitmap factoryImage;
        private Bitmap roadImage;
        private Rectangle visibleRect;
        private Point lastMousePos;
        private bool isDragging;
        private const int clickThreshold = 5;
        private Button setFarmButton;
        private Button placeHouseButton;
        private Button placeFactoryButton;
        private Button placeRoadButton;
        private bool isPlacingFarm = false;
        private bool isPlacingHouse = false;
        private bool isPlacingFactory = false;
        private bool isPlacingRoad = false;
        private disabledLabel currAction;
        private disabledLabel moneyDisplay;
        private int money = 1000;
        private int farmCost = 50;
        private int houseCost = 50;
        private int factoryCost = 100;
        private int roadCost = 10;
        private int housePeopleLimit = 10;
        private float moneyPerPerson = 0.3f;
        private int totalPopulation = 0;
        private System.Windows.Forms.Timer gameTimer;
        private ToolTip toolTip;
        private Random rng;
        private float moveinrate = 0.3f;
        private int szWidth = 1000;
        private int szHeight = 1000;
        private Button showMoreDetails;
        private helpWindow help;

        public MainForm()
        {
            InitializeComponent();

            assets = new List<structure>();

            //image = new Bitmap("grass.jpg");
            farmImage = new Bitmap("farm.png");
            houseImage = new Bitmap("house.png");
            factoryImage = new Bitmap("factory.png");
            roadImage = new Bitmap("road.png");

            rng = new((int)DateTime.Now.Ticks);

            visibleRect = new Rectangle(0, 0, 800, 800);

            pictureBox = new PictureBox
            {
                Width = visibleRect.Width,
                Height = visibleRect.Height,
                BackColor = Color.Green
                //Image = CropImage(image, visibleRect)
            };

            pictureBox.MouseDown += PictureBox_MouseDown;
            pictureBox.MouseMove += PictureBox_MouseMove;
            pictureBox.MouseUp += PictureBox_MouseUp;

            setFarmButton = new Button
            {
                Text = "Place Farm",
                Location = new Point(700, 10),
                Size = new Size(90, 30)
            };
            setFarmButton.Click += SetFarmButton_Click;

            placeHouseButton = new Button
            {
                Text = "Place House",
                Location = new Point(700, 50),
                Size = new Size(90, 30)
            };
            placeHouseButton.Click += PlaceHouseButton_Click;

            placeFactoryButton = new Button
            {
                Text = "Place Factory",
                Location = new Point(700, 90),
                Size = new Size(90, 30)
            };
            placeFactoryButton.Click += PlaceFactoryButton_Click;

            placeRoadButton = new Button
            {
                Text = "Place Road",
                Location = new Point(700, 130),
                Size = new Size(90, 30)
            };
            placeRoadButton.Click += PlaceRoadButton_Click;

            showMoreDetails = new Button
            {
                Text = "Help",
                Location = new Point(700, 170),
                Size = new Size(90, 30)
            };
            showMoreDetails.Click += Help_Click;

            currAction = new disabledLabel
            {
                Text = "Now Placing: Nothing",
                Location = new Point(10, 10),
                BackColor = Color.Transparent,
                ForeColor = Color.Cyan,
                Font = new Font("Ariel", 20),
                Size = new Size(400, 50),
                Enabled = false,
            };
            currAction.setColor(Color.Cyan);
            pictureBox.Controls.Add(currAction);

            moneyDisplay = new disabledLabel
            {
                Text = "Money: " + money.ToString(),
                Location = new Point(10, 60),
                BackColor = Color.Transparent,
                ForeColor = Color.Cyan,
                Font = new Font("Ariel", 16),
                Size = new Size(400, 50),
                Enabled = false,
            };
            moneyDisplay.setColor(Color.Cyan);
            pictureBox.Controls.Add(moneyDisplay);

            this.Controls.Add(showMoreDetails);
            this.Controls.Add(setFarmButton);
            this.Controls.Add(placeHouseButton);
            this.Controls.Add(placeFactoryButton);
            this.Controls.Add(placeRoadButton);
            this.Controls.Add(pictureBox);

            gameTimer = new System.Windows.Forms.Timer
            {
                Interval = 1000
            };
            gameTimer.Tick += (sender, e) => UpdateFoodAndPeople();
            gameTimer.Start();

            toolTip = new ToolTip();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (gameTimer != null)
                {
                    gameTimer.Stop();
                    gameTimer.Dispose();
                }

                if (pictureBox != null)
                {
                    pictureBox.MouseDown -= PictureBox_MouseDown;
                    pictureBox.MouseMove -= PictureBox_MouseMove;
                    pictureBox.MouseUp -= PictureBox_MouseUp;
                    if (pictureBox.Image != null)
                    {
                        pictureBox.Image.Dispose();
                    }
                    pictureBox.Dispose();
                }

                DisposeBitmaps();
                DisposeControls();
            }
            base.Dispose(disposing);
        }
        private void Help_Click(object? sender, EventArgs e) 
        {
            help = new helpWindow();
            help.Show();
        }

        private void DisposeBitmaps()
        {
            farmImage?.Dispose();
            houseImage?.Dispose();
            factoryImage?.Dispose();
            roadImage?.Dispose();
        }

        private void DisposeControls()
        {
            setFarmButton?.Dispose();
            placeHouseButton?.Dispose();
            placeFactoryButton?.Dispose();
            placeRoadButton?.Dispose();
            currAction?.Dispose();
            moneyDisplay?.Dispose();
        }

        private void InitializeComponent()
        {
            this.SuspendLayout();
            this.ClientSize = new System.Drawing.Size(800, 800);
            this.MaximizeBox = false;
            this.Text = "Farm Game";
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.ResumeLayout(false);
        }

        private void SetFarmButton_Click(object? sender, EventArgs e)
        {
            isPlacingFarm = !isPlacingFarm;
            isPlacingHouse = false;
            isPlacingFactory = false;
            isPlacingRoad = false;
            UpdateCurrActionText();
            UpdatePictureBoxImage();
        }

        private void PlaceHouseButton_Click(object? sender, EventArgs e)
        {
            isPlacingHouse = !isPlacingHouse;
            isPlacingFarm = false;
            isPlacingFactory = false;
            isPlacingRoad = false;
            UpdateCurrActionText();
            UpdatePictureBoxImage();
        }

        private void PlaceFactoryButton_Click(object? sender, EventArgs e)
        {
            isPlacingFactory = !isPlacingFactory;
            isPlacingFarm = false;
            isPlacingHouse = false;
            isPlacingRoad = false;
            UpdateCurrActionText();
            UpdatePictureBoxImage();
        }

        private void PlaceRoadButton_Click(object? sender, EventArgs e)
        {
            isPlacingRoad = !isPlacingRoad;
            isPlacingFarm = false;
            isPlacingHouse = false;
            isPlacingFactory = false;
            UpdateCurrActionText();
            UpdatePictureBoxImage();
        }

        private void UpdateCurrActionText()
        {
            if (isPlacingFarm)
                currAction.Text = "Now Placing: Farm";
            else if (isPlacingHouse)
                currAction.Text = "Now Placing: House";
            else if (isPlacingFactory)
                currAction.Text = "Now Placing: Factory";
            else if (isPlacingRoad)
                currAction.Text = "Now Placing: Road";
            else
                currAction.Text = "Now Placing: Nothing";
        }

        private void UpdatePictureBoxImage()
        {
            DrawAllAssets();
        }

        private void PictureBox_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                lastMousePos = e.Location;
                isDragging = false;
            }
        }

        private void PictureBox_MouseMove(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int dx = e.X - lastMousePos.X;
                int dy = e.Y - lastMousePos.Y;

                if (Math.Abs(dx) > clickThreshold || Math.Abs(dy) > clickThreshold)
                {
                    isDragging = true;
                    visibleRect.X = Math.Max(0, Math.Min(szWidth - visibleRect.Width, visibleRect.X - dx));
                    visibleRect.Y = Math.Max(0, Math.Min(szHeight - visibleRect.Height, visibleRect.Y - dy));
                    UpdatePictureBoxImage();
                    lastMousePos = e.Location;
                }
            }
            else
            {
                bool found = false;
                foreach (var asset in assets)
                {
                    Rectangle assetRect = new Rectangle(asset.x, asset.y, asset.width, asset.height);
                    if (assetRect.Contains(e.Location))
                    {
                        string info = asset.type == LANDTYPE.HOUSE
                            ? $"House: {asset.h.occupation}/{asset.h.maxSz} people"
                            : asset.type == LANDTYPE.FARM
                                ? $"Farm: {asset.f.workers}/{asset.f.maxWorkers} workers"
                                : asset.type == LANDTYPE.FACTORY
                                    ? $"Factory: {asset.fact.workers}/{asset.fact.maxWorkers} workers"
                                    : "Road";

                        toolTip.SetToolTip(pictureBox, info);
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    toolTip.SetToolTip(pictureBox, "");
                }
            }
        }

        private void PictureBox_MouseUp(object? sender, MouseEventArgs e)
        {
            if (!isDragging)
            {
                if (isPlacingFarm && money >= farmCost)
                {
                    var newFarm = new structure
                    {
                        type = LANDTYPE.FARM,
                        x = (int)((e.X + visibleRect.X) / 100) * 100,
                        y = (int)((e.Y + visibleRect.Y) / 100) * 100,
                        width = farmImage.Width,
                        height = farmImage.Height,
                        f = new farm { remaining_food = 100, workers = 0, maxWorkers = 10 }
                    };
                    assets.Add(newFarm);
                    money -= farmCost;
                    UpdateMoneyDisplay();
                }
                else if (isPlacingHouse && money >= houseCost)
                {
                    var newHouse = new structure
                    {
                        type = LANDTYPE.HOUSE,
                        x = (int)((e.X + visibleRect.X) / 100) * 100,
                        y = (int)((e.Y + visibleRect.Y) / 100) * 100,
                        width = houseImage.Width,
                        height = houseImage.Height,
                        h = new house { occupation = 0, maxSz = housePeopleLimit }
                    };
                    assets.Add(newHouse);
                    money -= houseCost;
                    UpdateMoneyDisplay();
                }
                else if (isPlacingFactory && money >= factoryCost)
                {
                    var newFactory = new structure
                    {
                        type = LANDTYPE.FACTORY,
                        x = (int)((e.X + visibleRect.X) / 100) * 100,
                        y = (int)((e.Y + visibleRect.Y) / 100) * 100,
                        width = factoryImage.Width,
                        height = factoryImage.Height,
                        fact = new factory { workers = 0, maxWorkers = 30 }
                    };
                    assets.Add(newFactory);
                    money -= factoryCost;
                    UpdateMoneyDisplay();
                }
                else if (isPlacingRoad && money >= roadCost)
                {
                    var newRoad = new structure
                    {
                        type = LANDTYPE.ROAD,
                        x = (int)((e.X + visibleRect.X) / 100) * 100,
                        y = (int)((e.Y + visibleRect.Y) / 100) * 100,
                        width = roadImage.Width,
                        height = roadImage.Height,
                        r = new road { developed = true }
                    };
                    assets.Add(newRoad);
                    money -= roadCost;
                    UpdateMoneyDisplay();
                }
                DrawAllAssets();
            }
        }

        private void DrawAllAssets()
        {
            Bitmap bmp = new Bitmap(szWidth, szHeight);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                using (SolidBrush brush = new SolidBrush(Color.Green))
                {
                    g.FillRectangle(brush, 0, 0, szWidth, szHeight);
                }
                using (SolidBrush brush = new SolidBrush(Color.Red))
                {
                    g.FillRectangle(brush, 0, 0, szWidth, 10);
                    g.FillRectangle(brush, 0, szHeight - 10, szWidth, 10);
                    g.FillRectangle(brush, 0, 0, 10, szHeight);
                    g.FillRectangle(brush, szWidth - 10, 0, 10, szHeight);
                }
                foreach (var asset in assets)
                {
                    Bitmap assetImage = asset.type switch
                    {
                        LANDTYPE.FARM => farmImage,
                        LANDTYPE.HOUSE => houseImage,
                        LANDTYPE.FACTORY => factoryImage,
                        LANDTYPE.ROAD => roadImage
                    };

                    if (assetImage != null)
                    {
                        g.DrawImage(assetImage, new Point(asset.x, asset.y));
                    }
                }
            }
            if (pictureBox.Image != null)
            {
                pictureBox.Image.Dispose();
            }
            pictureBox.Image = CropImage(bmp, visibleRect);
            bmp.Dispose();
            pictureBox.Refresh();
        }

        private void UpdateMoneyDisplay()
        {
            moneyDisplay.Text = "Money: " + money.ToString();
        }

        private Bitmap CropImage(Bitmap source, Rectangle section)
        {
            return source.Clone(section, source.PixelFormat);
        }

        private void UpdateFoodAndPeople()
        {
            int totalWorkers = 0;
            int totalFood = 0;

            for (int i = 0; i < assets.Count; i++)
            {
                var asset = assets[i];

                if (asset.type == LANDTYPE.FARM)
                {
                    totalWorkers += asset.f.workers;
                    totalFood += asset.f.remaining_food;
                }
                if (asset.type == LANDTYPE.FACTORY)
                {
                    totalWorkers += asset.fact.workers;
                }
            }

            totalPopulation = 0;

            for (int i = 0; i < assets.Count; i++)
            {
                var asset = assets[i];

                if (asset.type == LANDTYPE.HOUSE && asset.h.occupation < asset.h.maxSz)
                {
                    int newOccupants = Math.Min(asset.h.maxSz - asset.h.occupation,
                        CalculateNewOccupants(asset));
                    asset.h.occupation = newOccupants;
                    totalPopulation += asset.h.occupation;
                    assets[i] = asset;
                }
            }

            money += (int)(totalWorkers * moneyPerPerson);
            UpdateMoneyDisplay();
            DrawAllAssets();
            if (help != null)
            {
                help.populationCnt = totalWorkers;
                help.Reload();
            }
        }

        private int CalculateNewOccupants(structure house)
        {
            float proximityFactor = 1.0f;
            foreach (var asset in assets)
            {
                if (asset.type == LANDTYPE.FACTORY)
                {
                    int distance = Math.Abs(house.x - asset.x) + Math.Abs(house.y - asset.y);
                    if (distance < 500)
                    {
                        proximityFactor *= 0.6f;
                    }
                }
            }

            if (rng.NextDouble() > proximityFactor)
            {
                // Polluted
                return (int)(-house.h.occupation * rng.NextDouble());
            }
            for (var i = 0; i < assets.Count; ++i)
            {
                var asset = assets[i];
                if(asset.type == LANDTYPE.FACTORY)
                {
                    asset.f.workers = 0;
                }
                if(asset.type == LANDTYPE.FARM)
                {
                    asset.fact.workers = 0;
                }
                assets[i] = asset;
            }
            int amtMove = 0;

            for (var i = 0; i < assets.Count; ++i)
            {
                var asset = assets[i];
                if (asset.type == LANDTYPE.FACTORY || asset.type == LANDTYPE.FARM)
                {
                    if (Math.Abs(house.x - asset.x) + Math.Abs(house.y - asset.y) <= 500)
                    {
                        if (!IsConnectedByPath(house, asset))
                        {
                            continue;
                        }
                        if (rng.NextDouble() < moveinrate)
                        {
                            if (asset.type == LANDTYPE.FACTORY)
                            {
                                asset.fact.workers += (int)((rng.NextDouble() < 1 - house.h.occupation / house.h.maxSz ? 1 : 0)
                                    * rng.NextDouble()
                                    * asset.fact.maxWorkers);
                                amtMove += asset.fact.workers;
                            }
                            else if (asset.type == LANDTYPE.FARM)
                            {
                                asset.f.workers += (int)((rng.NextDouble() < 1 - house.h.occupation / house.h.maxSz ? 1 : 0)
                                    * rng.NextDouble()
                                    * asset.f.maxWorkers);
                                amtMove += asset.f.workers;
                            }
                        }
                    }
                }
                assets[i] = asset;
            }

            return amtMove;
        }

        private bool IsConnectedByPath(structure start, structure end)
        {
            if (start.x == end.x && start.y == end.y)
                return true;

            int[] dx = { 0, 0, -1, 1 };
            int[] dy = { -1, 1, 0, 0 };

            Queue<Point> queue = new Queue<Point>();
            HashSet<Point> visited = new HashSet<Point>();

            queue.Enqueue(new Point(start.x, start.y));
            visited.Add(new Point(start.x, start.y));

            while (queue.Count > 0)
            {
                Point current = queue.Dequeue();

                for (int i = 0; i < 4; i++)
                {
                    int newX = current.X + dx[i];
                    int newY = current.Y + dy[i];
                    Point neighbor = new Point(newX, newY);

                    if (visited.Contains(neighbor))
                        continue;

                    structure? neighborAsset = assets.Find(asset => asset.x == newX && asset.y == newY);

                    if (neighborAsset.HasValue &&
                        (neighborAsset.Value.type == LANDTYPE.ROAD ||
                         neighborAsset.Value.type == LANDTYPE.FARM ||
                         neighborAsset.Value.type == LANDTYPE.HOUSE ||
                         neighborAsset.Value.type == LANDTYPE.FACTORY))
                    {
                        if (newX == end.x && newY == end.y)
                            return true;

                        queue.Enqueue(neighbor);
                        visited.Add(neighbor);
                    }
                }
            }

            return false;
        }
    }
}
