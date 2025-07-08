using UnityEngine;

public class UIScript : MonoBehaviour
{
    [SerializeField] private GameObject _blackhole;
	[SerializeField] private Transform _player;
	[SerializeField] private GameObject _UIBar;
	[SerializeField] private GameObject _blackholeIcon;
    [SerializeField] private Transform _blackholeIconPosition;
    [SerializeField] private GameObject[] _otherUiElements;
    private Collider2D _collider;

	private void Start()
	{
		_UIBar.SetActive(false);
		_collider = _blackhole.GetComponent<Collider2D>();
        _blackholeIcon.SetActive(false);
        foreach (GameObject uiElement in _otherUiElements)
        {
            uiElement.SetActive(false);
        }
    }
	// Update is called once per frame
	void Update()
    {
		if (_blackhole.activeSelf)
		{
			if(!_UIBar.activeSelf)
			{
                _UIBar.SetActive(true);
				_blackholeIcon.SetActive(true);
                foreach (GameObject uiElement in _otherUiElements)
                {
                    uiElement.SetActive(true);
                }
            }
            float distance = Vector3.Distance(_player.position, _collider.ClosestPoint(_player.position));
			float scaleX = 5f / distance; // Tune 5f higher for a fuller bar
			scaleX = Mathf.Clamp(scaleX, 0f, 1f); // Optional: prevent it from getting too big
			_UIBar.transform.localScale = new Vector3(scaleX, 1, 1);
            _blackholeIcon.transform.position = _blackholeIconPosition.position;
        }
	}
}
