using UnityEngine;

[System.Serializable]
public class EnemyData
{
    [Header("Basic Info")]
    public string EnemyName;
    public EnemyType EnemyType;

    [Header("Stats")]
    public float Health = 10f;
    public float Damage = 10f;
    public float MoveSpeed = 3f;
    public float ChaseSpeed = 6f;

    [Header("Detection")]
    public float DetectionRadius = 5f;
    public float AttackRadius = 0.5f;

    [Header("Depth")]
    public DepthLayer DepthLayer;
    public float MinDepth; // Minimum Y position (deeper = more negative)
    public float MaxDepth; // Maximum Y position (shallower = less negative)

    [Header("Behavior")]
    public float WanderChangeTime = 3f; // How often to change wander direction
    public float ChaseTime = 5f; // How long to chase before giving up

    [Header("Rewards")]
    public int GoldReward = 5;
}

public enum EnemyType
{
    SmallFish,
    MediumFish,
    LargeFish,
    DeepSeaCreature,
    Boss
}

public enum DepthLayer
{
    Surface,      // 0 to -20
    Shallow,      // -20 to -40
    Medium,       // -40 to -70
    Deep,         // -70 to -100
    Abyss         // -100+
}
