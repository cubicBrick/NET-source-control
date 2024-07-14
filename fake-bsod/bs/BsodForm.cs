using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace bs
{
	public partial class BsodForm : Form
	{
		/// <summary>
		/// The duration the BSOD will be shown for. After this duration, its opacity is gradually reduced.
		/// </summary>
		private const int bsodVisibilityDurationMs = 4000;
		/// <summary>
		/// Time between the start of the application and when it will start listening for mouse input.
		/// </summary>
		private const int listenForMouseMovementDelayMs = 2000;

		private Timer updateTimer = new Timer();
		private Point initialCursorPosition;
		private List<FullscreenColor> fullscreenColorForms = new List<FullscreenColor>();


		public BsodForm()
		{
			InitializeComponent();

			updateTimer.Interval = 50;
			updateTimer.Tick += ListenForMouseMovement;

			this.GotFocus += (object sender, EventArgs e)=>  Cursor.Hide();
			this.LostFocus += (object sender, EventArgs e) => Cursor.Show();

			EnableMouseMovementMonitoring();
		}

		protected override void OnKeyDown(KeyEventArgs e)
		{
			if (e.KeyCode == Keys.Escape)
			{
				Application.Exit();
			}
			else
			{
				base.OnKeyDown(e);
			}
		}

		/// <summary>
		/// Begin monitoring mouse movement after a delay has expired.
		/// </summary>
		private async void EnableMouseMovementMonitoring()
		{
			await Task.Delay(listenForMouseMovementDelayMs);

			initialCursorPosition = Cursor.Position;
			updateTimer.Start();
		}

		private async void ListenForMouseMovement(object sender, System.EventArgs e)
		{
			if (Cursor.Position != initialCursorPosition)
			{
				//The position of the cursor has moved, which likely means that the user is using the computer.
				ShowBsod();
				updateTimer.Tick -= ListenForMouseMovement;
				await Task.Delay(bsodVisibilityDurationMs);
				updateTimer.Tick += FadeFormOpacity;
			}
		}

		/// <summary>
		/// Fade out opacity of all visible forms.
		/// This tells the user to stop panicking and that the BSOD is not real.
		/// </summary>
		private void FadeFormOpacity(object sender, System.EventArgs e)
		{
			this.Opacity -= 0.02;
			foreach (var form in fullscreenColorForms)
			{
				form.Opacity = this.Opacity;
			}

			if (this.Opacity <= 0.0)
			{
				Application.Exit();
			}
		}

		private void ShowBsod()
		{
			foreach (Screen screen in Screen.AllScreens)
			{
				if (screen.Primary)
				{
					this.Location = new Point(0, 0);
					this.Size = screen.Bounds.Size;
					CenterBsodText();
				}
				else
				{
					//Make a new form which makes all non-primary screens black.
					var form = new FullscreenColor(Color.Black, screen.Bounds.Size, screen.Bounds.Location);
					form.Show();
					fullscreenColorForms.Add(form);
				}
			}

			this.Show();
			//Keep keyboard focus on main form so that pressing escape is captured properly.
			this.Focus();
		}

		/// <summary>
		/// Makes the BSOD text centered in the form, using the form's size.
		/// </summary>
		private void CenterBsodText()
		{
			int halfBsodWidth = bsodTextBox.Width / 2;
			int halfFormWidth = this.Width / 2;
			int halfBsodHeight = bsodTextBox.Height / 2;
			int halfFormHeight = this.Height / 2;

			Point position = new Point();
			position.X = halfFormWidth - halfBsodWidth;
			position.Y = halfFormHeight - halfBsodHeight;
			bsodTextBox.Location = position;
		}
	}
}
