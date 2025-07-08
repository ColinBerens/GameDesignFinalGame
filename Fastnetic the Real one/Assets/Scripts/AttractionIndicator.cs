using UnityEngine;

public class AttractionIndicator : MonoBehaviour
{
    public float Speed;
    Vector3 _startScale;
    private void Start()
    {
        _startScale = transform.localScale;
    }
    // Update is called once per frame
    void Update()
    {
        transform.localScale -= new Vector3(0.1f, 0.1f, 0.1f) * Speed * Time.deltaTime;
        if (transform.localScale.x <= 0.5f)
        {
            transform.localScale = _startScale;
        }
    }
}
