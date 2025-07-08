using UnityEngine;

public class ScoreKeeper : MonoBehaviour
{
    public string Score;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Score = "GameOver";
    }
}
