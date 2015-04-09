namespace DniproFuture.Models.Extentions
{
    public static class NeedHelpLocalSetExtention
    {
        public static string GetFullName(this NeedHelpLocalSet local)
        {
            return string.Format("{0} {1}", local.FirstName, local.LastName);
        }
    }
}