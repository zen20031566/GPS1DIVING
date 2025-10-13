using UnityEngine;

public class KeepInWater : MonoBehaviour
{
    [SerializeField] private float waterSurfaceY = 0; // Set this to your water level

    private Rigidbody2D rb;
    private Enemy enemy;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemy = GetComponent<Enemy>();
    }

    private void FixedUpdate()
    {
        // If fish is trying to go above water surface
        if (transform.position.y >= waterSurfaceY - 0.5f)
        {
            // Hard clamp position below water surface
            Vector3 pos = transform.position;
            pos.y = waterSurfaceY - 0.5f;
            transform.position = pos;

            // Force velocity downward immediately, kill upward movement
            Vector2 velocity = rb.linearVelocity;
            velocity.y = Mathf.Min(velocity.y, -2f); // Force downward movement, minimum -2
            rb.linearVelocity = velocity;

            // Change wander direction to go sideways/downward instead of upward
            if (enemy != null)
            {
                Vector2 newDirection = new Vector2(
                    Random.Range(-1f, 1f), // Random horizontal
                    Random.Range(-1f, -0.3f) // Always downward
                );
                enemy.WanderDirection = newDirection.normalized;
                enemy.WanderTimer = enemy.EnemyData.WanderChangeTime; // Reset timer
            }
        }
    }
}
