using System.Drawing;
using System.Windows.Forms;

namespace bs
{
	/// <summary>
	/// Simple form which contains nothing but a background color.
	/// </summary>
	public partial class FullscreenColor : Form
	{
		public FullscreenColor(Color color, Size size, Point position)
		{
			InitializeComponent();

			this.Size = size;
			this.Location = position;
			this.BackColor = color;
		}
	}
}
