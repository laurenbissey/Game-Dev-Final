using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class Hazard : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        TryKill(collision.collider);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        TryKill(other);
    }

    private void TryKill(Collider2D col)
    {
        if (col.attachedRigidbody == null) return;

        BallManager ball = col.attachedRigidbody.GetComponent<BallManager>();
        if (ball == null) return;

        ball.Kill();
    }
}
