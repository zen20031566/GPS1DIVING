using UnityEngine;

[CreateAssetMenu(fileName = "NewEnemy", menuName = "Enemy/Enemy Data")]
public class EnemyDataSO : ScriptableObject
{
    public EnemyData EnemyData;
    public GameObject Prefab;
    public Sprite Sprite;

    [Header("Spawn Settings")]
    [Range(0f, 1f)]
    public float SpawnWeight = 0.5f; // Probability weight for spawning
}
