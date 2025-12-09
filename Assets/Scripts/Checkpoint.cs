using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class Checkpoint : MonoBehaviour
{
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

    private void OnTriggerEnter(Collider other)
    {
        if (other.attachedRigidbody == null) return;

        BallManager ball = other.attachedRigidbody.GetComponent<BallManager>();
        if (ball == null) return;

        ball.SetCheckpoint(transform.position);
        ActivateVisual();
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
