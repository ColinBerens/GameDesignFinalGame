using UnityEngine;

public class MagneticObjectsScript : MonoBehaviour
{
    public Collider2D Collider;
    public Rigidbody2D Rb;
    public bool IsAttractor;
    public bool GotRepellet;
    private SoundManager _soundManager;
    private GameObject _player;
    public Material Material;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Material = GetComponent<Renderer>().material;
        _player = GameObject.FindGameObjectWithTag("Player");
        _soundManager = FindFirstObjectByType<SoundManager>();
		Collider = GetComponent<Collider2D>();
        Rb = GetComponent<Rigidbody2D>();
        _soundManager = FindFirstObjectByType<SoundManager>();
    }
    private void OnCollisionEnter2D(Collision2D collision)
	{
        if(Vector3.Distance(_player.transform.position, transform.position) < 10f)
		    _soundManager.PlayFull("Metal");
	}
}
