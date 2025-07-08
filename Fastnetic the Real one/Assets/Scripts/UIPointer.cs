using UnityEngine;
using UnityEngine.UI;

public class UIPointer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private RectTransform pointerArrow;
    [SerializeField] private GameObject targetObject;
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Canvas canvas;                     

    [Header("Settings")]
    [SerializeField] private float edgeMargin = 20f;
    [SerializeField] private bool hideWhenTargetVisible = true;
    [SerializeField] private Trigger trigger;

    private RectTransform canvasRect;
    private GameObject _player;
    private Vector3 _initialScale;
    private void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        if (canvas == null)
            canvas = GetComponentInParent<Canvas>();

        canvasRect = canvas.GetComponent<RectTransform>();

        pointerArrow.gameObject.SetActive(false);
        _player = GameObject.FindGameObjectWithTag("Player");
        _initialScale = pointerArrow.localScale;

    }

    private void Update()
    { 
        Debug.Log($"Target: {targetObject}");

        if (targetObject == null || mainCamera == null || !trigger.IsTriggered)
            return;


        Vector3 screenPos = mainCamera.WorldToScreenPoint(targetObject.transform.position);
        bool targetIsVisible = screenPos.z > 0 &&
                               screenPos.x > 0 && screenPos.x < Screen.width &&
                               screenPos.y > 0 && screenPos.y < Screen.height;

        // Handle visibility
        if (hideWhenTargetVisible)
        {
            pointerArrow.gameObject.SetActive(!targetIsVisible);
        }
        else
        {
            // Always ensure it's active if we're not hiding it when visible
            if (!pointerArrow.gameObject.activeSelf)
                pointerArrow.gameObject.SetActive(true);
        }


        Debug.Log($"Target is visible: {targetIsVisible}");
        if (!targetIsVisible)
        {
            float distance = Vector3.Distance(_player.transform.position, targetObject.GetComponent<Collider2D>().ClosestPoint(_player.transform.position));

            // Avoid divide-by-zero by clamping to a minimum distance
            distance = Mathf.Max(0.01f, distance);

            // Inverse scale: closer = bigger
            float scale = Mathf.Clamp(1f / distance, 0.5f, 3f); // clamp to prevent extreme sizes
            Vector3 targetScale = _initialScale * scale;
            pointerArrow.transform.localScale = Vector3.Lerp(pointerArrow.transform.localScale, targetScale, Time.deltaTime * 5f);


            // Just in case the pointer got disabled when visible and never reenabled
            if (!pointerArrow.gameObject.activeSelf)
                pointerArrow.gameObject.SetActive(true);

            if (screenPos.z < 0)
            {
                screenPos.x = Screen.width - screenPos.x;
                screenPos.y = Screen.height - screenPos.y;
                screenPos.z = 0;
            }

            Vector2 viewportPos = new Vector2(screenPos.x / Screen.width, screenPos.y / Screen.height);
            Vector2 canvasPos = new Vector2(
                ((viewportPos.x - 0.5f) * canvasRect.sizeDelta.x),
                ((viewportPos.y - 0.5f) * canvasRect.sizeDelta.y)
            );

            Vector2 cappedCanvasPos = ClampToCanvas(canvasPos);

            if (pointerArrow != null)
            {
                pointerArrow.anchoredPosition = cappedCanvasPos;

                float angle = Mathf.Atan2(canvasPos.y - cappedCanvasPos.y, canvasPos.x - cappedCanvasPos.x);
                pointerArrow.rotation = Quaternion.Euler(0, 0, angle * Mathf.Rad2Deg + 90);
            }
        }
    }

    private Vector2 ClampToCanvas(Vector2 position)
    {
        float halfWidth = canvasRect.sizeDelta.x * 0.5f - edgeMargin;
        float halfHeight = canvasRect.sizeDelta.y * 0.5f - edgeMargin;

        // Calculate the distance from the center
        float distX = Mathf.Abs(position.x);
        float distY = Mathf.Abs(position.y);

        // If inside the safe area, no need to clamp
        if (distX <= halfWidth && distY <= halfHeight)
            return position;

        // Find the point on the edge
        float screenRatio = halfWidth / halfHeight;
        float posRatio = distX / distY;

        if (posRatio > screenRatio)
        {
            // Clamp to left/right edge
            float sign = Mathf.Sign(position.x);
            return new Vector2(
                sign * halfWidth,
                position.y * (halfWidth / distX)
            );
        }
        else
        {
            // Clamp to top/bottom edge
            float sign = Mathf.Sign(position.y);
            return new Vector2(
                position.x * (halfHeight / distY),
                sign * halfHeight
            );
        }
    }
}