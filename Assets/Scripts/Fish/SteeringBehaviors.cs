using UnityEngine;

public class SteeringBehaviors : MonoBehaviour
{
    private Rigidbody2D rb;
    private BoxCollider2D col;

    [SerializeField] private float dirRayVisualizationLength = 3f;
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float maxSteerForce = 5f;
    Vector2 velocity;
    Vector2 transformPos;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        col = GetComponent<BoxCollider2D>();    
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        rb.linearVelocity = velocity;
        transformPos = transform.position;

        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (Input.GetMouseButton(0))
        {
            //Seek(mousePos);
            Arrival(mousePos);
        }
        else if (Input.GetMouseButton(1))
        {
            Flee(mousePos);
        }
        else
        {
            velocity = Vector2.zero;
        }
    }

    //Returns steering force
    private Vector2 SteerTowards(Vector2 targetVector)
    {
        //vector.normalized * moveSpeed this give u your desired velocity based on the velocity u want to steer to
        //we - the current velocity to get the difference between the 2 vectors
        //we do so to get the get how much u need to change your current velocity to reach the desired velocity

        Vector2 desiredVelocity = targetVector.normalized * moveSpeed;
        Vector2 steeringForce = desiredVelocity - velocity;

        return Vector2.ClampMagnitude(steeringForce, maxSteerForce);
    }

    private void Seek(Vector2 targetPosition)
    {
        Vector2 targetDir = targetPosition - transformPos;
        velocity += SteerTowards(targetDir);

        Debug.DrawRay(transformPos, targetDir * dirRayVisualizationLength, Color.green);
    }

    private void Flee(Vector2 targetPosition)
    {
        Vector2 targetDir = transformPos - targetPosition;
        velocity += SteerTowards(targetDir);

        Debug.DrawRay(transformPos, targetDir * dirRayVisualizationLength, Color.red);
    }

    private void Arrival(Vector2 targetPosition, float arrivalRadius = 0f)
    {
        Vector2 targetDir = transformPos - targetPosition;

        float distance = Vector2.Distance(targetPosition, transformPos);

        if (distance < arrivalRadius)
        {
            //Scale velocity based on distance so the closer we get we to target position slow down
            //I use a slowfactor cause I have a steer function otherwise u would set target velocity to *= distance / arrival radius
            //Then calculate the steering force the same way

            float slowFactor = Mathf.Clamp01(distance / arrivalRadius);
            velocity += SteerTowards(targetPosition) * slowFactor;

            Debug.DrawRay(transformPos, targetDir * dirRayVisualizationLength, Color.blue);
        }
        else
        {
            Seek(targetPosition); //When outside of the arrivalRadius we dont slowdown so its just seek behavior
        }
    }

    private void Pursue(Vector2 targetPosition, Vector2 targetVelocity, float minPredictionTime = 1)
    {
        //Predicted pos is current targeted position + target's velocity * specified t
        //x = initx + vt

        float distance = Vector2.Distance(targetPosition, transformPos);

        //With v = d/t we can get the t 
        float predictTime = Mathf.Min(distance / velocity.magnitude, minPredictionTime);

        Vector3 predictedTarget = targetPosition + targetVelocity * predictTime;    

        Seek(predictedTarget);
    }
}
