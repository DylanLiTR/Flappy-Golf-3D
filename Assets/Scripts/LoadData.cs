using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadData : MonoBehaviour
{
    [Header("General")]
    [SerializeField] bool canUse = false;
    [SerializeField] GameObject gameplayMenu;

    [Header("Volume")]
    [SerializeField] TextMeshProUGUI volumeValue;
    [SerializeField] Slider volumeSlider;

    [Header("Brightness")]
    [SerializeField] TextMeshProUGUI brightnessValue;
    [SerializeField] Slider brightnessSlider;

    [Header("Graphics")]
    [SerializeField] TMP_Dropdown qualityDropdown;
    [SerializeField] Toggle fullscreenToggle;

    [Header("Sensitivity")]
    [SerializeField] TextMeshProUGUI sensValue;
    [SerializeField] Slider sensSlider;

    [Header("Other")]
    [SerializeField] Toggle FPSToggle;

    void Awake()
    {
        // Enable testing with or without loading data
        if (canUse)
        {
            // Check if volume setting is saved
            if (PlayerPrefs.HasKey("volume"))
            {
                // Retrieves the value of volume from PlayerPrefs
                float localVolume = PlayerPrefs.GetFloat("volume");

                // Set the volume text, slider, and actual setting
                volumeValue.SetText("{0:0}", localVolume);
                volumeSlider.value = localVolume;
                AudioListener.volume = localVolume / 100f;
            }

            // Check if quality setting is saved
            if (PlayerPrefs.HasKey("quality"))
            {
                // Retrieves the index of quality from PlayerPrefs
                int localQuality = PlayerPrefs.GetInt("quality");

                // Set the quality dropdown and actual setting
                qualityDropdown.value = localQuality;
                QualitySettings.SetQualityLevel(localQuality);
            }

            // Check if fullscreen setting is saved
            if (PlayerPrefs.HasKey("fullscreen"))
            {
                // Retrieves the bool of fullscreen from PlayerPrefs
                int localFullscreen = PlayerPrefs.GetInt("fullscreen");

                // Set whether the game is fullscreen or not
                if (localFullscreen == 1)
                {
                    Screen.fullScreen = fullscreenToggle.isOn = true;
                }
                else
                {
                    Screen.fullScreen = fullscreenToggle.isOn = false;
                }
            }

            // Check if brightness setting is saved
            if (PlayerPrefs.HasKey("brightness"))
            {
                // Retrieves the value of brightness from PlayerPrefs
                float localBrightness = PlayerPrefs.GetFloat("brightness");

                // Set brightness text and slider
                brightnessValue.SetText("{0:1}", localBrightness);
                brightnessSlider.value = localBrightness;
            }

            // Check if sensitivity setting is saved
            if (PlayerPrefs.HasKey("sensitivity"))
            {
                // Retrieves the value of sensitivity from PlayerPrefs
                float localSens = PlayerPrefs.GetFloat("sensitivity");

                // Set sensitivity text and 
                sensSlider.value = localSens;
                gameplayMenu.GetComponent<GameplayController>().sensitivity = 180f * localSens;
            }

            // Check if frameCount setting is saved
            if (PlayerPrefs.HasKey("frameCount"))
            {
                // Retrieves the bool of frameCount from PlayerPrefs
                int localFrameCount = PlayerPrefs.GetInt("frameCount");

                // Set whether the frameCount is toggled on or not
                if (localFrameCount == 1)
                {
                    FPSToggle.isOn = true;
                }
                else
                {
                    FPSToggle.isOn = false;
                }
            }
        }
    }
}
