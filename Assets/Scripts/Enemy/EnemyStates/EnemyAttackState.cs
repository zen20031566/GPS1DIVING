using UnityEngine;

public class EnemyAttackState : EnemyState
{
    public EnemyAttackState(Enemy enemy, EnemyStateMachine stateMachine) : base(enemy, stateMachine) { }

    public override void Enter()
    {
        // Attack is handled by collision in Enemy.cs
        // This state is just to maintain momentum towards player
    }

    public override void Update()
    {
        float distanceToPlayer = enemy.GetDistanceToPlayer();

        // If somehow didn't collide and moved away, go back to chase
        if (distanceToPlayer > enemy.EnemyData.AttackRadius * 2f)
        {
            stateMachine.ChangeState(new EnemyChaseState(enemy, stateMachine));
        }
    }

    public override void FixedUpdate()
    {
        // Continue moving towards player at high speed
        Vector2 directionToPlayer = enemy.GetDirectionToPlayer();
        enemy.Move(directionToPlayer, enemy.EnemyData.ChaseSpeed * 1.2f);
    }

    public override void Exit()
    {
        // Cleanup if needed
    }
}
