using System;
using System.Linq;
using System.Reflection;
using HWIDPatch;
using MelonLoader;
using UnhollowerBaseLib;
using UnityEngine;

[assembly:MelonInfo(typeof(HwidPatchMod), "HWIDPatch", "1.0.2", "knah", "https://github.com/knah/ML-UniversalMods")]
[assembly:MelonGame]

namespace HWIDPatch
{
    public class HwidPatchMod : MelonMod
    {
        private static Il2CppSystem.Object ourGeneratedHwidString;
        
        public override unsafe void OnApplicationStart()
        {
            try
            {
                var category = MelonPreferences.CreateCategory("HWIDPatch", "HWID Patch");
                var hwidEntry = category.CreateEntry("HWID", "", is_hidden: true);

                var newId = hwidEntry.Value;
                if (newId.Length != SystemInfo.deviceUniqueIdentifier.Length)
                {
                    var random = new System.Random(Environment.TickCount);
                    var bytes = new byte[SystemInfo.deviceUniqueIdentifier.Length / 2];
                    random.NextBytes(bytes);
                    newId = string.Join("", bytes.Select(it => it.ToString("x2")));
                    MelonLogger.Msg("Generated and saved a new HWID");
                    hwidEntry.Value = newId;
                    category.SaveToFile(false);
                }

                ourGeneratedHwidString = new Il2CppSystem.Object(IL2CPP.ManagedStringToIl2Cpp(newId));

                var icallName = "UnityEngine.SystemInfo::GetDeviceUniqueIdentifier";
                var icallAddress = IL2CPP.il2cpp_resolve_icall(icallName);
                if (icallAddress == IntPtr.Zero)
                {
                    MelonLogger.Error("Can't resolve the icall, not patching");
                    return;
                }
                
                MelonUtils.NativeHookAttach((IntPtr) (&icallAddress),
                    typeof(HwidPatchMod).GetMethod(nameof(GetDeviceIdPatch),
                        BindingFlags.Static | BindingFlags.NonPublic)!.MethodHandle.GetFunctionPointer());

                MelonLogger.Msg("Patched HWID; below two should match:");
                MelonLogger.Msg($"Current: {SystemInfo.deviceUniqueIdentifier}");
                MelonLogger.Msg($"Target:  {newId}");
            }
            catch (Exception ex)
            {
                MelonLogger.Error(ex.ToString());
            }
        }

        private static IntPtr GetDeviceIdPatch() => ourGeneratedHwidString.Pointer;
    }
}