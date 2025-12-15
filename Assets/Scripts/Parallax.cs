using UnityEngine;

public class Parallax : MonoBehaviour
{
    [Tooltip("0 = locked to world, 1 = locked to camera")]
    [Range(0f, 1f)]
    public float parallaxStrength = 0.02f; 

    private Transform cam;
    private Vector3 startPos;

    void Awake()
    {
        cam = Camera.main.transform;
        startPos = transform.position;
    }

    void LateUpdate()
    {
        Vector3 camDelta = cam.position;
        camDelta.z = 0f;

        transform.position = startPos + camDelta * parallaxStrength;
    }
}
