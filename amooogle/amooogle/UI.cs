using System.Reflection.Metadata.Ecma335;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml.Linq;

namespace amooogle
{
    partial class Main : Form
    {
        private TextBox SearchMain;
        private Button SearchGo;
        private const int SEARCH_LEFT_OFF = 25;
        private const int SEARCH_TOP_OFF = 25;
        List<int> searchLines = new();
        List<int> searchLineWhere = new();
        bool isOnPage = false;
        private void InitilizeUI() {
            SearchMain = new TextBox();
            SearchMain.Height = 50;
            SearchMain.Width = this.Width - 180;
            SearchMain.Location = new Point(SEARCH_LEFT_OFF, SEARCH_TOP_OFF);
            SearchGo = new Button();
            SearchGo.Height = 23;
            SearchGo.Width = 70;
            SearchGo.Text = "Go!";
            SearchGo.Location = new Point(this.ClientSize.Width - 110, SEARCH_TOP_OFF+1);
            SearchGo.Click += new EventHandler(OnSearch);
            mainPage.MouseDown += new MouseEventHandler(mainPage_MouseDown);
            this.Controls.Add(SearchMain);
            this.Controls.Add(SearchGo);
        }
        private void mainPage_MouseDown(object? sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                int p = mainPage.GetCharIndexFromPosition(e.Location);
                int line = mainPage.GetLineFromCharIndex(p);
                if (searchLines.Contains(line) && !isOnPage)
                {
                    ShowPage(searchLineWhere[searchLines.IndexOf(line)]);
                    isOnPage = true;
                }
            }
        }
        private void ShowSearchResults(List<int> order)
        {
            isOnPage = false;
            string htmlSearchCode = "<h1>Search Results</h1> <br> ";
            int currLine = 2;
            searchLines.Clear();
            searchLineWhere.Clear();
            foreach (int item in order)
            {
                searchLines.Add(currLine);
                searchLineWhere.Add(item);
                currLine += 5 + Regex.Count(pages[item].description, "<br> | \n") + Regex.Count(pages[item].title, "<br> | \n");
                htmlSearchCode += "<h2>" + pages[item].title + "</h2> <br> " + pages[item].description + " <br> <br> <br> ";
            }
            mainPage.Clear();
            mainPage.Rtf = Convert.HTMLtoRTB(htmlSearchCode);
        }
        private void OnSearch(object? sender, EventArgs e)
        {
            List<int> closest = ClosestPageId(SearchMain.Text);
            ShowSearchResults(closest);
        }
    }
}