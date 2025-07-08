using UnityEngine;

public class ParalexEffect : MonoBehaviour
{
    [SerializeField] private float parallexEffectMulitplier;
    [SerializeField] private float _maxXAxis = 95f;
    Transform cameraTransform;
    Vector3 lastCameraPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cameraTransform = Camera.main.transform;
        lastCameraPosition = cameraTransform.position;
    }
    private void LateUpdate()
    {
        if (cameraTransform.position.x < _maxXAxis)
        {
            Vector3 deltaMovement = cameraTransform.position - lastCameraPosition;
            transform.position += new Vector3(deltaMovement.x, deltaMovement.y, 0) * parallexEffectMulitplier;
            lastCameraPosition = cameraTransform.position;
        }
    }
}
