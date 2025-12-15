using UnityEngine;

public class CameraPan : MonoBehaviour
{
    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float minZoom = 2.5f;
    [SerializeField] private float maxZoom = 7.5f;

    [Header("Max Screen")]
    [SerializeField] private Vector2 widthClamp;
    [SerializeField] private Vector2 heightClamp;
    [SerializeField] private bool clamped = false;

    private Camera cam;
    private Vector3 lastMousePos;

    void Start()
    {
        cam = GetComponent<Camera>();
    }

    void Update()
    {
        HandlePan();
        HandleZoom();

        if (clamped)
            ClampPosition();
    }

    void HandlePan()
    {
        // Middle mouse drag to pan.
        if (Input.GetMouseButtonDown(2))
        {
            lastMousePos = Input.mousePosition;
        }

        if (Input.GetMouseButton(2))
        {
            Vector3 delta = Input.mousePosition - lastMousePos;
            lastMousePos = Input.mousePosition;

            float worldDeltaX = -delta.x * cam.orthographicSize * 2f / Screen.height;
            float worldDeltaY = -delta.y * cam.orthographicSize * 2f / Screen.height;

            transform.Translate(worldDeltaX, worldDeltaY, 0f, Space.World);
        }
    }

    void HandleZoom()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll == 0f)
            return;

        float targetSize = cam.orthographicSize - scroll * zoomSpeed;

        targetSize = Mathf.Max(targetSize, minZoom);

        float maxAllowedZoom = GetMaxZoomAtPosition();
        targetSize = Mathf.Min(targetSize, maxAllowedZoom);

        cam.orthographicSize = targetSize;
    }

    float GetMaxZoomAtPosition()
    {
        Vector3 pos = transform.position;

        float distLeft = pos.x - widthClamp.x;
        float distRight = widthClamp.y - pos.x;
        float distBottom = pos.y - heightClamp.x;
        float distTop = heightClamp.y - pos.y;

        float maxVert = Mathf.Min(distTop, distBottom);
        float maxHorz = Mathf.Min(distLeft, distRight) / cam.aspect;

        return Mathf.Min(maxVert, maxHorz, maxZoom);
    }


    void ClampPosition()
    {
        float camHeight = cam.orthographicSize;
        float camWidth = camHeight * cam.aspect;

        float minX = widthClamp.x + camWidth;
        float maxX = widthClamp.y - camWidth;
        float minY = heightClamp.x + camHeight;
        float maxY = heightClamp.y - camHeight;

        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;
    }
}
