using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject drownedMsg;

    Color normalColor;
    Color underwaterColor;

    void Start()
    {
        normalColor = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        underwaterColor = new Color(0.22f, 0.65f, 0.77f, 0.5f);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "MainCamera")
        {
            RenderSettings.fogColor = underwaterColor;
            RenderSettings.fogDensity = 0.05f;
        }
        else if (other.gameObject.tag == "Player")
        {
            gameManager.Death();
            other.gameObject.GetComponent<Movement>().drowned = true;
            if (!PhotonNetwork.inRoom) drownedMsg.SetActive(true);

            GetComponent<AudioSource>().Play();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "MainCamera")
        {
            RenderSettings.fogColor = normalColor;
            RenderSettings.fogDensity = 0f;
        }
    }
}
