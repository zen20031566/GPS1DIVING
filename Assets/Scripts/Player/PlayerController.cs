using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D Rb { get; private set; }
    private bool isFacingRight;

    //Collision checks
    [SerializeField] private float groundDetectionRayLength = 0.02f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private BoxCollider2D col;
    private RaycastHit2D groundHit;
    private bool isGrounded = false;

    //Movement variables
    private float defaultGravity = 9.8f;
    private Vector2 moveVelocity;
    [SerializeField] private float moveSpeed;
    [SerializeField] private float climbSpeed;

    //Swim variables
    [SerializeField] private float maxSwimSpeed = 8;
    [SerializeField] private float swimAcceleration = 0.1f; //How fast to reach max speed
    [SerializeField] private float maxSwimAccelerationForce = 20f; //Limits maximum force that can be applied when accelerating
    [SerializeField] private float moveRotationSmoothing = 0.1f;
    [SerializeField] private float headRotationOffset = 90f;
    public float WaterLevel = 0f;

    //Properties
    public bool IsGrounded => isGrounded;
    public float DefaultGravity => defaultGravity;

    private void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        defaultGravity = 9.8f;
        Rb.gravityScale = defaultGravity;
    }

    public void Move(Vector2 moveDir)
    {
        float targetXVelocity = moveDir.x * moveSpeed;
        Rb.linearVelocity = new Vector3(targetXVelocity, Rb.linearVelocity.y, 0);
    }

    public void LadderMove(Vector2 moveDir)
    {
        float targetYVelocity = moveDir.y * climbSpeed;

        Rb.linearVelocity = new Vector3 (0, targetYVelocity, 0);
    }

    public void Swim(Vector2 moveDir)
    {
        Vector2 targetVelocity = moveDir * maxSwimSpeed;

        moveVelocity = Vector2.MoveTowards(moveVelocity, targetVelocity, swimAcceleration * Time.fixedDeltaTime);

        //Actual force
        Vector2 neededAccel = (targetVelocity - Rb.linearVelocity) / Time.fixedDeltaTime;

        neededAccel = Vector2.ClampMagnitude(neededAccel, maxSwimAccelerationForce);

        Rb.AddForce(neededAccel * Rb.mass);
    }

    public void SwimTurn(Vector2 moveDir)
    {
        if (moveDir.magnitude > 0)
        {
            float targetAngle = Mathf.Atan2(moveDir.y, moveDir.x) * Mathf.Rad2Deg;
            targetAngle -= headRotationOffset; //Add the head offset to have the head facing forward

            //Smoothly rotate 
            float currentAngle = transform.eulerAngles.z;
            float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, moveRotationSmoothing);

            transform.rotation = Quaternion.Euler(0, 0, newAngle);
        }
        else
        {
            ResetOrientation();
        }

        Turn(moveDir);
    }

    public void ResetOrientation()
    {
        float currentAngle = transform.eulerAngles.z;
        float newAngle = Mathf.LerpAngle(currentAngle, 0, moveRotationSmoothing);
        transform.rotation = Quaternion.Euler(0, 0, newAngle);
    }

    private void FlipSprite()
    {
        isFacingRight = !isFacingRight;
        Vector3 newScale = transform.localScale;
        newScale.x = Mathf.Abs(newScale.x) * (isFacingRight ? 1 : -1);
        transform.localScale = newScale;
    }

    public void Turn(Vector2 moveDir)
    {

        bool isMovingRight = moveDir.x > 0.1f;
        bool isMovingLeft = moveDir.x < -0.1f;

        //Only flip when changing horizontal direction significantly
        if ((isMovingRight && !isFacingRight) || (isMovingLeft && isFacingRight))
        {
            //Only flip during substantial horizontal movement
            if (Mathf.Abs(moveDir.x) > 0.7f)
            {
                FlipSprite();
            }
        }
    }

    public void CheckGrounded()
    {
        Vector2 bottomCenter = new Vector2(col.bounds.center.x, col.bounds.min.y);
        groundHit = Physics2D.BoxCast(bottomCenter, col.bounds.size, 0f, Vector2.down, groundDetectionRayLength, groundLayer);

        if (groundHit)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }
}
