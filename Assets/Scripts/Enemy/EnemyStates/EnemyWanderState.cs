using UnityEngine;

public class EnemyWanderState : EnemyState
{
    public EnemyWanderState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void Enter()
    {
        // Start wandering with random direction that respects depth
        enemy.WanderDirection = GetValidWanderDirection();
        enemy.WanderTimer = enemy.EnemyData.WanderChangeTime;
    }

    private Vector2 GetValidWanderDirection()
    {
        Vector2 direction = Random.insideUnitCircle.normalized;

        // Check if near depth boundaries and adjust direction
        float currentDepth = enemy.transform.position.y;
        float minDepth = enemy.EnemyData.MinDepth;
        float maxDepth = enemy.EnemyData.MaxDepth;
        float boundaryBuffer = 3f;

        // If near max depth (shallow limit), don't go upward
        if (currentDepth >= maxDepth - boundaryBuffer && direction.y > 0)
        {
            direction.y = Random.Range(-1f, 0f); // Force downward or horizontal
        }

        // If near min depth (deep limit), don't go downward
        if (currentDepth <= minDepth + boundaryBuffer && direction.y < 0)
        {
            direction.y = Random.Range(0f, 1f); // Force upward or horizontal
        }

        return direction.normalized;
    }

    public override void Update()
    {
        // Check if player is in detection radius
        float distanceToPlayer = enemy.GetDistanceToPlayer();

        if (distanceToPlayer <= enemy.EnemyData.DetectionRadius)
        {
            // Switch to chase state
            stateMachine.ChangeState(new EnemyChaseState(enemy, stateMachine));
            return;
        }

        // Update wander timer
        enemy.WanderTimer -= Time.deltaTime;

        if (enemy.WanderTimer <= 0)
        {
            // Change wander direction (respecting depth boundaries)
            enemy.WanderDirection = GetValidWanderDirection();
            enemy.WanderTimer = enemy.EnemyData.WanderChangeTime;
        }

        // Continuously check if at boundaries and need to turn
        CheckDepthBoundaries();
    }

    public override void FixedUpdate()
    {
        // Move in wander direction
        enemy.Move(enemy.WanderDirection, enemy.EnemyData.MoveSpeed);
    }

    private void CheckDepthBoundaries()
    {
        float currentDepth = enemy.transform.position.y;
        float minDepth = enemy.EnemyData.MinDepth;
        float maxDepth = enemy.EnemyData.MaxDepth;

        // If at or beyond max depth (too shallow), force downward
        if (currentDepth >= maxDepth)
        {
            Vector2 newDirection = new Vector2(
                Random.Range(-1f, 1f),
                Random.Range(-1f, -0.3f) // Downward
            );
            enemy.WanderDirection = newDirection.normalized;
            enemy.WanderTimer = enemy.EnemyData.WanderChangeTime;
        }
        // If at or beyond min depth (too deep), force upward
        else if (currentDepth <= minDepth)
        {
            Vector2 newDirection = new Vector2(
                Random.Range(-1f, 1f),
                Random.Range(0.3f, 1f) // Upward
            );
            enemy.WanderDirection = newDirection.normalized;
            enemy.WanderTimer = enemy.EnemyData.WanderChangeTime;
        }
    }

    public override void Exit()
    {
        // Cleanup if needed
    }
}
