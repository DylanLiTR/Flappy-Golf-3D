using UnityEngine;

[RequireComponent(typeof(Camera))]
public class OrbitCamera : MonoBehaviour
{
	[SerializeField] Transform focus = default;
	[SerializeField, Range(1f, 20f)] float distance = 10f;
    [SerializeField, Min(0f)] float focusRadius = 2f;
	[SerializeField, Range(0f, 1f)] float focusCentering = 0.5f;
	[SerializeField, Range(-89f, 89f)] float minVerticalAngle = -30f, maxVerticalAngle = 60f;
	[SerializeField] LayerMask obstructionMask = -1;
	[SerializeField] Transform abovePlayer;
	[SerializeField] GameObject gameplayMenu;

	Vector3 focusPoint;
	Vector2 orbitAngles = new Vector2(20f, 0f);

	public bool frozen;
	public float sensitivity = 90f;

	Camera regularCamera;

	void Awake()
	{
		// Get camera and set position
		regularCamera = GetComponent<Camera>();
		focusPoint = focus.position;
		transform.localRotation = Quaternion.Euler(orbitAngles);

		// Hide cursor and lock position
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;

		// Gets the sensitivity saved from the gameplay controller
		sensitivity = gameplayMenu.GetComponent<GameplayController>().sensitivity;
	}

	void LateUpdate()
	{
		// Freeze the camera when paused
		if (frozen)
        {
			return;
        }

		UpdateFocusPoint();
		Quaternion lookRotation;

		// Limit camera angles
		if (CamControl())
        {
			ConstrainAngles();
			lookRotation = Quaternion.Euler(orbitAngles);
		} 
		else {
			lookRotation = transform.localRotation;
		}

		// Set camera position and rotation
		Vector3 lookDirection = lookRotation * Vector3.forward;
		Vector3 lookPosition = focusPoint - lookDirection * distance;

		// Managing camera collisions
		Vector3 rectOffset = lookDirection * regularCamera.nearClipPlane;
		Vector3 rectPosition = lookPosition + rectOffset;
		Vector3 castFrom = focus.position;
		Vector3 castLine = rectPosition - castFrom;
		float castDistance = castLine.magnitude;
		Vector3 castDirection = castLine / castDistance;

		// Zoom in if part of camera is blocked
		if (Physics.BoxCast(
			castFrom, CameraHalfExtends, castDirection, out RaycastHit hit,
			lookRotation, castDistance, obstructionMask,
			QueryTriggerInteraction.Ignore
		)) {
			rectPosition = castFrom + castDirection * hit.distance;
			lookPosition = rectPosition - rectOffset;
		}

		// Apply camera position and rotation
		transform.SetPositionAndRotation(lookPosition, lookRotation);

		abovePlayer.rotation = regularCamera.transform.rotation;
		abovePlayer.position = focus.position + Vector3.up;
	}

	void OnValidate()
	{
		// Ensure max and min vertical angle are valid
		if (maxVerticalAngle < minVerticalAngle)
		{
			maxVerticalAngle = minVerticalAngle;
		}
	}

	void UpdateFocusPoint()
	{
		// Allow movement in focus radius without moving camera
		Vector3 targetPoint = focus.position;
		if (focusRadius > 0f)
		{
			float distance = Vector3.Distance(targetPoint, focusPoint);
			float t = 1f;
			if (distance > 0.01f && focusCentering > 0f)
			{
				t = Mathf.Pow(1f - focusCentering, Time.unscaledDeltaTime);
			}
			if (distance > focusRadius)
			{
				t = Mathf.Min(t, focusRadius / distance);
			}
			focusPoint = Vector3.Lerp(targetPoint, focusPoint, t);
		}
		else
		{
			focusPoint = targetPoint;
		}
	}

	bool CamControl()
    {
		// Get direction of mouse movement
		Vector2 input = new Vector2(
			-Input.GetAxis("Mouse Y") * 2,
			Input.GetAxis("Mouse X") * 4
		);

		// Check if mouse was moved
		const float e = 0.001f;
		if (input.x < -e || input.x > e || input.y < -e || input.y > e)
		{
			orbitAngles += sensitivity * Time.unscaledDeltaTime * input;
			return true;
		}
		return false;
    }

	void ConstrainAngles()
	{
		// Limit camera angle to between min and max vertical angle
		orbitAngles.x = Mathf.Clamp(orbitAngles.x, minVerticalAngle, maxVerticalAngle);

		// Make sure angle is between 0 and 360 degrees
		if (orbitAngles.y < 0f)
		{
			orbitAngles.y += 360f;
		}
		else if (orbitAngles.y >= 360f)
		{
			orbitAngles.y -= 360f;
		}
	}

	Vector3 CameraHalfExtends
	{
		// Half of camera width and height for box cast
		get {
			Vector3 halfExtends;
			halfExtends.y = regularCamera.nearClipPlane * Mathf.Tan(0.5f * Mathf.Deg2Rad * regularCamera.fieldOfView);
			halfExtends.x = halfExtends.y * regularCamera.aspect;
			halfExtends.z = 0f;
			return halfExtends;
		}
	}
}
