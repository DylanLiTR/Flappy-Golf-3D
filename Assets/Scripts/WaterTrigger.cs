using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] GameObject drownedMsg;
    [SerializeField] GameObject ball;

    void OnTriggerEnter()
    {
        gameManager.Death();
        ball.GetComponent<Movement>().drowned = true;
        drownedMsg.SetActive(true);

        GetComponent<AudioSource>().Play();
    }
}
