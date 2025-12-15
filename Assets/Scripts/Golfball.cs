using UnityEngine;
using UnityEngine.SceneManagement;

public class Golfball : MonoBehaviour
{
    [Header("Components")]
    private Collider2D col;
    private Rigidbody2D rb;
    private BallAudio ballAudio;

    public enum BallActivity { idle, active, aiming };
    [Header("Status")]
    [Tooltip("Idle: Standstill, Active: Moving, Aiming: Standstill + MouseDown")]
    public BallActivity activity = BallActivity.idle;

    [Header("Launch")]
    [SerializeField] private Arrow arrow;
    [SerializeField] private float launchMultiplier = 2f;
    private float stopVelocity = .01f;

    [Tooltip("Gives time for the ball to accumulate velocity from idle.")]
    private float stopDelay = 0f;
    private float maxStopDelay = .1f;

    void Start()
    {
        col = GetComponent<Collider2D>();
        rb = GetComponent<Rigidbody2D>();
        ballAudio = GetComponent<BallAudio>();

        arrow.gameObject.SetActive(false);
    }

    void Update()
    {
        CheckMousePosition();
        CheckVelocity();
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        // Checks that the object has collided with an object with GroundType
        GroundType groundType = collision.gameObject.GetComponent<GroundType>();

        if (groundType != null)
        {
            // GroundType's friction multiplier is used to slow down the ball.
            float frictionForce = groundType.rollingFriction;

            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, frictionForce * Time.deltaTime);
        }
    }

    private void CheckMousePosition()
    {
        switch (activity)
        {
            case BallActivity.idle:
                MouseOnBall();
                break;
            case BallActivity.aiming:
                MouseAimingBall();
                break;
        }
    }

    private void MouseOnBall()
    {
        // Only check if the mouse has been clicked.
        if (!Input.GetMouseButtonDown(0)) return;

        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Check if the mouse is on the ball's collider.
        if (col.OverlapPoint(worldPoint))
        {
            activity = BallActivity.aiming;
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

        // Shows aiming trajectory with LineRenderer. 
        arrow.gameObject.SetActive(true);
        Vector2 launchDirection = CalculateLaunchVector();
        arrow.SetArrow(transform.position, launchDirection, launchDirection.magnitude * .5f);
    }

    private void LaunchBall()
    {
        Vector2 launchDirection = CalculateLaunchVector();
        arrow.gameObject.SetActive(false);

        // Ensure the mouse is far enough off the ball to launch.
        if (launchDirection.magnitude < .5f)
        {
            activity = BallActivity.idle;
            return;
        }

        rb.AddForce(launchDirection * launchMultiplier, ForceMode2D.Impulse);

        // Count stroke
        if(SceneManager.GetActiveScene().buildIndex != 0) GameManager.Instance.RegisterStroke();
        ballAudio.BallHit(launchDirection.magnitude);

        stopDelay = 0;
        activity = BallActivity.active;
    }

    private void CheckVelocity()
    {
        if (activity != BallActivity.active) return;

        stopDelay += Time.deltaTime;

        // If the ball has stopped moving, changed mode.
        if (rb.velocity.magnitude <= stopVelocity && stopDelay >= maxStopDelay)
        {
            activity = BallActivity.idle;
            rb.velocity = Vector2.zero;
        }
    }

    private Vector2 CalculateLaunchVector()
    {
        // Get mouse position and find the direction away from the ball.
        Vector2 worldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Direction should be opposite of the mouse.
        Vector2 direction = worldPos - (Vector2) transform.position;
        Vector2 launchDirection = -direction;

        return launchDirection;
    }
}
