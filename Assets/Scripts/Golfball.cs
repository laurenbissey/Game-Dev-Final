using UnityEngine;

public class Golfball : MonoBehaviour
{
    [Header("Components")]
    private Collider col;
    private Rigidbody rb;

    private enum Activity { idle, active, aiming };
    [Header("Status")]
    [Tooltip("Idle: Standstill, Active: Moving, Aiming: Standstill + MouseDown")]
    [SerializeField] private Activity activity = Activity.idle;

    [Header("Launch")]
    [SerializeField] private float launchMultiplier = 1f;
    private float stopVelocity = .01f;

    void Start()
    {
        col = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        CheckMousePosition();
        CheckVelocity();

        Debug.Log(rb.velocity);
    }

    private void OnCollisionStay(Collision collision)
    {
        // Checks that the object has collided with an object with GroundType
        GroundType groundType = collision.gameObject.GetComponent<GroundType>();

        if (groundType != null)
        {
            // GroundType's friction multiplier is used to slow down the ball.
            float frictionForce = groundType.rollingFriction;

            rb.velocity = Vector3.Lerp(rb.velocity, Vector3.zero, frictionForce * Time.deltaTime);
        }
    }

    private void CheckMousePosition()
    {
        switch (activity)
        {
            case Activity.idle:
                MouseOnBall();
                break;
            case Activity.aiming:
                MouseAimingBall();
                break;
        }
    }

    private void MouseOnBall()
    {
        // Only check if the mouse has been clicked.
        if (!Input.GetMouseButtonDown(0)) return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // Raycast towards the screen from the mouse, checking for the Golf Ball collider.
        if (col.Raycast(ray, out hit, Mathf.Infinity))
        {
            activity = Activity.aiming;
        }
    }

    private void MouseAimingBall()
    {
        // If the mouse has been let go, launch the ball.
        if (!Input.GetMouseButton(0))
        {
            LaunchBall();
            return;
        }

        // Show aiming.
    }

    private void LaunchBall()
    {
        // Get mouse position and find the direction away from the ball.
        Vector3 mousePos = Input.mousePosition;
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        worldPos.z = 0f;

        // Direction should be opposite of the mouse.
        Vector3 direction = worldPos - transform.position;
        Vector3 launchDirection = -direction;

        // Ensure the mouse is far enough off the ball to launch.
        if (launchDirection.magnitude < 1f)
        {
            activity = Activity.idle;
            return;
        }

        rb.AddForce(launchDirection * launchMultiplier, ForceMode.Impulse);

        activity = Activity.active;
    }

    private void CheckVelocity()
    {
        if (activity != Activity.active) return;

        // If the ball has stopped moving, changed mode.
        if (rb.velocity.magnitude <= stopVelocity)
        {
            activity = Activity.idle;
            rb.velocity = Vector3.zero;
        }
    }
}
