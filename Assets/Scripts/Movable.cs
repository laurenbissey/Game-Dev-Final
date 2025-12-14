using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Movable : MonoBehaviour
{
    [Header("Components")]
    private Collider2D col;
    private Rigidbody2D rb;

    private enum Activity { movable, moving, locked };
    [Header("Status")]
    [SerializeField] private Activity activity = Activity.movable;
    private Vector2 offset = Vector2.zero;
    private Vector2 previousPos = Vector2.zero;
    private bool isSeparated = true;

    void Start()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        rb.isKinematic = true;
        previousPos = rb.position;
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        if (activity != Activity.moving) return;

        if (Input.GetMouseButtonUp(0))
        {
            if (!isSeparated)
            {
                rb.position = previousPos;
            }
            else
            {
                previousPos = rb.position;
            }

            activity = Activity.movable;
            return;
        }

        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        rb.position = worldPos - offset;

        PreventCollision();

        if (isSeparated)
        {
            previousPos = rb.position;
        }
        else
        {
            rb.position = previousPos;
        }
    }

    private void PreventCollision()
    {
        const int iterationCount = 10;
        Collider2D[] overlapping = new Collider2D[16];
        ContactFilter2D filter = new ContactFilter2D().NoFilter();

        int contactCount = rb.OverlapCollider(filter, overlapping);

        if (contactCount == 0)
        {
            isSeparated = true;
            return;
        }

        for (int i = 0; i < iterationCount; i++)
        {
            isSeparated = true;

            contactCount = rb.OverlapCollider(filter, overlapping);
            if (contactCount == 0) break;

            for (int j = 0; j < contactCount; j++)
            {
                Collider2D overlap = overlapping[j];

                if (overlap.isTrigger || overlap == col) continue;

                if (overlap.CompareTag("Blocking"))
                {
                    isSeparated = false;
                    activity = Activity.movable;
                    return;
                }

                ColliderDistance2D colliderDistance = Physics2D.Distance(col, overlap);

                if (colliderDistance.isOverlapped)
                {
                    Vector2 separation = colliderDistance.normal * colliderDistance.distance;
                    rb.position += separation;
                    isSeparated = false;
                }
            }

            if (isSeparated) break;
        }
    }

    public bool StartMoving()
    {
        if (activity == Activity.locked) return false;

        activity = Activity.moving;
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        offset = worldPos - rb.position;
        previousPos = rb.position;

        return true;
    }

    public void Lock()
    {
        activity = Activity.locked;
        this.enabled = false;
    }
}