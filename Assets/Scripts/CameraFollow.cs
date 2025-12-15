using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("Target")]
    public Transform target;

    [Header("Dead Zone (world units)")]
    [Tooltip("Width and height of the region around the camera where the ball can move without moving the camera.")]
    public Vector2 deadZoneSize = new Vector2(6f, 4f);

    [Header("Movement")]
    public float followSpeed = 5f;
    public bool followX = true;
    public bool followY = true;

    [Header("Progression Locks (optional)")]
    [Tooltip("If true, camera will not move down even if ball goes below the dead zone.")]
    public bool onlyMoveUp = false;

    [Tooltip("If true, camera will not move left (useful if the course only progresses to the right).")]
    public bool onlyMoveRight = false;

    private Camera cam;
    private float initialZoom;
    private float initialZ;
    private Vector3 velocity;

    void Awake()
    {
        cam = GetComponent<Camera>();
        initialZoom = cam.orthographicSize;
        initialZ = transform.position.z;
    }

    private void OnEnable()
    {
        cam.orthographicSize = initialZoom;

        ResetView();
    }

    public void ResetView()
    {
        if (target != null)
            transform.position = new Vector3(
                target.position.x,
                target.position.y,
                initialZ
            );
    }

    void LateUpdate()
    {
        if (target == null) return;

        Vector3 camPos = transform.position;
        Vector3 targetPos = target.position;

        float halfDeadWidth = deadZoneSize.x * 0.5f;
        float halfDeadHeight = deadZoneSize.y * 0.5f;

        Vector3 move = Vector3.zero;

        if (followX)
        {
            float dx = targetPos.x - camPos.x;

            if (dx > halfDeadWidth)
            {
                float newX = dx - halfDeadWidth;
                if (!onlyMoveRight || newX > 0)
                    move.x = newX;
            }
            else if (dx < -halfDeadWidth)
            {
                float newX = dx + halfDeadWidth;
                if (!onlyMoveRight || newX > 0)
                    move.x = newX;
            }
        }

        if (followY)
        {
            float dy = targetPos.y - camPos.y;

            if (dy > halfDeadHeight)
            {
                move.y = dy - halfDeadHeight;
            }
            else if (dy < -halfDeadHeight)
            {
                float newY = dy + halfDeadHeight;
                if (!onlyMoveUp || newY > 0)
                    move.y = newY;
            }
        }

        Vector3 desiredPos = camPos + move;
        desiredPos.z = initialZ;

        transform.position = Vector3.SmoothDamp(
            transform.position,
            desiredPos,
            ref velocity,
            1f / followSpeed
        );

    }

}