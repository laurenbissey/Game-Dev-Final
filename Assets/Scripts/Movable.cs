using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movable : MonoBehaviour
{
    [Header("Components")]
    private Collider col;

    private enum Activity { movable, moving, locked };
    [Header("Status")]
    [SerializeField] private Activity activity = Activity.movable;
    private Vector3 offset = Vector3.zero;
    private Vector3 previousPos;

    [Header("Object")]
    [SerializeField] private float radius = 2.0f;

    void Start()
    {
        col = GetComponent<Collider>();

        previousPos = transform.position;
    }

    void Update()
    {
        Move();
    }

    private void Move()
    {
        // StartMoving() called before
        if (activity != Activity.moving) return;
        
        // If the mouse has been released, stop moving.
        if (Input.GetMouseButtonUp(0)) {
            // Prevent objects from being placed on each other on the Z-axis.
            if (previousPos.z != transform.position.z)
                transform.position = previousPos;
            else
                previousPos = transform.position;

            activity = Activity.movable; 
            return;
        }

        // Find the mouse position and keep the correct offset.
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        transform.position = worldPos - offset;

        PreventCollision();
    }

    private void PreventCollision()
    {
        const int iterationCount = 10;

        for (int i = 0; i < iterationCount; i++)
        {
            bool isSeparated = true;

            // Finds all overlapping colliders.
            Collider[] overlapping = Physics.OverlapSphere(transform.position, radius, ~0, QueryTriggerInteraction.Ignore);

            // For each overlapping collider, move the object to the closest non-collision.
            foreach (Collider overlap in overlapping)
            {
                if (overlap == col || overlap.isTrigger) continue;

                Vector3 directionToMove = Vector3.zero;
                float distanceToMove = 0f;

                // Calculates the distance to the outside of the collider.
                if (Physics.ComputePenetration(col, transform.position, transform.rotation,
                                               overlap, overlap.transform.position, overlap.transform.rotation,
                                               out directionToMove, out distanceToMove))
                {
                    transform.position += directionToMove * distanceToMove;
                    isSeparated = false;
                }
            }

            if (isSeparated) break;
        }
    }

    public bool StartMoving()
    {
        // Cannot move if locked.
        if (activity == Activity.locked) return false;

        activity = Activity.moving;

        // Calculate the offset from the center of the object, to keep correct positioning while moving.
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = -Camera.main.transform.position.z;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);

        offset = worldPos - transform.position;

        return true;
    }

    public void Lock()
    {
        activity = Activity.locked;
        this.enabled = false;
    }
}
