using UnityEngine;

public class Boids : MonoBehaviour
{
    //Array to hold the 8 main directions
    private Vector2[] directions = new Vector2[8];
    private Rigidbody2D rb;
    private BoxCollider2D col;

    [SerializeField] private float moveSpeed = 5f;

    private Vector2 moveVelocity;
    private Vector2 forwardDir;
    private float colRadius;
    [SerializeField] private float avoidDistance = 2f;
    [SerializeField] LayerMask obstacleMask;

    [SerializeField] float maxSteerForce = 3f;

    [SerializeField] private float avoidCollisionWeight = 10f; //higher value means that the obj will steer more aggressively

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();
    }

    void Start()
    {
        directions[0] = Vector2.up;      
        directions[1] = Vector2.down;    
        directions[2] = Vector2.left;     
        directions[3] = Vector2.right;    
        directions[4] = new Vector2(-1, 1).normalized; //Up left
        directions[5] = new Vector2(1, 1).normalized; //Up right
        directions[6] = new Vector2(-1, -1).normalized; //Down left
        directions[7] = new Vector2(1, -1).normalized; //Down right

        forwardDir = transform.up;
        colRadius = col.bounds.size.x / 2;
    }

    private void FixedUpdate()
    {
        moveVelocity = forwardDir * moveSpeed;
       
        if (IsHeadingForCollision())
        {
            Vector2 collisionAvoidDir = CheckDirections();
            //Vector2 collisionAvoidForce = SteerTowards(collisionAvoidDir) * avoidCollisionWeight;
            //moveVelocity += collisionAvoidForce;

            moveVelocity = collisionAvoidDir * moveSpeed;
            forwardDir = moveVelocity.normalized;
        }

        rb.linearVelocity = moveVelocity;
    }

    private bool IsHeadingForCollision()
    {
        Debug.DrawRay(transform.position, forwardDir * avoidDistance, Color.yellow);
        DrawCircleCast(transform.position, forwardDir, avoidDistance, Color.yellow);
        if (Physics2D.CircleCast(transform.position, colRadius, forwardDir, avoidDistance, obstacleMask))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private Vector2 CheckDirections()
    {
        for (int i = 0; i < directions.Length; i++)
        {
            Vector2 dir = transform.TransformDirection(directions[i]);
            if (!Physics2D.CircleCast(transform.position, colRadius, dir, avoidDistance, obstacleMask))
            {
                Debug.DrawRay(transform.position, dir * avoidDistance, Color.green);
                DrawCircleCast(transform.position, dir, avoidDistance, Color.green);

                return dir;
            } 
            else
            {
                Debug.DrawRay(transform.position, dir * avoidDistance, Color.red);
                DrawCircleCast(transform.position, dir, avoidDistance, Color.red);
            }
        }
        //If all directions have an obstacle
        return forwardDir;
    }

    //Returns steering force
    private Vector2 SteerTowards(Vector2 vector)
    {
        //vector.normalized * moveSpeed this give u your desired velocity based on the velocity u want to steer to
        //we - the current velocity to get the difference between the 2 vectors
        //we do so to get the get how much u need to change your current velocity to reach the desired velocity

        Vector2 v = vector.normalized * moveSpeed - moveVelocity;

        return Vector2.ClampMagnitude(v, maxSteerForce); 
    }

    // Draw the CircleCast shape for visualization
    private void DrawCircleCast(Vector2 origin, Vector2 direction, float distance, Color color)
    {
        // Draw start circle
        DrawCircle(origin, colRadius, color);

        // Draw end circle  
        DrawCircle(origin + direction * distance, colRadius, color);

        // Draw connecting lines
        Vector2 perpendicular = new Vector2(-direction.y, direction.x).normalized * colRadius;
        Debug.DrawLine(origin + perpendicular, origin + direction * distance + perpendicular, color);
        Debug.DrawLine(origin - perpendicular, origin + direction * distance - perpendicular, color);
    }

    // Helper method to draw a circle using Debug.DrawLine
    private void DrawCircle(Vector2 center, float radius, Color color)
    {
        int segments = 12;
        float angleIncrement = 360f / segments * Mathf.Deg2Rad;

        for (int i = 0; i < segments; i++)
        {
            float angle1 = i * angleIncrement;
            float angle2 = (i + 1) * angleIncrement;

            Vector2 point1 = center + new Vector2(Mathf.Cos(angle1), Mathf.Sin(angle1)) * radius;
            Vector2 point2 = center + new Vector2(Mathf.Cos(angle2), Mathf.Sin(angle2)) * radius;

            Debug.DrawLine(point1, point2, color);
        }
    }

}
