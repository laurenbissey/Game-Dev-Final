using UnityEngine.UI;
using UnityEngine;
using System.ComponentModel.Design.Serialization;

public class ReleaseToWorld : MonoBehaviour
{
    public Movable movable;
    private MoveBlocks moveBlocks;

    private Transform blockTransform;
    [SerializeField] private Image panelImage;
    [SerializeField] private Renderer rend;

    private bool released = false;

    void Start()
    {
        blockTransform = transform;

        if (movable != null)
        {
            movable.enabled = false;
        }

        moveBlocks = GameManager.Instance.GetComponent<MoveBlocks>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !released)
        {
            Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);

            if (hit.collider != null && hit.collider == GetComponent<Collider2D>())
            {
                ReleaseBlockToWorld();
                movable.StartMoving();

                released = true;
            }
        }

        if (panelImage != null) {
            if (moveBlocks != null && moveBlocks.movingBlock)
                panelImage.color = new Color(panelImage.color.r, panelImage.color.g, panelImage.color.b, .25f);
            else
                panelImage.color = new Color(panelImage.color.r, panelImage.color.g, panelImage.color.b, 1.0f);
        }

        if (released && movable.activity == Movable.Activity.movable)
        {
            rend.sortingOrder = -3;
        }
        else if (released && movable.activity == Movable.Activity.moving)
        {
            rend.sortingOrder = 10;
        }
    }

    public void ReleaseBlockToWorld()
    {
        Vector3 currentWorldPosition = blockTransform.position;
        Quaternion currentWorldRotation = blockTransform.rotation;

        blockTransform.SetParent(null);
        blockTransform.position = currentWorldPosition;
        blockTransform.rotation = currentWorldRotation;
        blockTransform.localScale = Vector3.one;

        if (movable != null)
        {
            movable.enabled = true;
        }
    }
}