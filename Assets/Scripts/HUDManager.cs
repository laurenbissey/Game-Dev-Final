using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class HUDManager : MonoBehaviour
{
    [Header("In-game HUD (always visible)")]
    [SerializeField] private TMP_Text levelText;
    [SerializeField] private TMP_Text strokesText;
    [SerializeField] private TMP_Text parText;

    [Header("Complete Overlay")]
    [SerializeField] private GameObject completeOverlay;
    [SerializeField] private TMP_Text completeStrokesText;
    [SerializeField] private TMP_Text completeParText;
    [SerializeField] private TMP_Text completeResultText;

    [SerializeField] private int lastLevelBuildIndex = 3; // set in inspector
    [SerializeField] private int endGameBuildIndex = 4;   // set in inspector

    private void Start()
    {
        // In-game HUD should always show
        RefreshInGameHUD();

        // Overlay hidden at start
        if (completeOverlay != null)
            completeOverlay.SetActive(false);
    }

    public void RefreshInGameHUD()
    {
        if (GameManager.Instance == null) return;

        levelText.text = GameManager.Instance.LevelName;
        strokesText.text = $"Strokes: {GameManager.Instance.Strokes}";
        parText.text = $"Par: {GameManager.Instance.Par}";
    }

    public void ShowCompleteOverlay()
    {
        if (GameManager.Instance == null) return;

        int strokes = GameManager.Instance.Strokes;
        int par = GameManager.Instance.Par;

        // Keep the top HUD updated too
        RefreshInGameHUD();

        if (completeOverlay != null)
            completeOverlay.SetActive(true);

        if (completeStrokesText != null) completeStrokesText.text = $"Strokes:\n{strokes}";
        if (completeParText != null) completeParText.text = $"Par:\n{par}";

        int diff = strokes - par;
        string result = diff == 0 ? "Even par"
                     : diff < 0 ? "Under par"
                     : "Over par";

        if (completeResultText != null) completeResultText.text = result;
    }

    public void HideCompleteOverlay()
    {
        if (completeOverlay != null)
            completeOverlay.SetActive(false);
    }

    // Button hooks
    public void OnRetryPressed()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    public void OnNextLevelPressed()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int sceneCount = SceneManager.sceneCountInBuildSettings;

        int lastPlayableIndex = sceneCount - 2;

        if (currentIndex < lastPlayableIndex)
        {
            SceneManager.LoadScene(currentIndex + 1);
        }
        else
        {
            SceneManager.LoadScene(sceneCount - 1);
        }
    }
}
