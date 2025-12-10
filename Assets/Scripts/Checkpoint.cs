using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{
    private BallManager cachedBallManager;
    private Golfball cachedBall;

    [Header("Visuals (optional)")]
    [SerializeField] private Renderer flagRenderer;
    [SerializeField] private Color inactiveColor = Color.white;
    [SerializeField] private Color activeColor = Color.green;

    private bool isActivated = false;

    private void Reset()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void Start()
    {
        if (flagRenderer != null)
        {
            var mat = flagRenderer.material;
            mat.color = inactiveColor;
        }
    }

    // Caches Golfball and BallManager scripts to prevent calling GetComponent each frame.
    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody == null) return;

        cachedBallManager = other.attachedRigidbody.GetComponent<BallManager>();
        cachedBall = other.attachedRigidbody.GetComponent<Golfball>();
    }

    private void OnTriggerExit(Collider other)
    {
        if (cachedBall == null || cachedBallManager == null) return;

        if (other.attachedRigidbody.GetComponent<BallManager>() == cachedBallManager)
        {
            cachedBallManager = null;
            cachedBall = null;
        }
    }

    // Checks for the ball to become idle, preventing it from falling off
    // and gaining a checkpoint.
    private void OnTriggerStay(Collider other)
    {
        if (cachedBall == null || cachedBallManager == null) return;

        if (cachedBall.activity == Golfball.BallActivity.idle)
        {
            cachedBallManager.SetCheckpoint(transform.position);
            ActivateVisual();
        }
    }

    private void ActivateVisual()
    {
        if (isActivated) return;
        isActivated = true;

        if (flagRenderer != null)
        {
            var mat = flagRenderer.material;
            mat.color = activeColor;
        }
    }
}
