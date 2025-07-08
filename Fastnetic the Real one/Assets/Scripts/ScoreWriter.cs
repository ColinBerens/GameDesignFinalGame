using TMPro;
using UnityEngine;

public class ScoreWriter : MonoBehaviour
{
    private ScoreKeeper scoreKeeper;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        scoreKeeper = FindFirstObjectByType<ScoreKeeper>();
    }

    // Update is called once per frame
    void Update()
    {
        TextMeshProUGUI text = GetComponent<TextMeshProUGUI>();
        if (text != null && scoreKeeper != null)
        {
            text.text = scoreKeeper.Score;
        }
        else
        {
            Debug.LogWarning("TextMeshProUGUI component or ScoreKeeper not found.");
        }
    }
}
