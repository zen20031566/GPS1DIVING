using System;
using UnityEngine;

public class EnemyEvents
{
    public event Action<Enemy> OnEnemyDied;

    public void EnemyDied(Enemy enemy)
    {
        OnEnemyDied?.Invoke(enemy);
    }
}
