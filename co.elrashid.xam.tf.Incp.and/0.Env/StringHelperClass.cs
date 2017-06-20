using System.Linq;
internal static class stringUtils
{
    internal static string[] Split(this string me, string reg, bool trim)
    {
        var reslt = System.Text.RegularExpressions.Regex.Split(me, reg);
        var  reslt1 = trim? reslt.Where((w,index) => index == 0 || w.Length > 0)?.ToArray() : reslt;
        return reslt1;
    }
}