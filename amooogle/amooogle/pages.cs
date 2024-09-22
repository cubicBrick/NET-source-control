namespace amooogle
{
    partial class Main : Form
    {
        Dictionary<int, Page> pages = new Dictionary<int, Page>()
        {
            {0, new Page("<h1>Amo0ogle is cool!</h1>", "Amooogle Home Page", "Opinion on Amooogle") },
            {1, new Page("<h1><strike>Alien?</strike>Amough is a person(duh)</h1><br>" +
                " But is he? How do we know that Amough is not an alien<br>" +
                " Amough doesn't say that he isn't!", "Amough is an alien?", "Is Amough an alien???") },
            {2, new Page("<h1>All about ducks</h1><br> " +
                "Duck is the common name for numerous species of waterfowl in the family Anatidae. Ducks are generally smaller and shorter-necked than swans and geese, which are members of the same family. Divided among several subfamilies, they are a form taxon; they do not represent a monophyletic group (the group of all descendants of a single common ancestral species), since swans and geese are not considered ducks. Ducks are mostly aquatic birds, and may be found in both fresh water and sea water.", "All about ducks!", "Everything you'll ever need to know about ducks")}
        };
    }
}