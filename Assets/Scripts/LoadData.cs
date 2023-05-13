using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LoadData : MonoBehaviour
{
    [Header("General")]
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

    [Header("Customization")]
    [SerializeField] GameObject customizationMenu;
    [SerializeField] TMP_Dropdown ballDropdown;
    [SerializeField] TMP_Dropdown wingsDropdown;
    [SerializeField] TMP_Dropdown trailDropdown;

    [Header("Other")]
    [SerializeField] Toggle FPSToggle;
    [SerializeField] GameObject frameCounter;
    [SerializeField] Toggle PingToggle;
    [SerializeField] GameObject pingCounter;

    void Awake()
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
            brightnessValue.SetText("{0:0}", localBrightness);
            brightnessSlider.value = localBrightness;
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
                frameCounter.SetActive(true);
            }
            else
            {
                FPSToggle.isOn = false;
            }
        }

        // Check if pingCount setting is saved
        if (PlayerPrefs.HasKey("pingCount"))
        {
            // Retrieves the bool of pingCount from PlayerPrefs
            int localPingCount = PlayerPrefs.GetInt("pingCount");

            // Set whether the pingCount is toggled on or not
            if (localPingCount == 1)
            {
                PingToggle.isOn = true;
                pingCounter.SetActive(true);
            }
            else
            {
                PingToggle.isOn = false;
            }
        }
    }

    public void applyCustomization()
    {
        CustomizationMenu custom = customizationMenu.GetComponent<CustomizationMenu>();
        // Check if ball skin is saved
        if (PlayerPrefs.HasKey("ball"))
        {
            int skin = PlayerPrefs.GetInt("ball");
            custom.ChangeBall(skin);
            ballDropdown.value = skin;
        }

        // Check if wings skin is saved
        if (PlayerPrefs.HasKey("wings"))
        {
            int skin = PlayerPrefs.GetInt("wings");
            custom.ChangeWings(skin);
            wingsDropdown.value = skin;
        }

        // Check if trail is saved
        if (PlayerPrefs.HasKey("trail"))
        {
            int skin = PlayerPrefs.GetInt("trail");
            custom.ChangeTrail(skin);
            trailDropdown.value = skin;
        }

        // Check if sensitivity setting is saved
        if (PlayerPrefs.HasKey("sensitivity"))
        {
            // Retrieves the value of sensitivity from PlayerPrefs
            float localSens = PlayerPrefs.GetFloat("sensitivity");

            // Set sensitivity text and 
            sensSlider.value = localSens;
            gameplayMenu.GetComponent<GameplayController>().SetSens(localSens);
        }
    }
}
