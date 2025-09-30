using UnityEngine;

[RequireComponent(typeof(CircleCollider2D))]

public class GoToLocationQuestStep : QuestStep
{
    private CircleCollider2D col;
    private float triggerRadius;

    private Transform playerTransform;
    private Vector2 playerPos;

    private float distanceAway;
    private string baseDescription;

    private void Start()
    {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;

        if (playerTransform == null)
        {
            Debug.LogError("Player not found ensure that the player has the Player tag");
        }

        col = GetComponent<CircleCollider2D>();
        col.radius = triggerRadius;
        col.isTrigger = true;   
    }

    private void Update()
    {
        playerPos = playerTransform.position;

        distanceAway = Mathf.Round(Vector2.Distance(playerPos, transform.position) - triggerRadius);
        description = baseDescription + " " + distanceAway + "m";
        GameEventsManager.Instance.QuestStepEvents.QuestStepProgressChanged(questId);
    }

    public override void Configure(QuestStepConfig config)
    {
        transform.position = config.targetPosition;
        triggerRadius = config.triggerRadius;

        if (config.description != string.Empty)
        {
            baseDescription = config.description;
        }
        else
        {
            baseDescription = "Travel to location ";
        }

        distanceAway = Mathf.Round(Vector2.Distance(playerPos, transform.position) - triggerRadius);
        description = baseDescription + " " + distanceAway + "m";
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            FinishQuestStep();
        }
    }
}
