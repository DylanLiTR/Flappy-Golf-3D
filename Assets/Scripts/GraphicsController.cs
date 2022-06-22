using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GraphicsController : MonoBehaviour
{
    [SerializeField] Slider brightnessSlider;
    [SerializeField] TextMeshProUGUI brightnessValue;
    [SerializeField] float defaultBrightness = 100f;

    [SerializeField] TMP_Dropdown qualityDropdown;
    [SerializeField] Toggle fullscreenToggle;

    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    void Start()
    {
        // Get screen resolutions and clear all dropdown options
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

        // Add dropdown options and select the option corresponding to the current resolution
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; ++i)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    // Apply resolution change
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    // Apply brightness change
    public void SetBrightness(float brightness)
    {
        brightnessValue.SetText("{0:0}", brightness);
        PlayerPrefs.SetFloat("brightness", brightness);
    }
    
    // Toggle full screen
    public void SetFullscreen(bool isFullscreen)
    {
        PlayerPrefs.SetInt("fullscreen", (isFullscreen ? 1 : 0));
        Screen.fullScreen = isFullscreen;
    }

    // Apply quality change
    public void SetQuality(int qualityIndex)
    {
        PlayerPrefs.SetInt("quality", qualityIndex);
    }

    // Reset settings
    public void Reset()
    {
        brightnessSlider.value = defaultBrightness;
        SetBrightness(defaultBrightness);

        qualityDropdown.value = 1;
        QualitySettings.SetQualityLevel(1);
        SetQuality(1);

        fullscreenToggle.isOn = true;
        SetFullscreen(true);

        Resolution currentResolution = Screen.currentResolution;
        Screen.SetResolution(currentResolution.width, currentResolution.height, Screen.fullScreen);
        resolutionDropdown.value = resolutions.Length;
    }
}
