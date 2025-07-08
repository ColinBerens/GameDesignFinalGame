using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Trigger : MonoBehaviour
{
	[SerializeField] private GameObject _blackhole;
	[SerializeField] private GameObject _wall;
	[SerializeField] private float _delay = 1f;
	[SerializeField] private CameraScript _camera;
    [SerializeField] private Transform _cameraPos;
	[SerializeField] private GameObject[] _lights;
    [SerializeField] private SoundManager _soundManager;
    public bool IsTriggered = false;
    public bool IsBlackholeTriggered = false;
    private void Start()
	{
        _soundManager = FindAnyObjectByType<SoundManager>();
        _blackhole.SetActive(IsBlackholeTriggered);
        foreach (GameObject light in _lights)
        {
            light.SetActive(false);
        }
    }
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			StartCoroutine(DelayDestroy());
		}
    }

	private IEnumerator DelayDestroy()
	{
        _camera.InCutsceen = true;
        _camera.CutscenePosition = _cameraPos;
        yield return new WaitForSeconds(_delay/2);
        StartCoroutine(Lights());
        yield return new WaitForSeconds(_delay);
        StopCoroutine(Lights());
        _soundManager.Stop("Alarm");
        _blackhole.SetActive(true);
        _blackhole.transform.localScale = new Vector3(0.1f, 0.1f, 0);
		_blackhole.GetComponent<Blackhole>().IsTriggered = true;
		IsTriggered = true;
		yield return new WaitForSeconds(_delay);
        _soundManager.Play("DoorOpen");
        Destroy(_wall);
		_camera.InCutsceen = false;
        StopAllCoroutines();
    }

    private IEnumerator Lights()
    {
        _soundManager.Play("Alarm");
        while (true)
        {
            foreach (GameObject light in _lights)
            {
                light.SetActive(true);
                yield return new WaitForSeconds(0.25f);
                light.SetActive(false);
                yield return new WaitForSeconds(0.15f);
            }
        }
    }
}
