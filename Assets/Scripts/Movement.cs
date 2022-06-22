using UnityEngine;
using TMPro;

public class Movement : MonoBehaviour
{
    [Header("General")]
	[SerializeField, Range(0f, 10f)] float flapHeight = 2f;
	[SerializeField, Range(0f, 10f)] float flapForce = 2f;
	[SerializeField] Transform playerInputSpace = default;
	[SerializeField, Range(0f, 10f)] float waterDrag = 1f;
	[SerializeField] TextMeshPro abovePlayer;

    [Header("Sound Effects")]
	[SerializeField] AudioSource audioSource;
	[SerializeField] AudioClip wings;
	[SerializeField] AudioClip sand;
	[SerializeField] AudioClip impact;

	public Animator flapAnim;

	Vector3 velocity;
	Vector3 horizontalVelocity;
	Vector3 verticalVelocity;
	Vector2 playerInput;

	public float maxSpeed = 10f;
	public bool disabled;
	public bool drowned;
	public int flapCount;

	bool desiredFlap;
	bool putting;
	int surfaceIndex;

	Rigidbody body;

	void Awake()
	{
		body = GetComponent<Rigidbody>();
		flapCount = 0;
	}

	void Update()
	{
		// Get user direction input
		playerInput.x = Input.GetAxis("Horizontal");
		playerInput.y = Input.GetAxis("Vertical");
		playerInput = Vector2.ClampMagnitude(playerInput, 1f);

		desiredFlap |= Input.GetButtonDown("Jump");
	}

	void FixedUpdate()
	{
		// Check if drowned to slow player
		if (drowned)
		{
			body.velocity *= 1f - waterDrag * Time.deltaTime;
		}

		if (disabled)
        {
			return;
        }

		// Check if flap was made
		if (desiredFlap)
		{
			// Get the current velocity
			velocity = body.velocity;
			horizontalVelocity = new Vector3(velocity.x, 0f, velocity.z);
			verticalVelocity = new Vector3(0f, velocity.y, 0f);

			desiredFlap = false;
			Flap();
			++flapCount;
			abovePlayer.SetText("{0:0}", flapCount);

			// Apply directional velocity
			body.velocity = horizontalVelocity + verticalVelocity;

			flapAnim.Play("Flap", -1, 0f);
			audioSource.PlayOneShot(wings, 0.5f);
		}
	}

	void Flap()
    {
		body.constraints = RigidbodyConstraints.None;
		body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;

		// Speed needed to jump specified flapHeight
		float flapSpeed = Mathf.Sqrt(-2f * Physics.gravity.y * flapHeight);

		// Reset upward velocity if already moving upward
		if (verticalVelocity.y > 0f)
		{
			flapSpeed = Mathf.Max(flapSpeed - verticalVelocity.y, 0f);
		}
		float magnitude = horizontalVelocity.magnitude;

		Vector3 flapDirection;

		// Check if camera affects direction
		if (playerInputSpace)
		{
			Vector3 forward = playerInputSpace.forward;
			forward.y = 0f;
			forward.Normalize();
			Vector3 right = playerInputSpace.right;
			right.y = 0f;
			right.Normalize();
			flapDirection = forward * playerInput.y + right * playerInput.x;
			horizontalVelocity += flapDirection * flapForce;
		}
		else
		{
			horizontalVelocity.x += playerInput.x * flapForce;
			horizontalVelocity.z += playerInput.y * flapForce;
			flapDirection = playerInput;
		}
		body.transform.rotation = Quaternion.LookRotation(flapDirection);

		// Make sure player cannot increase speed over maxSpeed by flapping
		if (magnitude > maxSpeed)
        {
			horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, magnitude);
		}
		else
        {
			horizontalVelocity = Vector3.ClampMagnitude(horizontalVelocity, maxSpeed);
		}

		// Set upward velocity
		if (putting)
		{
			return;
		}
		else if (verticalVelocity.y < 0)
		{
			verticalVelocity.y = flapSpeed;
		}
		else
		{
			verticalVelocity.y += flapSpeed;
		}
	}

	void OnCollisionEnter(Collision collision)
	{
		// Check if player hit terrain
		if (collision.gameObject.name == "Terrain")
        {
			// Get terrain index
			surfaceIndex = TerrainSurface.GetMainTexture(transform.position);

			surfaceDict();

			float volume = Mathf.Clamp(collision.relativeVelocity.magnitude / (maxSpeed * 2), 0.0f, 1.0f);

			if (volume > 0.3)
            {
				// If hit sand, play sand sound effect
				if (surfaceIndex == 1)
				{
					audioSource.PlayOneShot(sand, volume * 0.3f);
				}
				// Otherwise, play impact sound effect
				else
				{
					audioSource.PlayOneShot(impact, volume);
				}
			}
		}
	}

	void OnCollisionStay(Collision collision)
	{
		// Check if player hit terrain
		if (collision.gameObject.name == "Terrain")
		{
			int curSurface = TerrainSurface.GetMainTexture(transform.position);

			if (surfaceIndex != curSurface)
			{
				// Get terrain index
				surfaceIndex = curSurface;

				surfaceDict();
			}
		}
	}

	void surfaceDict()
	{
		putting = false;

		// If hit sand, ball gets stuck
		if (surfaceIndex == 1)
		{
			body.constraints = RigidbodyConstraints.FreezePosition;
		}
		// If hit green, activate putting mode flap
		else if (surfaceIndex == 2)
		{
			putting = true;
		}
	}
}