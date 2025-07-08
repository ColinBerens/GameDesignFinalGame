using System;
using System.Collections;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
	private SoundManager _soundManager;
    private void Start()
    {
        _soundManager = FindFirstObjectByType<SoundManager>();
    }
    private void OnTriggerEnter2D(Collider2D other)
	{
		if (other.gameObject.CompareTag("Breakable"))
		{
			_soundManager.PlayFull("Breaking");
            // Destroy the object if it has the "Breakable" tag
            Destroy(other.gameObject);
			Destroy(gameObject);
		}
		if (other.gameObject.CompareTag("Moveable"))
		{
			return;
		}
		Destroy(gameObject);
	}
}
