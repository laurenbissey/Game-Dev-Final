using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HowToOverlay : MonoBehaviour
{
    [SerializeField] private GameObject overlayRoot;

    private void Start()
    {
        if (overlayRoot != null)
            overlayRoot.SetActive(true);

        Time.timeScale = 0f;
    }

    public void CloseOverlay()
    {
        if (overlayRoot != null)
            overlayRoot.SetActive(false);

        Time.timeScale = 1f;
    }
}
