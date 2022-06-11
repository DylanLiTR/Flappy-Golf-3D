using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class SoundController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI volumeValue;
    [SerializeField] Slider volumeSlider;
    [SerializeField] float defaultVolume = 100f;

    public void SetVolume(float volume)
    {
        AudioListener.volume = volume / 100f;
        volumeValue.SetText("{0:0}", volume);
        PlayerPrefs.SetFloat("volume", volume);
    }

    public void Reset()
    {
        AudioListener.volume = defaultVolume / 100f;
        volumeSlider.value = defaultVolume;
        volumeValue.SetText("{0:0}", defaultVolume);
    }
}
