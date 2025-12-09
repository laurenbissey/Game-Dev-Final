using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class BallManager : MonoBehaviour
{
    public static BallManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private Rigidbody rb;

    [SerializeField] private MonoBehaviour shotController;

    [Header("Respawn / Checkpoints")]
    [Tooltip("If set, this is the initial spawn/checkpoint. Otherwise the ball's starting position is used.")]
    [SerializeField] private Transform initialSpawnPoint;

    private Vector3 currentCheckpoint;

    [Header("Events (for UI / effects)")]
    public UnityEvent onDeath;
    public UnityEvent onRespawn;
    public UnityEvent onLevelComplete;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (rb == null)
            rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        if (initialSpawnPoint != null)
            currentCheckpoint = initialSpawnPoint.position;
        else
            currentCheckpoint = transform.position;
    }

    /// Called by checkpoints to update where the ball will respawn
    public void SetCheckpoint(Vector3 position)
    {
        currentCheckpoint = position;
    }

    /// Called by hazards when the ball "dies"
    public void Kill()
    {
        onDeath?.Invoke();
        Respawn();
    }

    /// Move ball back to checkpoint and stop its movement
    public void Respawn()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = currentCheckpoint;

        onRespawn?.Invoke();
    }

    /// Called by the FinishZone when the level is completed
    public void LevelComplete()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;

        if (shotController != null)
            shotController.enabled = false;

        onLevelComplete?.Invoke();
    }

    public void EnableInput(bool enabled)
    {
        if (shotController != null)
            shotController.enabled = enabled;
    }
}
