using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsOverlayUI : MonoBehaviour
{
    [SerializeField] private GameObject creditsOverlay;

    private void Start()
    {
        if (creditsOverlay != null)
            creditsOverlay.SetActive(false);
    }

    public void OpenCredits()
    {
        if (creditsOverlay != null)
            creditsOverlay.SetActive(true);
    }

    public void CloseCredits()
    {
        if (creditsOverlay != null)
            creditsOverlay.SetActive(false);
    }

    private void Update()
    {
        if (creditsOverlay != null && creditsOverlay.activeSelf && Input.GetKeyDown(KeyCode.Escape))
            CloseCredits();
    }
}
