using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody2D rb;

    private float rideHeight;
    private float rideSpringStrength;
    private float rideSpringDamper;

    private void GroundCollision()
    {
        Vector2 rayDir = Vector2.zero;
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, rayDir);

        Vector2 currentVel = rb.linearVelocity;
        Vector2 otherVel = Vector2.zero;

        Rigidbody2D hitBody = rayHit.rigidbody;
        if (hitBody != null )
        {
            otherVel = hitBody.linearVelocity;
        }

        float rayDirVel = Vector2.Dot(rayDir, currentVel);
        float otherDirVel = Vector2.Dot(rayDir, currentVel);

        float relativeVel = rayDirVel - otherDirVel;    

        //F=kx

        float x = rayHit.distance - rideHeight;

        float springForce = (x * rideSpringStrength) - (relativeVel * rideSpringDamper);

        Debug.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y) + (rayDir * springForce), Color.yellow);

        rb.AddForce(rayDir * springForce);

        if (hitBody != null)
        {
            hitBody.AddForceAtPosition(rayDir * -springForce, rayHit.point);
        }
    }
}
