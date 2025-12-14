using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : MonoBehaviour
{
    private BallManager cachedBallManager;
    private Golfball cachedBall;

    [Header("Visuals (optional)")]
    [SerializeField] private SpriteRenderer flagRenderer;
    [SerializeField] private Color inactiveColor = Color.white;
    [SerializeField] private Color activeColor = Color.green;

    private bool isActivated = false;

    private void Reset()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    private void Start()
    {
        if (flagRenderer != null)
        {
            flagRenderer.color = inactiveColor;
        }
    }

    // Caches Golfball and BallManager scripts to prevent calling GetComponent each frame.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.attachedRigidbody == null) return;

        cachedBallManager = other.attachedRigidbody.GetComponent<BallManager>();
        cachedBall = other.attachedRigidbody.GetComponent<Golfball>();
    }

    private void OnTriggerExit2D(Collider2D other)
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
    private void OnTriggerStay2D(Collider2D other)
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
            flagRenderer.color = activeColor;
        }
    }
}
