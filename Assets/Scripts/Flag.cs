using UnityEngine;

public class Flag : MonoBehaviour
{
    // [SerializeField] GameObject ball;

    void OnCollisionEnter(Collision collision)
    {
		// if (collision.gameObject.name == "Ball")
		// {
        float volume = Mathf.Clamp(collision.relativeVelocity.magnitude / 10.0f /* ball.GetComponent<Movement>().maxSpeed */, 0.0f, 1.0f);
        AudioSource audioSource = GetComponent<AudioSource>();
        audioSource.PlayOneShot(audioSource.clip, volume);
        // }
    }
}
