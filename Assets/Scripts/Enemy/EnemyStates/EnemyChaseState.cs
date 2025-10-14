using UnityEngine;

public class EnemyChaseState : EnemyState
{
    public EnemyChaseState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void Enter()
    {
        enemy.ChaseTimer = enemy.EnemyData.ChaseTime;
    }

    public override void Update()
    {
        float distanceToPlayer = enemy.GetDistanceToPlayer();

        // Check if close enough to attack
        if (distanceToPlayer <= enemy.EnemyData.AttackRadius)
        {
            stateMachine.ChangeState(new EnemyAttackState(enemy, stateMachine));
            return;
        }

        // Check if player escaped
        enemy.ChaseTimer -= Time.deltaTime;

        if (distanceToPlayer > enemy.EnemyData.DetectionRadius * 1.5f || enemy.ChaseTimer <= 0)
        {
            // Player escaped or gave up, go back to wandering
            stateMachine.ChangeState(new EnemyWanderState(enemy, stateMachine));
            return;
        }
    }

    public override void FixedUpdate()
    {
        // Move towards player at chase speed
        Vector2 directionToPlayer = enemy.GetDirectionToPlayer();
        enemy.Move(directionToPlayer.normalized, enemy.EnemyData.ChaseSpeed);
    }

    public override void Exit()
    {
        // Cleanup if needed
    }
}
