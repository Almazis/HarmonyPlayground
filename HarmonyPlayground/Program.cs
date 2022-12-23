using System.Diagnostics;
using System.Reflection.Emit;
using HarmonyLib;

class ppp
{
    private static List<string> ress = new List<string>() { "11", "222" };
        
    static void MyPostfix(ref string __result)
    {
        __result = ress[0];
        ress.RemoveAt(0);
    }

}

class Program
{
    static void Main1(string[] args)
    {
        MyPatcher.DoPatching();
        var r1 = DtNow();
        var r2 = StrDt2013();
        Debug.Assert(r1 == new DateTime(2011, 1,1));
        Debug.Assert(r2 == "patched");
    }
    
    static void Main(string[] args)
    {
        var harmony = new Harmony("com.example.patch");

        var mOriginal = AccessTools.Method(typeof(DateTime), "ToLongDateString"); // if possible use nameof() here
        var mPostfix = AccessTools.Method(typeof(ppp), "MyPostfix");

        harmony.Patch(mOriginal, postfix: new HarmonyMethod(mPostfix));
        Console.WriteLine(StrDt2013());
        Console.WriteLine(StrDt2013());
        Console.ReadLine();
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
    