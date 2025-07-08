using UnityEngine;

public class ThemePlayer : MonoBehaviour
{
    [SerializeField] private SoundManager _soundManager;
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _blackhole;
    [SerializeField] private float _distance = 10f;
    [SerializeField] private Trigger _trigger;

    private bool _stop = false;

    private bool _isBlackholeSoundPlaying = false;

    private void Start()
    {
        _soundManager = FindFirstObjectByType<SoundManager>();
        _soundManager.Play("Theme1");
        _isBlackholeSoundPlaying = false;
    }

    void Update()
    {
        if (!_blackhole.activeSelf || _stop) return;

        var playerPos = _player.transform.position;
        var blackholeCollider = _blackhole.GetComponent<Collider2D>();
        var closestPoint = blackholeCollider.ClosestPoint((Vector2)playerPos);
        float dist = Vector3.Distance(playerPos, closestPoint);

        if (dist < _distance)
        {
            if (!_isBlackholeSoundPlaying)
            {
                _soundManager.Stop("Theme1");
                Debug.Log("Playing blackhole sound");
                _soundManager.Play("Blackhole");
                if (_trigger.IsTriggered)
                {
                    _soundManager.Stop("Timer");
                }
                _isBlackholeSoundPlaying = true;
            }
        }
        else
        {
            if (_isBlackholeSoundPlaying)
            {
                Debug.Log("Stopping blackhole sound");
                _soundManager.Stop("Blackhole");
                _soundManager.Play("Theme1");
                if (_trigger.IsTriggered)
                {
                    _soundManager.Play("Timer");
                }
                _isBlackholeSoundPlaying = false;
            }
        }
    }
    public void StopAll()
    {
        _soundManager.Stop("Theme1");
        _soundManager.Stop("Blackhole");
        _soundManager.Stop("Timer");
        _stop = true;
    }
}
