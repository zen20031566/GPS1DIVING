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
            // Change wander direction
            enemy.WanderDirection = GetValidWanderDirection();
            enemy.WanderTimer = enemy.EnemyData.WanderChangeTime;
        }
    }

    public override void FixedUpdate()
    {
        // Move in wander direction
        enemy.Move(enemy.WanderDirection, enemy.EnemyData.MoveSpeed);
    }

    public override void Exit()
    {
        // Cleanup if needed
    }
}
