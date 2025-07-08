using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class RumbleManager : MonoBehaviour
{
    public static RumbleManager Instance { get; private set; }

    private Gamepad _pad;

	private Coroutine _stopRumbleCoroutine;
	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
        if (Instance == null)
		{
			Instance = this;
		}
	}
	public void RumblePulse(float lowFrequency, float highFrequency, float duration)
	{
		_pad = Gamepad.current;

		if (_pad != null)
		{
			_pad.SetMotorSpeeds(lowFrequency, highFrequency);
			_stopRumbleCoroutine = StartCoroutine(StopRumble(duration, _pad));

		}
	}
	public void StartRumble(float lowFrequency, float highFrequency)
	{
		_pad = Gamepad.current;

		if (_pad != null)
		{
			_pad.SetMotorSpeeds(lowFrequency, highFrequency);
		}
	}
	public void StopRumble()
	{
		if (_pad != null)
		{
			_pad.SetMotorSpeeds(0, 0);
		}
	}
	private IEnumerator StopRumble(float duration, Gamepad pad)
	{
		yield return new WaitForSeconds(duration);
		if (_pad != null)
		{
			_pad.SetMotorSpeeds(0, 0);
		}
	}
}
