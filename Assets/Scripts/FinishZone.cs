using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class FinishZone : MonoBehaviour
{
    private BallManager cachedBallManager;
    private Golfball cachedBall;

    [Header("Events")]
    public UnityEvent onBallEnterFinish;

    private void Reset()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
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
    // and completing the level.
    private void OnTriggerStay2D(Collider2D other)
    {
        if (cachedBall == null || cachedBallManager == null) return;

        if (cachedBall.activity == Golfball.BallActivity.idle)
        {
            cachedBallManager.LevelComplete();
            onBallEnterFinish?.Invoke();
        }
    }
}
