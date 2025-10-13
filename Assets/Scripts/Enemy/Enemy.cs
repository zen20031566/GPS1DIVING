using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyDataSO enemyDataSO;
    private EnemyData enemyData;

    private EnemyStateMachine stateMachine;
    private Rigidbody2D rb;
    private SpriteRenderer spriteRenderer;
    private Collider2D col;

    private Transform playerTransform;
    private float currentHealth;

    // Wander variables
    private Vector2 wanderDirection;
    private float wanderTimer;

    // Chase variables
    private float chaseTimer;

    // Properties
    public EnemyData EnemyData => enemyData;
    public Rigidbody2D Rb => rb;
    public Transform PlayerTransform => playerTransform;
    public Vector2 WanderDirection { get => wanderDirection; set => wanderDirection = value; }
    public float WanderTimer { get => wanderTimer; set => wanderTimer = value; }
    public float ChaseTimer { get => chaseTimer; set => chaseTimer = value; }
    public EnemyStateMachine StateMachine => stateMachine;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();

        if (enemyDataSO != null)
        {
            enemyData = enemyDataSO.EnemyData;
            currentHealth = enemyData.Health;
        }
    }

    private void Start()
    {
        playerTransform = GameManager.Instance.PlayerTransform;

        // Only initialize if data is set
        if (enemyData != null)
        {
            InitializeStateMachine();
        }
    }

    private void InitializeStateMachine()
    {
        // Initialize state machine
        stateMachine = new EnemyStateMachine();

        EnemyWanderState wanderState = new EnemyWanderState(this, stateMachine);
        EnemyChaseState chaseState = new EnemyChaseState(this, stateMachine);
        EnemyAttackState attackState = new EnemyAttackState(this, stateMachine);

        stateMachine.Initialize(wanderState);

        // Random initial wander direction
        wanderDirection = Random.insideUnitCircle.normalized;
        wanderTimer = enemyData.WanderChangeTime;
    }

    private void Update()
    {
        if (stateMachine != null && stateMachine.CurrentState != null)
        {
            stateMachine.CurrentState.Update();
        }
    }

    private void FixedUpdate()
    {
        if (stateMachine != null && stateMachine.CurrentState != null)
        {
            stateMachine.CurrentState.FixedUpdate();
        }
    }

    public void Move(Vector2 direction, float speed)
    {
        rb.linearVelocity = direction.normalized * speed;
        FlipSprite(direction);
    }

    private void FlipSprite(Vector2 direction)
    {
        if (direction.x > 0)
            spriteRenderer.flipX = false;
        else if (direction.x < 0)
            spriteRenderer.flipX = true;
    }

    public float GetDistanceToPlayer()
    {
        if (playerTransform == null) return Mathf.Infinity;
        return Vector2.Distance(transform.position, playerTransform.position);
    }

    public Vector2 GetDirectionToPlayer()
    {
        if (playerTransform == null) return Vector2.zero;
        return (playerTransform.position - transform.position).normalized;
    }

    public void TakeDamage(float damage)
    {
        currentHealth -= damage;

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        // Give rewards
        GameEventsManager.Instance.GoldEvents.AddGold(enemyData.GoldReward);

        // Notify spawner
        GameEventsManager.Instance.EnemyEvents.EnemyDied(this);

        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Enemy collided with: " + collision.gameObject.name + " Tag: " + collision.gameObject.tag);

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Collision with Player detected!");
            Player player = collision.gameObject.GetComponent<Player>();
            if (player != null)
            {
                Debug.Log("Player component found, dealing damage: " + enemyData.Damage);
                if (player.PlayerHealth != null)
                {
                    player.PlayerHealth.TakeDamage(enemyData.Damage);
                    Debug.Log("Damage applied successfully!");
                }
                else
                {
                    Debug.LogError("PlayerHealth component is NULL!");
                }
                Die(); // Fish dies after hitting player
            }
            else
            {
                Debug.LogError("Player component not found on collision object!");
            }
        }
    }

    public void InitializeEnemy(EnemyDataSO data)
    {
        enemyDataSO = data;
        enemyData = data.EnemyData;
        currentHealth = enemyData.Health;

        if (spriteRenderer != null && data.Sprite != null)
        {
            spriteRenderer.sprite = data.Sprite;
        }

        // Initialize state machine after data is set
        if (stateMachine == null)
        {
            InitializeStateMachine();
        }
    }
}
