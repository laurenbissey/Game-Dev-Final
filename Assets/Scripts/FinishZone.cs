using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider2D))]
public class FinishZone : MonoBehaviour
{
    private BallManager cachedBallManager;
    private Golfball cachedBall;

    private bool levelCompleted = false;

    [Header("Particles")]
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private float particleLength = 3f;

    [Header("SFX")]
    [SerializeField] private AudioClip victorySound;

    [Header("Events")]
    public UnityEvent onBallEnterFinish;

    private void Awake()
    {
        particles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

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

        if (cachedBall.activity == Golfball.BallActivity.idle && !levelCompleted)
        {
            levelCompleted = true;

            cachedBallManager.LevelComplete();
            onBallEnterFinish?.Invoke();

            AudioManager.instance.PlaySFX(victorySound, .25f);
            StartCoroutine(StartParticles());
        }
    }

    IEnumerator StartParticles()
    {
        particles.Clear();
        particles.Play();

        yield return new WaitForSeconds(particleLength);

        particles.Stop(true, ParticleSystemStopBehavior.StopEmitting);
    }
}
