using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GoalScript : MonoBehaviour
{
	//[SerializeField] private GameObject _TimeText;
	//[SerializeField] private Trigger _trigger;
	//   private float _time = 0f;
	public bool IsTriggered = false;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.tag == "Player")
		{
			IsTriggered = true;
			//_TimeText.SetActive(true);

			//int minutes = Mathf.FloorToInt(_time / 60);
			//int seconds = Mathf.FloorToInt(_time % 60);
			//_TimeText.GetComponent<TextMeshProUGUI>().text = string.Format("{0:00}:{1:00}", minutes, seconds);
			//StartCoroutine(DelayLoad());
			//Destroy(gameObject);
		}
	}
}
