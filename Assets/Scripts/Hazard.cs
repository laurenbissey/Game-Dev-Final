using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Hazard : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        TryKill(collision.collider);
    }

    private void OnTriggerEnter(Collider other)
    {
        TryKill(other);
    }

    private void TryKill(Collider col)
    {
        if (col.attachedRigidbody == null) return;

        BallManager ball = col.attachedRigidbody.GetComponent<BallManager>();
        if (ball == null) return;

        ball.Kill();
    }
}
