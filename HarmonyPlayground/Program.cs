using MonoMod.RuntimeDetour;
using MonoMod.Utils;
using System;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.ComponentModel;

class ppp
{
    private static string[] ress = new string[] { "11", "222" };
    private static int counter = 0;
    static void MyPostfix(ref string __result)
    {
        __result = ress[counter];
        counter++;
    }

    static public string MyExtraMethod()
    {
        var res = ress[counter];
        counter++;
        return res;
    }
}

class Program
{
    static void Main1(string[] args)
    {

        IDetour detour1 = new Detour(
                    () => default(DateTime).ToLongDateString(),
                    () => ppp.MyExtraMethod()
        );
        detour1.GenerateTrampoline<Action<string, string>>();
        var s1 = StrDt2013();
        var s2 = StrDt2013();

        Console.WriteLine(s1);
        Console.WriteLine(s2);

        detour1.Undo();
        Console.WriteLine(StrDt2013());
        Console.ReadLine();
    }
    
    static void Main(string[] args)
    {
        MethodBase bTo = typeof(Program).GetMethod("not_rand", BindingFlags.Static | BindingFlags.Public);
        IntPtr ptrTo = bTo.MethodHandle.GetFunctionPointer();
        typeof(Program).GetMethod("not_rand", BindingFlags.Static | BindingFlags.Public,
                                                null, new[] { typeof(void) }, null);

        DynDll.TryOpenLibrary("libc.so.6", out IntPtr libc);
            // !(PlatformHelper.Is(Platform.MacOS) && DynDll.TryOpenLibrary("/usr/lib/libc.dylib", out libc)) &&
            // !DynDll.TryOpenLibrary($"libc.{PlatformHelper.LibrarySuffix}", out libc))
            // return;

        NativeDetour d = new NativeDetour(
            libc.GetFunction("rand"),
            ptrTo,
            new NativeDetourConfig() {
                ManualApply = false
            }
        );
        var v1 = libc_rand();
        var v2 = libc_rand();
        var v3 = libc_rand();
        var v4 = libc_rand();

        Console.WriteLine(v1);
        Console.WriteLine(v2);
        Console.WriteLine(v3);
        Console.WriteLine(v4);
        
        d.Undo();
        Console.WriteLine(libc_rand());
        
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

    public static int not_rand() {
        return -1;
    }
    [DllImport("libc", EntryPoint = "rand", CallingConvention = CallingConvention.Cdecl)] 
    public static extern int libc_rand();
}
    