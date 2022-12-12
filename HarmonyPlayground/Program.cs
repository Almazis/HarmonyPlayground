using System.Diagnostics;

class Program
{
    static void Main(string[] args)
    {
        MyPatcher.DoPatching();
        var r1 = DtNow();
        var r2 = StrDt2013();
        Debug.Assert(r1 == new DateTime(2011, 1,1));
        Debug.Assert(r2 == "patched");
    }

    static private DateTime DtNow()
    {
        return DateTime.Now;
    }

    static private string StrDt2013()
    {
        DateTime dt = new DateTime(2013, 12, 22);
        return dt.ToLongDateString();
    }

}
    