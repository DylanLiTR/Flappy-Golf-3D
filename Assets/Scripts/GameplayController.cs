using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class GameplayController : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI sensValue;
    [SerializeField] Slider sensSlider;
    [SerializeField, Range(0f, 1f)] float defaultSens = 0.5f;
    [SerializeField] GameObject orbitCamera;

    public float sensitivity = 90f;

    // Set the sensitivity when slider is moved
    public void SetSens(float sens)
    {
        // Check if orbit camera exists to set sensitivity
        if (orbitCamera != null)
        {
            orbitCamera.GetComponent<OrbitCamera>().sensitivity = 180f * sens;
        }
        else
        {
            sensitivity = 180f * sens;
        }

        // Set sensitivity value and save to PlayerPrefs
        sensValue.SetText("{0:2}", sens);
        PlayerPrefs.SetFloat("sensitivity", sens);
    }

    // Resets the sensitivity
    public void Reset()
    {
        // Check if orbit camera exists to set sensitivity
        if (orbitCamera != null)
        {
            orbitCamera.GetComponent<OrbitCamera>().sensitivity = 180f * defaultSens;
        }
        else
        {
            sensitivity = 180f * defaultSens;
        }

        // Reset sensitivity value and save to PlayerPrefs
        sensSlider.value = defaultSens;
        sensValue.SetText("{0:2}", defaultSens);
    }
}
