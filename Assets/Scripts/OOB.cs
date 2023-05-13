using UnityEngine;

public class OOB : MonoBehaviour
{
	[SerializeField] GameManager gameManager;
	[SerializeField] GameObject oobMsg;

	void OnTriggerExit(Collider other)
	{
		if (other.gameObject.tag == "Player")
		{
			gameManager.Death();
			if (!PhotonNetwork.inRoom) oobMsg.SetActive(true);
		}
	}
}
