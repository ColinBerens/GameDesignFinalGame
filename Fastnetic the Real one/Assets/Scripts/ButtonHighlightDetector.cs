using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonHighlightDetector : MonoBehaviour, ISelectHandler
{
    SoundManager _soundManager;
    RumbleManager _rumbleManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _soundManager = FindFirstObjectByType<SoundManager>();
        _rumbleManager = FindFirstObjectByType<RumbleManager>();
    }

    public void OnSelect(BaseEventData eventData)
    {
        _rumbleManager.RumblePulse(0.25f, 0.75f,0.1f);
        Debug.Log("Button selected!");
        _soundManager.PlayFull("ButtonSelect");
    }
}
