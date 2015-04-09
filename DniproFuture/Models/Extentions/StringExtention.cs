using System.Text.RegularExpressions;

namespace DniproFuture.Models.Extentions
{
    public static class StringExtention
    {
        public static string GetStringForUrl(this string title)
        {
            Regex pattern = new Regex("[.,!?:;-]|[\"]");

            string s = pattern.Replace(title, " ");
            pattern = new Regex("[(\\s+)]");
            s = pattern.Replace(s, "-");

            return s;
        }

        public static string GetTextWithoutTags(this string text)
        {
            Regex pattern = new Regex(@"<(?!\/?(>)).*?>");
            string result = pattern.Replace(text, string.Empty);
            return result;
        }
    }
}