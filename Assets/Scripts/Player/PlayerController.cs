using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerController : MonoBehaviour
{
    public Rigidbody2D Rb;
    private bool isFacingRight;

    private float rideHeight;
    private float rideSpringStrength;
    private float rideSpringDamper;

    //Movement variables
    private float defaultGravity = 9.8f;
    public float DefaultGravity => defaultGravity;
    private Vector2 moveVelocity;
    public float MaxSpeed;
    public float Acceleration; //How fast to reach max speed
    public float MaxAccelerationForce; //Limits maximum force that can be applied when accelerating

    //Swim variables
    public float WaterLevel;
    public float moveRotationSmoothing = 0.2f;
    public float headRotationOffset = 75f;

    private void Start()
    {
        Rb = GetComponent<Rigidbody2D>();
        defaultGravity = 9.8f;
    }

    public void Move(Vector2 moveDir)
    {
        Vector2 targetVelocity = moveDir * MaxSpeed;

        moveVelocity = Vector2.Lerp(moveVelocity, targetVelocity, Acceleration * Time.fixedDeltaTime);

        //Actual force
        Vector2 neededAccel = (targetVelocity - Rb.linearVelocity) / Time.fixedDeltaTime;

        neededAccel = Vector2.ClampMagnitude(neededAccel, MaxAccelerationForce);

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

    public void GroundCollision()
    {
        Vector2 rayDir = Vector2.zero;
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, rayDir);

        Vector2 currentVelocity = Rb.linearVelocity;
        Vector2 otherVel = Vector2.zero;

        Rigidbody2D hitBody = rayHit.rigidbody;
        if (hitBody != null )
        {
            otherVel = hitBody.linearVelocity;
        }

        float rayDirVelocity = Vector2.Dot(rayDir, currentVelocity);
        float otherDirVelocity = Vector2.Dot(rayDir, currentVelocity);

        float relativeVelocity = rayDirVelocity - otherDirVelocity;    

        //F=kx
        float x = rayHit.distance - rideHeight;

        float springForce = (x * rideSpringStrength) - (relativeVelocity * rideSpringDamper);

        Debug.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y) + (rayDir * springForce), Color.yellow);

        Rb.AddForce(rayDir * springForce);

        if (hitBody != null)
        {
            hitBody.AddForceAtPosition(rayDir * -springForce, rayHit.point);
        }
    }
}
