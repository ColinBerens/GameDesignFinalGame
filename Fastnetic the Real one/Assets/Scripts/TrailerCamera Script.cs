using UnityEngine;

public class TrailerCameraScript : MonoBehaviour
{
	public float moveSpeed = 5f; // Units per second
	private InputScript inputScript;

	void Awake()
	{
		inputScript = FindAnyObjectByType<InputScript>();
		if (inputScript == null)
			Debug.LogError("InputScript not found in the scene!");
	}

	void Update()
	{
		if (inputScript == null)
			return;

		// Get the move input from your custom input script (should be a Vector2)
		Vector2 moveInput = inputScript.MoveInput;

		// Move the camera
		Vector3 move = new Vector3(moveInput.x, moveInput.y, 0f);
		transform.position += move * moveSpeed * Time.deltaTime;
	}
}
