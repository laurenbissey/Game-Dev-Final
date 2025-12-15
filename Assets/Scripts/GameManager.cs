using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public enum GameState { build, play, complete };

    [Header("State")]
    public GameState state = GameState.build;

    [Header("Prefabs")]
    [SerializeField] private GameObject golfball;

    [Header("Manager")]
    [SerializeField] private MoveBlocks moveBlocks;
    private CameraFollow cameraFollow;
    private CameraPan cameraPan;

    [Header("Spawning")]
    [SerializeField] private Transform spawnPoint;

    [Header("HUD")]
    [SerializeField] private HUDManager hud;
    [SerializeField] private GameObject startButton;

    [Header("Level Settings")]
    [SerializeField] private string levelName = "Level 1";
    [SerializeField] private int par = 6;

    public int Strokes { get; private set; }
    public int Par => par;
    public string LevelName => levelName;
    private string LevelId => $"level_{SceneManager.GetActiveScene().buildIndex}";

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Gets the camera components. Could be changed to be a CameraManager.
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        cameraPan = Camera.main.GetComponent<CameraPan>();
    }

    private void Start()
    {
        // Ensure HUD exists (in case it's spawned dynamically)
        if (hud == null)
            hud = FindFirstObjectByType<HUDManager>();

        // Ensure RunScoreStore exists (in case level is loaded directly)
        if (RunScoreStore.Instance == null)
        {
            var go = new GameObject("RunScoreStore");
            go.AddComponent<RunScoreStore>();
        }

        BeginBuild();

        // Now safe:
        int idx = SceneManager.GetActiveScene().buildIndex;
        RunScoreStore.Instance.RegisterLevelMeta(idx, levelName, par);

        hud?.RefreshInGameHUD();
        hud?.HideCompleteOverlay();
    }

    private void BeginBuild()
    {
        cameraFollow.enabled = false;
        cameraPan.enabled = true;

        state = GameState.build;
    }

    // Begins the game, creating the Ball + BallManager.
    public void BeginPlay()
    {
        if (state != GameState.build) return;

        moveBlocks.LockBlocks();

        cameraPan.enabled = false;
        cameraFollow.enabled = true;
        startButton.gameObject.SetActive(false);

        // Reset scoring for the level
        Strokes = 0;
        hud?.RefreshInGameHUD();

        GameObject golfBall = Instantiate(golfball);
        golfBall.GetComponent<BallManager>().SetInitialSpawnPoint(spawnPoint);

        BallManager.Instance.onRespawn.AddListener(BallDeath);
        BallManager.Instance.onLevelComplete.AddListener(OnLevelComplete);

        cameraFollow.target = golfBall.transform;
        cameraFollow.ResetView();

        state = GameState.play;
    }

    public void RegisterStroke()
    {
        if (state != GameState.play) return;

        Strokes++;
        hud?.RefreshInGameHUD();
    }

    public void BeginComplete()
    {
        state = GameState.complete;
    }

    private void OnLevelComplete()
    {
        BeginComplete();
        int idx = SceneManager.GetActiveScene().buildIndex;
        RunScoreStore.Instance.TrySetBest(idx, Strokes);
        hud?.ShowCompleteOverlay();
    }

    public void BallDeath()
    {
        cameraFollow.ResetView();
    }
}
