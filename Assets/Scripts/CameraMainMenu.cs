using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMainMenu : MonoBehaviour
{
    [Header("Walls")]
    [SerializeField] private GameObject leftWall;
    [SerializeField] private GameObject rightWall;
    [SerializeField] private GameObject topWall;
    [SerializeField] private GameObject bottomWall;
    [SerializeField] private float wallWidth;

    [Header("Ball")]
    [SerializeField] private GameObject golfball;

    void Start()
    {
        Camera cam = Camera.main;
        float screenHeight = 2f * cam.orthographicSize;
        float screenWidth = screenHeight * cam.aspect;

        leftWall.transform.position = new Vector2(-screenWidth / 2f + -wallWidth / 2f, 0f);
        rightWall.transform.position = new Vector2(screenWidth / 2f + wallWidth / 2f, 0f);
        bottomWall.transform.position = new Vector2(0f, -screenHeight / 2f + -wallWidth / 2f);
        topWall.transform.position = new Vector2(0f, screenHeight / 2f + wallWidth / 2f);

        golfball.transform.position = new Vector2(screenWidth / 3f, screenHeight / 3f);
    }
}
