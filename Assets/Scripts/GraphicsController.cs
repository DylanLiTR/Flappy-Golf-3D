using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GraphicsController : MonoBehaviour
{
    [SerializeField] Slider brightnessSlider;
    [SerializeField] TextMeshProUGUI brightnessValue;
    [SerializeField] float defaultBrightness = 1f;

    [SerializeField] TMP_Dropdown qualityDropdown;
    [SerializeField] Toggle fullscreenToggle;

    public TMP_Dropdown resolutionDropdown;
    Resolution[] resolutions;

    void Start()
    {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();

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

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SetBrightness(float brightness)
    {
        brightnessValue.SetText("{0:1}", brightness);
        PlayerPrefs.SetFloat("brightness", brightness);
    }
    
    public void SetFullscreen(bool isFullscreen)
    {
        PlayerPrefs.SetInt("fullscreen", (isFullscreen ? 1 : 0));
        Screen.fullScreen = isFullscreen;
    }

    public void SetQuality(int qualityIndex)
    {
        PlayerPrefs.SetInt("quality", qualityIndex);
    }

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
