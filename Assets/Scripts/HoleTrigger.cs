using UnityEngine;

public class HoleTrigger : MonoBehaviour
{
    [SerializeField] GameManager gameManager;

    void OnTriggerEnter(Collider collider)
    {
        if (collider.name == "Ball")
        {
            gameManager.Scored();
            GetComponent<AudioSource>().Play();
        }
    }
}
