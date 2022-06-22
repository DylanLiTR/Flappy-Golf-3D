using UnityEngine;
using TMPro;

public class FPSCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI frameCounter;
    float fps;

    void Start()
    {
        InvokeRepeating("GetFPS", 1, 1);
    }

    void GetFPS()
    {
        fps = (int)(1f / Time.unscaledDeltaTime);
        frameCounter.SetText("{0:0} FPS", fps);
    }

    public void ShowFPS(bool displayed)
    {
        gameObject.SetActive(displayed);
        PlayerPrefs.SetInt("frameCount", (displayed ? 1 : 0));
    }
}
