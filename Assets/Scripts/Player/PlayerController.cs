using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    private float rideHeight;
    private float rideSpringStrength;
    private float rideSpringDamper;

    public float WaterLevel;

    //Movement variables
    public float MaxSpeed;
    public float Acceleration; //How fast to reach max speed
    public float MaxAccelerationForce; //Limits maximum force that can be applied when accelerating
    public  Vector2 ForceScale = Vector2.one;

    private Vector2 moveVelocity;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 moveDir)
    {
        Vector2 targetVelocity = moveDir * MaxSpeed;

        moveVelocity = Vector2.MoveTowards(moveVelocity, targetVelocity, Acceleration * Time.fixedDeltaTime);

        //Actual force
        Vector2 neededAccel = (targetVelocity - rb.linearVelocity) / Time.fixedDeltaTime;

        neededAccel = Vector2.ClampMagnitude(neededAccel, MaxAccelerationForce);

        rb.AddForce(Vector2.Scale(neededAccel * rb.mass, ForceScale));
    }

    public void GroundCollision()
    {
        Vector2 rayDir = Vector2.zero;
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, rayDir);

        Vector2 currentVelocity = rb.linearVelocity;
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

        rb.AddForce(rayDir * springForce);

        if (hitBody != null)
        {
            hitBody.AddForceAtPosition(rayDir * -springForce, rayHit.point);
        }
    }
}
