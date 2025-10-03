using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerMovementOld : MonoBehaviour
{
    private Rigidbody2D rb;

    //Collision checks
    [SerializeField] private float groundDetectionRayLength = 0.02f;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private BoxCollider2D feetCol;
    private RaycastHit2D groundHit;
    private bool isGrounded = false;

    //Movement variables
    [SerializeField] private float moveAcceleration = 5f;
    [SerializeField] private float moveDeceleration = 20f;
    [SerializeField] private float moveSpeed = 12f;
    [SerializeField] private float gravity = 10f;

    //In water
    [SerializeField] private float waterLevel = 0f;
    [SerializeField] private float buoyancyForce = 30f;
    [SerializeField] private float maxBuoyancyDepth = 2f;

    private Vector2 moveVelocity;
    private bool isFacingRight;
    private bool isInWater;

    private float oriMoveAcceleration = 5f;
    private float oriMoveDeceleration = 20f;

    void Start()
    {
        isFacingRight = true;
        rb = GetComponent<Rigidbody2D>();

        oriMoveAcceleration = moveAcceleration;
        oriMoveDeceleration = moveDeceleration;
    }

    private void Update()
    {
        if (InputManager.InteractPressed)
        {
            Debug.Log("Interact");
        }

        if (!isGrounded)
        {
            isInWater = transform.position.y <= waterLevel;
        }
        else
        {
            isInWater = false;
        }
    }

    void FixedUpdate()
    {
        Debug.Log(isGrounded);
        CheckGrounded();
        Float(InputManager.MoveDirection);
        Gravity();
        Move(InputManager.MoveDirection);

        //Apply move velocity
        rb.linearVelocity = moveVelocity;
    }

    private void Move(Vector2 moveDirection)
    {
        if (moveDirection != Vector2.zero)
        {
            Turn(moveDirection);

            Vector2 targetVelocity = Vector2.zero;
            if (InputManager.MoveHeld)
            {
                targetVelocity = new Vector2(moveDirection.x, moveDirection.y) * moveSpeed;
            }

            if (isGrounded)
            {
                moveAcceleration *= 2;
                moveDeceleration *= 2;
            }
            else
            {
                moveAcceleration = oriMoveAcceleration;
                moveDeceleration = oriMoveDeceleration; 
            }

            if (isInWater)
            {
                moveVelocity = Vector2.Lerp(moveVelocity, targetVelocity, moveAcceleration * Time.fixedDeltaTime);
            }
            else
            {
                moveVelocity.x = Mathf.Lerp(moveVelocity.x, targetVelocity.x, moveAcceleration * Time.fixedDeltaTime);
            }

            //Snap to targetVelocity when close enough needed due to lerp
            if (Vector2.Distance(moveVelocity, targetVelocity) < 0.1f)
            {
                moveVelocity = targetVelocity;
            }

            //Prevent upwards movement when above water
            if (moveDirection.y >= 0 && !isInWater &&isGrounded)
            {
                targetVelocity.y = 0;
            }
        }

        //If no movement input
        else if (moveDirection == Vector2.zero)
        {
            if (isInWater)
            {
                moveVelocity = Vector2.Lerp(moveVelocity, Vector2.zero, moveDeceleration * Time.fixedDeltaTime);
            }
            else
            {
                moveVelocity.x = Mathf.Lerp(moveVelocity.x, 0f, moveDeceleration * Time.fixedDeltaTime);
            }

            //Snap to 0 when close enough needed due to lerp
            if (Vector2.Distance(moveVelocity, Vector2.zero) < 0.1f)
            {
                moveVelocity = Vector2.zero;
            }
        }  
    }

    private void Turn(Vector2 moveDirection)
    {
        if (isFacingRight && moveDirection.x < 0)
        {
            isFacingRight = false;
            transform.Rotate(0f, -180, 0f);
        }
        else if (!isFacingRight && moveDirection.x > 0)
        {
            isFacingRight = true;
            transform.Rotate(0f, 180, 0f);
        }
    }

    private void Gravity()
    {
        //When not in water
        if (!isInWater && !isGrounded)
        {
            moveVelocity.y -= gravity * Time.fixedDeltaTime;
        }
        else if (isGrounded && moveVelocity.y < 0)
        {
            moveVelocity.y = 0f;
        }
    }

    private void Float(Vector2 moveDirection)
    {
        if(isInWater && !isGrounded)
        {
            float depth = Mathf.Abs(transform.position.y);
            float depthMultiplier = Mathf.Clamp(depth, waterLevel, maxBuoyancyDepth); //Cap at max depth

            if (transform.position.y > -maxBuoyancyDepth && moveDirection.y <= 0)
            {
                moveVelocity.y += buoyancyForce * depthMultiplier * Time.fixedDeltaTime;
            }
        }
    }

    private void CheckGrounded()
    {
        groundHit = Physics2D.BoxCast(feetCol.bounds.center, feetCol.bounds.size, 0f, Vector2.down, groundDetectionRayLength, groundLayer);

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

