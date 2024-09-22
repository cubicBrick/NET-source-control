using System.Windows.Forms;

namespace amooogle
{
    partial class Main : Form
    {
        public Main()
        {
            InitilizeSetup();
            InitilizePages();
            InitilizeUI();
        }
        ~Main()
        {}
        private void InitilizeSetup() {
            this.Size = new(1000, 700);
            this.Text = "Amooogle Search: Powering your friendly Amough since 2024. (Made by Jayden)";
        }
    }
}