using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    private LineRenderer lineRenderer;
    [SerializeField] private GameObject arrowHead;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    public void SetArrow(Vector3 start, Vector3 direction, float distance)
    {
        List<Vector3> points = new List<Vector3> { start, start + direction.normalized * distance };

        lineRenderer.SetPositions(points.ToArray());
        lineRenderer.positionCount = 2;

        arrowHead.transform.position = points[1];

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg + 270f;
        arrowHead.transform.rotation = Quaternion.Euler(0f, 0f, angle);
    }
}
