using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveBlocks : MonoBehaviour
{
    private bool movingBlock = false;

    void Update()
    {
        CheckMousePosition();
    }

    private void CheckMousePosition()
    {
        // If clicked, check for an object with the "Movable" component.
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, Mathf.Infinity))
            {
                Movable movable = hit.collider.GetComponent<Movable>();

                // Begin moving object.
                if (movable != null)
                    movingBlock = movable.StartMoving();
            }
        }
        // If click released, stop moving objects.
        else if (Input.GetMouseButtonUp(0)) {
            movingBlock = false;
        }
    }

    public void LockBlocks()
    {
        // Finds all Movable objects and locks them (preventing movement).
        GameObject[] movables = GameObject.FindGameObjectsWithTag("Movable");

        foreach (GameObject movable in movables)
        {
            movable.GetComponent<Movable>().Lock();
        }

        movingBlock = false;
    }
}
