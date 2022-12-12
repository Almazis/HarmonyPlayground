using System;
using HarmonyLib;


public class MyPatcher
{
    // make sure DoPatching() is called at start either by
    // the mod loader or by your injector

    public static void DoPatching()
    {
        var harmony = new Harmony("com.example.patch");
        harmony.PatchAll();
    }
}

[HarmonyPatch(typeof(DateTime))]
[HarmonyPatch("ToLongDateString")] // if possible use nameof() here
class PatchToLong
{
    static void Postfix(ref string __result)
    {
        __result = "patched";
    }
}

[HarmonyPatch(typeof(DateTime))]
[HarmonyPatch("Now", MethodType.Getter)]
class PatchNow
{
    static void Postfix(ref DateTime __result)
    {
        __result = new DateTime(2011, 1, 1);
    }
}
    
