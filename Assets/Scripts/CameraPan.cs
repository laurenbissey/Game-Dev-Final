using UnityEngine;

public class CameraPan : MonoBehaviour
{
    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float minZoom = 5f;
    [SerializeField] private float maxZoom = 50f;

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
        // Scrollwheel to zoom.
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (scroll != 0f)
        {
            float zoomAmount = -scroll * zoomSpeed;
            float newSize = cam.orthographicSize + zoomAmount;

            newSize = Mathf.Clamp(newSize, minZoom, maxZoom);

            cam.orthographicSize = newSize;
        }
    }
}
