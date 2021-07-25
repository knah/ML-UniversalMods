using MelonLoader;
using RuntimeGraphicsSettings;

[assembly:MelonInfo(typeof(RuntimeGraphicsSettingsMod), "Runtime Graphics Settings", "0.2.2", "knah", "https://github.com/knah/ML-UniversalMods")]
[assembly:MelonGame] // universal
namespace RuntimeGraphicsSettings
{
    public class RuntimeGraphicsSettingsMod : MelonMod
    {
        public override void OnApplicationStart()
        {
            RuntimeGraphicsSettings.RegisterSettings();
        }
    }
}