using System.Text.Encodings.Web;
using System.Text.RegularExpressions;
using System.Web;
using Windows.ApplicationModel.Chat;
using Windows.UI.Composition.Interactions;

namespace amooogle
{
    class Convert
    {
        public static string HTMLtoRTB(string html)
        {
            string rtfHeader = @"{\rtf1\ansi\ansicpg1252\deff0\nouicompat\deflang1033";
            string rtfBody = "";

            // Handle headings
            rtfBody = Regex.Replace(html, @"<h1>(.*?)<\/h1>", @"{\b\fs36 $1\par}");
            rtfBody = Regex.Replace(rtfBody, @"<h2>(.*?)<\/h2>", @"{\b\fs28 $1\par}");
            rtfBody = Regex.Replace(rtfBody, @"<h3>(.*?)<\/h3>", @"{\b\fs24 $1\par}");
            rtfBody = Regex.Replace(rtfBody, @"<h4>(.*?)<\/h4>", @"{\b\fs20 $1\par}"); // Added support for h4
            rtfBody = Regex.Replace(rtfBody, @"<h5>(.*?)<\/h5>", @"{\b\fs16 $1\par}"); // Added support for h5
            rtfBody = Regex.Replace(rtfBody, @"<h6>(.*?)<\/h6>", @"{\b\fs12 $1\par}"); // Added support for h6

            // Handle basic formatting
            rtfBody = Regex.Replace(rtfBody, @"<b>(.*?)<\/b>", @"{\b $1}");
            rtfBody = Regex.Replace(rtfBody, @"<i>(.*?)<\/i>", @"{\i $1}");
            rtfBody = Regex.Replace(rtfBody, @"<u>(.*?)<\/u>", @"{\ul $1}");
            rtfBody = Regex.Replace(rtfBody, @"<strike>(.*?)</strike>", @"{\strike $1}"); // Added support for strikethrough

            // Handle line breaks and paragraphs
            rtfBody = Regex.Replace(rtfBody, @"<br\s*\/?>", @"\par");
            rtfBody = Regex.Replace(rtfBody, @"<p>(.*?)</p>", @"$1\par"); // Added support for <p> tags

            // Handle lists
            rtfBody = Regex.Replace(rtfBody, @"<ul>(.*?)</ul>", @"{\list\pntext\f0\fs20\fi-360\li720\sa200\tab\ulnone\ul0\fi-360\li720\sa200\tab $1}");
            rtfBody = Regex.Replace(rtfBody, @"<li>(.*?)</li>", @"{\*\listlevel0\*\listsimple $1\par}");

            // Handle tables
            rtfBody = Regex.Replace(rtfBody, @"<table>(.*?)</table>", @"{\table $1}");
            rtfBody = Regex.Replace(rtfBody, @"<tr>(.*?)</tr>", @"{\tr $1}");
            rtfBody = Regex.Replace(rtfBody, @"<td>(.*?)</td>", @"{\cell $1}");

            string rtfFooter = "}";

            return rtfHeader + rtfBody + rtfFooter;
        }
    }
    struct Page
    {
        public string title;
        public string htmlCode;
        public string description;
        public Page(string html, string title)
        {
            this.htmlCode = html;
            this.title = title;
            this.description = "This page does not have a description.";
        }
        public Page(string html, string title, string description)
        {
            this.htmlCode = html;
            this.title = title;
            this.description = description;
        }
    }
    enum ErrorCode
    {
        OK = 200, E_404_NOTFOUND = 404, E_501_SERVERERROR = 501, E_502_FORBIDDEN = 502
    }
    partial class Main : Form
    {
        RichTextBox mainPage;
        private void InitilizePages()
        {
            mainPage = new();
            mainPage.Location = new(SEARCH_LEFT_OFF, SEARCH_TOP_OFF + 50);
            mainPage.Size = new(this.ClientSize.Width - SEARCH_LEFT_OFF * 2, this.ClientSize.Height - SEARCH_TOP_OFF * 2 - 100);
            mainPage.ReadOnly = true;
            this.Controls.Add(mainPage);
        }
        private ErrorCode ShowPage(int id)
        {
            try
            {
                if (pages == null || !pages.ContainsKey(id))
                {
                    return ErrorCode.E_404_NOTFOUND;
                }
                mainPage.Rtf = "";
                Page p = pages[id];
                mainPage.Rtf = Convert.HTMLtoRTB(p.htmlCode);
                return ErrorCode.OK;
            }
            catch (Exception e)
            {
                return ErrorCode.E_501_SERVERERROR;
            }
        }
        private List<int> ClosestPageId(string search)
        {
            List<int> res = new(pages.Keys);
            res.Sort(delegate (int x, int y)
            {
                return priority(pages[x].title.ToLower(), search.ToLower()).CompareTo(priority(pages[y].title.ToLower(), search.ToLower()));
            });
            return res;
        }

        private int LevenshteinDistance(string a, string b)
        {
            if (string.IsNullOrEmpty(a))
                return b.Length;
            if (string.IsNullOrEmpty(b))
                return a.Length;

            int[,] distance = new int[a.Length + 1, b.Length + 1];

            for (int i = 0; i <= a.Length; i++)
                distance[i, 0] = i;
            for (int j = 0; j <= b.Length; j++)
                distance[0, j] = j;

            for (int i = 1; i <= a.Length; i++)
            {
                for (int j = 1; j <= b.Length; j++)
                {
                    int cost = (a[i - 1] == b[j - 1]) ? 0 : 1;
                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }

            return distance[a.Length, b.Length];
        }

        private float priority(string title, string search)
        {
            string lowerTitle = title.ToLower();
            string lowerSearch = search.ToLower();

            int levenshteinDistance = LevenshteinDistance(lowerTitle, lowerSearch);

            float lengthDifference = Math.Abs(lowerTitle.Length - lowerSearch.Length);

            bool isSubstring = lowerTitle.Contains(lowerSearch);

            if (isSubstring)
            {
                return levenshteinDistance / 2.0f;
            }

            float normalizedDistance = (float)levenshteinDistance / Math.Max(lowerTitle.Length, lowerSearch.Length);

            return normalizedDistance + lengthDifference;
        }

    }
}