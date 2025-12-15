using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using static System.Net.Mime.MediaTypeNames;

public class EndSceneMenu : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private CreditsOverlayUI creditsUI;

    [Header("Build order assumption")]
    [SerializeField] private int firstLevelBuildIndex = 1; // 0 = MainMenu

    private void Start()
    {
        // Optional auto-find
        if (creditsUI == null)
            creditsUI = FindFirstObjectByType<CreditsOverlayUI>();
    }

    public void RestartRun()
    {
        if (RunScoreStore.Instance != null)
            RunScoreStore.Instance.ResetRun();

        SceneManager.LoadScene(firstLevelBuildIndex);
    }

    public void OpenCredits()
    {
        if (creditsUI != null) creditsUI.OpenCredits();
        else Debug.LogWarning("EndSceneMenu: creditsUI not assigned/found.");
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
