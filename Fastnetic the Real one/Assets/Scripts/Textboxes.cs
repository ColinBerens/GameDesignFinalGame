using System.Collections;
using TMPro;
using UnityEngine;

public class Textboxes : MonoBehaviour
{
    public float amplitude = 1f;      // Height of the wave
    public float frequency = 1f;      // Speed of the wave
    private Vector3 startPos;
    public string text;
    public TextMeshPro uiText;                 // Assign your UI Text component
    public float letterDelay = 0.05f;   // Delay between letters
    private Coroutine typingCoroutine;
    bool isExecuted = false;
    void Start()
    {
        startPos = transform.position;
        text = uiText.text;
        uiText.text = "";
    }

    void Update()
    {
        float yOffset = Mathf.Sin(Time.time * frequency) * amplitude;
        transform.position = startPos + new Vector3(0f, yOffset, 0f);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            if (!isExecuted)
                typingCoroutine = StartCoroutine(TypeText(text));
        }
    }
    private IEnumerator TypeText(string message)
    {
        isExecuted = true;
        uiText.text = "";

        foreach (char letter in message)
        {
            uiText.text += letter;
            yield return new WaitForSeconds(letterDelay);
        }
    }
}
