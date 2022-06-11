using UnityEngine;

public class OOB : MonoBehaviour
{
	[SerializeField] GameManager gameManager;
	[SerializeField] GameObject oobMsg;
	[SerializeField] float minx, maxx, minz, maxz;

	Rigidbody body;
	bool alreadyOOB;

	void Awake()
	{
		body = GetComponent<Rigidbody>();
	}

	void Update()
	{
		bool withinX = body.transform.position.x > minx && body.transform.position.x < maxx;
		bool withinZ = body.transform.position.z > minz && body.transform.position.z < maxz;

		if (!alreadyOOB && (!withinX || !withinZ))
        {
			gameManager.Death();
			oobMsg.SetActive(true);
			alreadyOOB = true;
		}
	}
}
