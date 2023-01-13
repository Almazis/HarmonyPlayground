using System.Diagnostics;
using System.Reflection.Emit;
using HarmonyLib;

class ppp
{
    private static string[] ress = new string[] { "11", "222" };
    private static int counter = 0;
    static void MyPostfix(ref string __result)
    {
        __result = ress[counter];
        counter++;
    }

    static string MyExtraMethod()
    {
        var res = ress[counter];
        counter++;
        return res;
    }
    static System.Reflection.MethodInfo m_MyExtraMethod = SymbolExtensions.GetMethodInfo(() => MyExtraMethod());
    static IEnumerable<CodeInstruction> MyTranspiler(IEnumerable<CodeInstruction> instructions)
    {
        yield return new CodeInstruction(OpCodes.Call, m_MyExtraMethod);
        yield return new CodeInstruction(OpCodes.Ret);
    }
}

class NowPatch
{
    private static DateTime[] ress = new DateTime[] {new DateTime(2012, 1, 1), new DateTime(2007,12,1)};
    private static int counter = 0;
    static void MyPostfix(ref DateTime __result)
    {
        __result = ress[counter];
        counter++;
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
    
    static void Main2(string[] args)
    {
        var harmony = new Harmony("com.example.patch");

        var mOriginal = AccessTools.Method(typeof(DateTime), "ToLongDateString"); // if possible use nameof() here
        var mPostfix = AccessTools.Method(typeof(ppp), "MyPostfix");

        harmony.Patch(mOriginal, postfix: new HarmonyMethod(mPostfix));
        Console.WriteLine(StrDt2013());
        Console.WriteLine(StrDt2013());
        Console.ReadLine();
    }

    static void Main3(string[] args)
    {
        DateTime dtMain = new DateTime(2012, 12, 12);
        var harmony = new Harmony("com.example.patch");
        
        var mOriginal = SymbolExtensions.GetMethodInfo(() => dtMain.ToLongDateString());
        var mPostfix = AccessTools.Method(typeof(ppp), "MyPostfix");

        harmony.Patch(mOriginal, postfix: new HarmonyMethod(mPostfix));
        Console.WriteLine(dtMain.ToLongDateString());
        Console.WriteLine(dtMain.ToLongDateString());

        Console.WriteLine(StrDt2013());
        Console.ReadLine();
    }
    
    static void Main4(string[] args)
    {
        var harmony = new Harmony("com.example.patch");
        
        // var mOriginal = SymbolExtensions.GetMethodInfo(() => DtNow());
        var mOriginal = AccessTools.PropertyGetter(typeof(DateTime), "Now");

        var mPostfix = AccessTools.Method(typeof(NowPatch), "MyPostfix");

        harmony.Patch(mOriginal, postfix: new HarmonyMethod(mPostfix));
        Console.WriteLine(DtNow());
        Console.WriteLine(DtNow());

        // Console.WriteLine(StrDt2013());
        Console.ReadLine();
    }
    
    static void Main(string[] args)
    {
        DateTime dtMain = new DateTime(2012, 12, 12);
        var harmony = new Harmony("com.example.patch");

        var mOriginal = SymbolExtensions.GetMethodInfo(() => dtMain.ToLongDateString());
        var mPostfix = AccessTools.Method(typeof(ppp), "MyTranspiler");
        
        harmony.Patch(mOriginal, transpiler: new HarmonyMethod(mPostfix));

        Console.WriteLine(dtMain.ToLongDateString());
        Console.WriteLine(dtMain.ToLongDateString());

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
    