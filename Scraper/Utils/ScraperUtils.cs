using AngleSharp.Dom;
using Common.Models;

namespace Scraper.Utils;


public static class ScraperUtils
{
    // Check if the given string is a valid URL, and if so, return it
    // If not, return it converted to a URL using baseUrl as the base.
    public static string GetUrl(string url, string baseUrl)
    {
        if (Uri.IsWellFormedUriString(url, UriKind.Absolute))
        {
            return url;
        }
        else
        {
            return new Uri(new Uri(baseUrl), url).ToString();
        }
    }


    // Parse a string into a double, and return the default value if it fails
    public static double ParseDouble(string text, double defaultValue)
    {
        double value;
        if (double.TryParse(text, out value))
        {
            return value;
        }
        else
        {
            return defaultValue;
        }
    }

    public static string ReadNodesUntilNextSection(INodeList nodes, int startIndex)
    {
        string result = "";
        for (int i = startIndex; i < nodes.Length; i++)
        {
            if (nodes[i].NodeName.ToLower() == "h4" ||
                nodes[i].NodeName.ToLower() == "h5")
            {
                return result;
            }
            else
            {
                result += nodes[i].TextContent;
            }
        }

        return result;
    }

    public static bool IsRedirectPage(Journal journal, IDocument page) {
        return (journal.Url != page.Url);
    }

}
