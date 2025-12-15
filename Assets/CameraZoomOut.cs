using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraZoomOut : MonoBehaviour
{
    private CameraFollow cameraFollow;
    private bool ballIdle;
    private float ballIdleTimer = 0;

    private float initialZoom;

    private Golfball cachedGolfball;

    [SerializeField] private float ballIdleMaxTime = 2f;
    [SerializeField] private float zoomScale = 1.5f;
    [SerializeField] private float zoomDuration = 5f;

    void Awake()
    {
        cameraFollow = GetComponent<CameraFollow>();
    }

    void Update()
    {
        if (cameraFollow.isActiveAndEnabled && cameraFollow.target != null) 
        { 
           cachedGolfball = cameraFollow.target.GetComponent<Golfball>();
        }


        if (cachedGolfball != null && cachedGolfball.activity == Golfball.BallActivity.idle)
        {
            if (!ballIdle)
                BallIdle();
        }
        else
        {
            if (ballIdle)
                BallActive();

            return;
        }

        if (ballIdle)
        {
            ballIdleTimer += Time.deltaTime;

            if (ballIdleTimer > ballIdleMaxTime)
            {
                Camera.main.orthographicSize = Mathf.Lerp(initialZoom, initialZoom * zoomScale, Mathf.Clamp01((ballIdleTimer - ballIdleMaxTime) / zoomDuration));
            }
        }
    }

    private void BallIdle()
    {
        ballIdle = true;
        ballIdleTimer = 0;

        initialZoom = Camera.main.orthographicSize;
    }

    private void BallActive() { 
        ballIdle = false;
        Camera.main.orthographicSize = initialZoom;
    }
}
