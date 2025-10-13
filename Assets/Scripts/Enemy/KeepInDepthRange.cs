using UnityEngine;

public class KeepInDepthRange : MonoBehaviour
{
    private Enemy enemy;
    private Rigidbody2D rb;

    [SerializeField] private float slowdownZone = 2f; // How far from boundaries to start slowing
    [SerializeField] private float minSpeedMultiplier = 0.3f; // Minimum speed at boundaries

    private void Start()
    {
        enemy = GetComponent<Enemy>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (enemy == null || enemy.EnemyData == null) return;

        float currentDepth = transform.position.y;
        float minDepth = enemy.EnemyData.MinDepth;
        float maxDepth = enemy.EnemyData.MaxDepth;

        // Check if approaching depth boundaries
        float distanceFromMax = Mathf.Abs(currentDepth - maxDepth);
        float distanceFromMin = Mathf.Abs(currentDepth - minDepth);

        // If outside depth range or very close, push back and slow down
        if (currentDepth > maxDepth) // Above maximum (too shallow)
        {
            // Push down
            Vector2 velocity = rb.linearVelocity;
            velocity.y = Mathf.Min(velocity.y, -1f); // Force downward
            rb.linearVelocity = velocity;

            // Change wander direction if in wander state
            if (enemy.WanderDirection.y > 0)
            {
                Vector2 newDirection = new Vector2(
                    Random.Range(-1f, 1f),
                    Random.Range(-1f, -0.3f) // Downward
                );
                enemy.WanderDirection = newDirection.normalized;
            }
        }
        else if (currentDepth < minDepth) // Below minimum (too deep)
        {
            // Push up
            Vector2 velocity = rb.linearVelocity;
            velocity.y = Mathf.Max(velocity.y, 1f); // Force upward
            rb.linearVelocity = velocity;

            // Change wander direction if in wander state
            if (enemy.WanderDirection.y < 0)
            {
                Vector2 newDirection = new Vector2(
                    Random.Range(-1f, 1f),
                    Random.Range(0.3f, 1f) // Upward
                );
                enemy.WanderDirection = newDirection.normalized;
            }
        }
        else if (distanceFromMax < slowdownZone || distanceFromMin < slowdownZone)
        {
            // In slowdown zone - gradually reduce speed
            float closestDistance = Mathf.Min(distanceFromMax, distanceFromMin);
            float speedMultiplier = Mathf.Lerp(minSpeedMultiplier, 1f, closestDistance / slowdownZone);

            // Apply speed reduction
            rb.linearVelocity *= speedMultiplier;

            // Start turning away from boundary
            if (distanceFromMax < slowdownZone && rb.linearVelocity.y > 0)
            {
                // Approaching max depth (shallow), turn downward
                Vector2 newDirection = new Vector2(
                    Random.Range(-1f, 1f),
                    Random.Range(-0.5f, 0f)
                );
                enemy.WanderDirection = newDirection.normalized;
            }
            else if (distanceFromMin < slowdownZone && rb.linearVelocity.y < 0)
            {
                // Approaching min depth (deep), turn upward
                Vector2 newDirection = new Vector2(
                    Random.Range(-1f, 1f),
                    Random.Range(0f, 0.5f)
                );
                enemy.WanderDirection = newDirection.normalized;
            }
        }
    }
}
