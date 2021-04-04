using System;
using System.Runtime.InteropServices;
using Harmony;
using MelonLoader;
using NoSteamAtAll;

[assembly:MelonGame]
[assembly:MelonInfo(typeof(NoSteamAtAllMod), "No Steam. At all.", "1.0.1", "knah")]

namespace NoSteamAtAll
{
    public class NoSteamAtAllMod : MelonMod
    {
        [DllImport("kernel32", SetLastError=true, CharSet = CharSet.Ansi)]
        static extern IntPtr LoadLibrary([MarshalAs(UnmanagedType.LPStr)]string lpFileName);
        
        [DllImport("kernel32", CharSet=CharSet.Ansi, ExactSpelling=true, SetLastError=true)]
        static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        public override void OnApplicationStart()
        {
            var library = LoadLibrary(MelonUtils.GameDirectory + "\\Plugins\\steam_api64.dll");
            if (library == IntPtr.Zero)
            {
                MelonLogger.LogError("Library load failed");
                return;
            }
            var names = new[]
            {
                "SteamAPI_Init", 
                "SteamAPI_RestartAppIfNecessary",
                "SteamAPI_GetHSteamUser",
                "SteamAPI_RegisterCallback",
                "SteamAPI_UnregisterCallback",
                "SteamAPI_RunCallbacks",
                "SteamAPI_Shutdown"
            };
            
            foreach (var name in names)
            {
                unsafe
                {
                    var address = GetProcAddress(library, name);
                    if (address == IntPtr.Zero)
                    {
                        MelonLogger.LogError($"Procedure {name} not found");
                        continue;
                    }
                    Imports.Hook((IntPtr) (&address),
                        AccessTools.Method(typeof(NoSteamAtAllMod), nameof(InitFail)).MethodHandle
                            .GetFunctionPointer());
                }
            }
        }

        public static bool InitFail()
        {
            return false;
        }
    }
}