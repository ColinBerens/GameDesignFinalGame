using UnityEngine;

public class CameraScript : MonoBehaviour
{
	[SerializeField] private Transform _playerTransform;
	[SerializeField] private Vector2 _offset;
	[SerializeField] private float _smoothSpeed = 5f;
	public bool InCutsceen = false;
	public Transform CutscenePosition;

	void Update()
	{
		if (!InCutsceen)
		{
			Vector3 targetPosition = new Vector3(
				_playerTransform.position.x + _offset.x,
				_playerTransform.position.y + _offset.y,
				transform.position.z // maintain original Z
			);

			transform.position = Vector3.Lerp(transform.position, targetPosition, _smoothSpeed * Time.deltaTime);
		}
		else if (CutscenePosition != null)
		{
			Vector3 targetPosition = new Vector3(
				CutscenePosition.position.x,
				CutscenePosition.position.y,
				transform.position.z // maintain original Z
			);
			transform.position = Vector3.Lerp(transform.position, targetPosition, _smoothSpeed * Time.deltaTime);
		}
	}
}
