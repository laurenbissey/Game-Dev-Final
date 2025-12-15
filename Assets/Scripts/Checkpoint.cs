using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Checkpoint : MonoBehaviour
{
    private BallManager cachedBallManager;
    private Golfball cachedBall;

    [SerializeField] private Transform respawnPos;

    [Header("SFX")]
    [SerializeField] private AudioClip checkpointSound;

    [Header("Visuals (optional)")]
    [SerializeField] private SpriteMaskTransition transition;

    private bool isActivated = false;

    private void Reset()
    {
        Collider2D col = GetComponent<Collider2D>();
        col.isTrigger = true;
    }

    // Caches Golfball and BallManager scripts to prevent calling GetComponent each frame.
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (isActivated) return;
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

        if (cachedBall.activity == Golfball.BallActivity.idle && !isActivated)
        {
            cachedBallManager.SetCheckpoint(respawnPos.position);

            AudioManager.instance.PlaySFX(checkpointSound, .75f);
            ActivateVisual();
        }
    }

    private void ActivateVisual()
    {
        if (isActivated) return;
        isActivated = true;

        if (transition != null)
        {
            transition.Reveal();
        }
    }
}
