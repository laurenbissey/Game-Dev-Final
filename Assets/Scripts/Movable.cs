using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class Movable : MonoBehaviour
{
    private Collider2D col;
    private Rigidbody2D rb;

    public enum Activity { movable, moving, locked };
    [SerializeField] public Activity activity = Activity.movable;

    [SerializeField] private float gridSize = 1f;
    [SerializeField] private Vector2 gridOrigin = Vector2.zero;

    private Vector2 offset = Vector2.zero;
    private Vector2 previousPos = Vector2.zero;
    private Vector2 palettePos = Vector2.zero; 
    
    private RaycastHit2D[] hitResults = new RaycastHit2D[1];
    private bool hasBeenSnapped = false; 

    void Awake()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();

        rb.isKinematic = true;
        palettePos = rb.position;
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
            if (IsMovementValid(Vector2.zero))
            {
                activity = Activity.movable;
            }
            else
            {
                rb.position = palettePos;
                previousPos = palettePos;
                hasBeenSnapped = false; 
                activity = Activity.movable;
            }
            return;
        }

        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 desiredPos = worldPos - offset;
        Vector2 newPos = SnapToGrid(desiredPos);

        if (newPos != rb.position)
        {
            Vector2 movementVector = newPos - rb.position;
            
            if (IsMovementValid(movementVector))
            {
                rb.position = newPos;
                previousPos = newPos;
            }
        }
    }

    private Vector2 SnapToGrid(Vector2 pos)
    {
        return new Vector2(
            Mathf.Round((pos.x - gridOrigin.x) / gridSize) * gridSize + gridOrigin.x,
            Mathf.Round((pos.y - gridOrigin.y) / gridSize) * gridSize + gridOrigin.y
        );
    }

    private bool IsMovementValid(Vector2 movementVector)
    {
        float distance = movementVector.magnitude;
        Vector2 direction = movementVector.normalized;

        if (distance == 0f)
        {
            direction = Vector2.right; 
            distance = 0.001f;
        }

        ContactFilter2D filter = new ContactFilter2D().NoFilter();
        filter.useTriggers = false;

        int hitCount = rb.Cast(
            direction,
            filter,
            hitResults,
            distance
        );

        for (int i = 0; i < hitCount; i++)
        {
            RaycastHit2D hit = hitResults[i];
            
            if (hit.collider == col) continue; 
            
            if (hit.collider.CompareTag("Blocking") || !hit.collider.isTrigger)
            {
                return false;
            }
        }

        return true;
    }

    public bool StartMoving()
    {
        if (activity == Activity.locked) return false;

        if (!hasBeenSnapped)
        {
            rb.position = SnapToGrid(rb.position);
            hasBeenSnapped = true;
        }

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