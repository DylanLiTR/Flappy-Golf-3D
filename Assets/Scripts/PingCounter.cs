using UnityEngine;
using TMPro;

public class PingCounter : MonoBehaviour
{
    public TextMeshProUGUI PingText;

    // Update is called once per frame
    void Update()
    {
        int ping = PhotonNetwork.GetPing();
        PingText.text = "Ping: " + ping;
        if (ping >= 300)
        {
            PingText.color = Color.red;
        } else if (ping >= 100)
        {
            PingText.color = Color.yellow;
        } else if (ping < 100)
        {
            PingText.color = Color.green;
        }
    }

    public void ShowPing(bool displayed)
    {
        gameObject.SetActive(displayed);
        PlayerPrefs.SetInt("pingCount", (displayed ? 1 : 0));
    }
}
