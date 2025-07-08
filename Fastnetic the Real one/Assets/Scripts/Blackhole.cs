using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Blackhole : MonoBehaviour
{
	[SerializeField] private float _force;
	public float GrowSpeed;
	public bool IsTriggered;
	private SoundManager _soundManager;
    [SerializeField] private Camera _camera;
    [SerializeField] private Transform _distance;
    private ScoreKeeper _scoreKeeper;
    private void Start()
    {
        _scoreKeeper = FindFirstObjectByType<ScoreKeeper>();
        _soundManager = FindAnyObjectByType<SoundManager>();
    }
    void Update()
	{
		if (IsTriggered)
		{
			transform.localScale += new Vector3(1, 1, 0) * GrowSpeed * Time.deltaTime;
            transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y , 0.25f);
			_camera.orthographicSize += 0.75f * Time.deltaTime * GrowSpeed;

        }
	}
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (IsTriggered)
        {
            if (collision.tag == "Player" || collision.tag == "Moveable")
            {
                collision.gameObject.GetComponent<Rigidbody2D>().AddForce((transform.position - collision.transform.position).normalized * _force * Time.deltaTime);
                collision.GetComponent<Collider2D>().isTrigger = true;
                if (Vector2.Distance(collision.transform.position, transform.position) < Vector2.Distance(this.transform.position, _distance.position))
                {
                    if (collision.tag == "Player")
                    {
                        _scoreKeeper.Score = "GameOver";
                        _soundManager.StopAllSounds();
                        _soundManager.PlayFull("Death");
                        while (!_soundManager.GetSoundInfo("Death").IsPlayed)
                        {
                            System.Threading.Thread.Sleep(100);
                        }
                        SceneManager.LoadScene("Game 1", LoadSceneMode.Single); 
                    }
                    else
                        Destroy(collision.gameObject);
                }
            }
        }
    }
}
