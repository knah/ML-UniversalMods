using MelonLoader;
using UnityEngine;
using UnityEngine.Rendering;

namespace RuntimeGraphicsSettings
{
    public static class RuntimeGraphicsSettings
    {
        public static void RegisterSettings()
        {
            var category = MelonPreferences.CreateCategory("GraphicsSettings", "Graphics settings");

            var msaaLevel = category.CreateEntry("MSAALevel", -1, "MSAA Level (1/2/4/8)");
            var allowMsaa = category.CreateEntry("AllowMSAA", true, "Enable MSAA");
            var anisoFilter = category.CreateEntry("AnisotropicFiltering", true, "Enable anisotropic filtering");
            var rtShadows = category.CreateEntry("RealtimeShadows", true, "Realtime shadows");
            var softShadows = category.CreateEntry("SoftShadows", true, "Soft shadows");
            var maxPixelLights = category.CreateEntry("PixelLights", -1, "Max pixel lights");
            var textureDecimation = category.CreateEntry("MasterTextureLimit", -1, "Texture decimation");
            var graphicsTier = category.CreateEntry("GraphicsTier", -1, "Graphics tier (1/2/3)");


            anisoFilter.OnValueChanged += (_, value) =>
            {
                QualitySettings.anisotropicFiltering = value ? AnisotropicFiltering.ForceEnable : AnisotropicFiltering.Disable;
            };
            QualitySettings.anisotropicFiltering = anisoFilter.Value ? AnisotropicFiltering.ForceEnable : AnisotropicFiltering.Disable;

            textureDecimation.OnValueChanged += (_, value) =>
            {
                if (value >= 0) QualitySettings.masterTextureLimit = value;
            };
            if (textureDecimation.Value >= 0) QualitySettings.masterTextureLimit = textureDecimation.Value;

            maxPixelLights.OnValueChanged += (_, value) =>
            {
                if (value >= 0) QualitySettings.pixelLightCount = value;
            };
            if (maxPixelLights.Value >= 0) QualitySettings.pixelLightCount = maxPixelLights.Value;

            graphicsTier.OnValueChanged += (_, value) =>
            {
                if (value > 0) Graphics.activeTier = (GraphicsTier)(value - 1);
            };
            if (graphicsTier.Value > 0) Graphics.activeTier = (GraphicsTier)(graphicsTier.Value - 1);

            void UpdateShadows()
            {
                QualitySettings.shadows = rtShadows.Value
                    ? softShadows.Value ? ShadowQuality.All : ShadowQuality.HardOnly
                    : ShadowQuality.Disable;
            }
            
            UpdateShadows();

            rtShadows.OnValueChangedUntyped += UpdateShadows;
            softShadows.OnValueChangedUntyped += UpdateShadows;

            void UpdateMsaa()
            {
                if (allowMsaa.Value)
                {
                    if (msaaLevel.Value > 0)
                        QualitySettings.antiAliasing = msaaLevel.Value;
                }
                else
                    QualitySettings.antiAliasing = 1;
            }

            msaaLevel.OnValueChangedUntyped += UpdateMsaa;
            allowMsaa.OnValueChangedUntyped += UpdateMsaa;
            UpdateMsaa();
        }
    }
}