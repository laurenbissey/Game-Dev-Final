using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class FinishZone : MonoBehaviour
{
    [Header("Events")]
    public UnityEvent onBallEnterFinish;

    private void Reset()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody == null) return;

        BallManager ball = other.attachedRigidbody.GetComponent<BallManager>();
        if (ball == null) return;

        ball.LevelComplete();
        onBallEnterFinish?.Invoke();
    }
}
