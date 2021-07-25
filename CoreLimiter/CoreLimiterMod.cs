using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using CoreLimiter;
using MelonLoader;

[assembly:MelonInfo(typeof(CoreLimiterMod), "Core Limiter", "1.0.2", "knah", "https://github.com/knah/ML-UniversalMods")]
[assembly:MelonGame]

namespace CoreLimiter
{
    public class CoreLimiterMod : MelonMod
    {
        public override void OnApplicationStart()
        {
            var category = MelonPreferences.CreateCategory("CoreLimiter", "Core Limiter");
            
            var maxCores = category.CreateEntry("MaxCores", Math.Max(1, Environment.ProcessorCount / 4), "Maximum cores");
            var useBothHyperthreads = category.CreateEntry("UseBothHyperthreads", true, "Use both hyperthreads");
            var useFirstCores = category.CreateEntry("UseFirstCores", false, "Use first X cores (instead of last)");
            var specificCores = category.CreateEntry("SpecificCores", "", "Use specific cores (comma separated, i.e. '0,2,4,6'; overrides other settings)");
            
            MelonLogger.Msg($"Have {Environment.ProcessorCount} processor cores");

            void UpdateState() => ApplyAffinity(maxCores.Value, useBothHyperthreads.Value, useFirstCores.Value, specificCores.Value);

            maxCores.OnValueChangedUntyped += UpdateState;
            useBothHyperthreads.OnValueChangedUntyped += UpdateState;
            useFirstCores.OnValueChangedUntyped += UpdateState;
            specificCores.OnValueChangedUntyped += UpdateState;
            
            UpdateState();
        }

        private static long GetMaskAuto(int coreCount, bool useHyperthreads, bool useFirstCores)
        {
            var processorCount = Environment.ProcessorCount;
            long mask = 0;

            var startBit = useFirstCores ? 0 : processorCount - coreCount * 2;
            var endBit = useFirstCores ? coreCount * 2 : processorCount;
            for (var i = startBit; i < endBit; i += 2)
            {
                mask |= 1L << i;
                if (useHyperthreads)
                    mask |= 1L << (i + 1);
            }

            return mask;
        }

        private static long GetMaskManual(string input)
        {
            long mask = 0;
            foreach (var s in input.Split(','))
            {
                if (int.TryParse(s, out var i))
                    mask |= 1L << i;
            }

            return mask;
        }

        private static void ApplyAffinity(int coreCount, bool useHyperthreads, bool useFirstCores, string specificCores)
        {
            var mask = string.IsNullOrEmpty(specificCores) ? GetMaskAuto(coreCount, useHyperthreads, useFirstCores) : GetMaskManual(specificCores);

            if (mask == 0) mask = 1; // don't set empty masks
            
            var process = Process.GetCurrentProcess().Handle;
            MelonLogger.Msg($"Assigning affinity mask: {mask}");
            SetProcessAffinityMask(process, new IntPtr(mask));
        }
        
        [DllImport("kernel32.dll", CallingConvention = CallingConvention.StdCall)]
        private static extern bool SetProcessAffinityMask(IntPtr hProcess, IntPtr dwProcessAffinityMask);
    }
}