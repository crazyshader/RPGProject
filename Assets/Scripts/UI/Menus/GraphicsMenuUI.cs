using System;
using System.Collections.Generic;
using RPG.Graphics;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using static RPG.Graphics.GraphicsSettingsSO;

namespace RPG.UI.Menus
{
    public class GraphicsMenuUI : MonoBehaviour
    {
        [SerializeField] GraphicsSettingsSO graphicsSettingsSO;
        [SerializeField] Slider brightnessSlider;
        [SerializeField] TMP_Dropdown windowModeDropdown;
        [SerializeField] TMP_Dropdown resolutionDropdown;
        [SerializeField] TMP_Dropdown vSyncDropdown;
        [SerializeField] TMP_Dropdown bloomDropdown;
        [SerializeField] TMP_Dropdown vignetteDropdown;
        [SerializeField] TMP_Dropdown depthOfFieldDropdown;

        void Start()
        {
            SetGraphicsSettings();

            brightnessSlider.onValueChanged.AddListener(graphicsSettingsSO.SetBrightness);
            windowModeDropdown.onValueChanged.AddListener(graphicsSettingsSO.SetFullScreenMode);
            resolutionDropdown.onValueChanged.AddListener(graphicsSettingsSO.SetResolution);
            vSyncDropdown.onValueChanged.AddListener(graphicsSettingsSO.SetVSyncMode);
            bloomDropdown.onValueChanged.AddListener(graphicsSettingsSO.SetBloomMode);
            vignetteDropdown.onValueChanged.AddListener(graphicsSettingsSO.SetVignetteMode);
            depthOfFieldDropdown.onValueChanged.AddListener(graphicsSettingsSO.SetDepthOfFieldMode);
        }

        void SetGraphicsSettings()
        {
            brightnessSlider.value = graphicsSettingsSO.GetBrightness();

            FillDropdown(windowModeDropdown, typeof(FullScreenMode));
            windowModeDropdown.value = (int) graphicsSettingsSO.GetFullScreenMode();

            FillDropdown(resolutionDropdown, graphicsSettingsSO.GetResolutionProfiles());
            resolutionDropdown.value = graphicsSettingsSO.GetCurrentResolutionProfile();

            FillDropdown(vSyncDropdown, typeof(VSyncMode));
            vSyncDropdown.value = (int) graphicsSettingsSO.GetVSyncMode();

            FillDropdown(bloomDropdown, typeof(BloomMode));
            bloomDropdown.value = (int) graphicsSettingsSO.GetBloomMode();

            FillDropdown(vignetteDropdown, typeof(VignetteMode));
            vignetteDropdown.value = (int) graphicsSettingsSO.GetVignetteMode();

            FillDropdown(depthOfFieldDropdown, typeof(DepthOfFieldMode));
            depthOfFieldDropdown.value = (int) graphicsSettingsSO.GetDepthOfFieldMode();
        }

        void FillDropdown(TMP_Dropdown dropdown, Type enumType)
        {
            dropdown.ClearOptions();

            foreach(var value in Enum.GetValues(enumType))
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData(value.ToString()));
            }
        }

        void FillDropdown<T>(TMP_Dropdown dropdown, IEnumerable<T> values)
        {
            dropdown.ClearOptions();

            foreach(var value in values)
            {
                dropdown.options.Add(new TMP_Dropdown.OptionData(value.ToString()));
            }
        }
    }
}
