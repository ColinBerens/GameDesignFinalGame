using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TimerScript : MonoBehaviour
{
    [SerializeField] Trigger Trigger;
	[SerializeField] GoalScript GoalScript;
	[SerializeField] private GameObject _TimeText;
	[SerializeField] private GameObject _TimeText2;
	[SerializeField] private SoundManager _soundManager;
	private ThemePlayer _themePlayer;
	private ScoreKeeper _scoreKeeper;
    private float _time = 0f;
	bool _isTriggered = false;
	private void Start()
	{
        _soundManager = FindFirstObjectByType<SoundManager>();
        _scoreKeeper = FindFirstObjectByType<ScoreKeeper>();
        _themePlayer = FindFirstObjectByType<ThemePlayer>();
        _TimeText.SetActive(false);
		_TimeText2.SetActive(false);
	}
	void Update()
    {
		if (Trigger.IsTriggered && !GoalScript.IsTriggered)
		{
			if (!_TimeText.activeSelf)
				_TimeText.SetActive(true);
			_time += Time.deltaTime;
			int minutes = Mathf.FloorToInt(_time / 60);
			int seconds = Mathf.FloorToInt(_time % 60);
			_TimeText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:00}:{1:00}", minutes, seconds);
		}
		else if (GoalScript.IsTriggered && !_isTriggered)
		{
			_TimeText.SetActive(false);
			StartCoroutine(DelayLoad());
			_isTriggered = true;
		}
	}
	private IEnumerator DelayLoad()
	{
		_themePlayer.StopAll();
        _soundManager.PlayFull("Victory");
        yield return new WaitForSeconds(2f);
        _TimeText2.SetActive(true);
        int minutes = Mathf.FloorToInt(_time / 60);
		int seconds = Mathf.FloorToInt(_time % 60);
		_TimeText2.GetComponent<TextMeshProUGUI>().text = string.Format("{0:00}:{1:00}", minutes, seconds);
		yield return new WaitForSeconds(5f);
        _soundManager.StopAllSounds();
        _scoreKeeper.Score = string.Format("{0:00}:{1:00}", minutes, seconds);
        SceneManager.LoadScene("End");
		Debug.Log("Load next scene");
		yield return null;
	}
}
