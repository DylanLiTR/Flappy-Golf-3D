using UnityEngine;

public class HoleTrigger : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Player" && collider.gameObject.GetComponent<PhotonView>().viewID == gameManager.ball.GetComponent<PhotonView>().viewID)
        {
            gameManager.Scored();
            GetComponent<AudioSource>().Play();
        }
    }
}
