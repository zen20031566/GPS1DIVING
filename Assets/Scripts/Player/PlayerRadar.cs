using UnityEngine;
using System.Collections.Generic;

public class PlayerRadar : MonoBehaviour
{
    [Header("Radar Settings")]
    public float detectionRadius = 24f; //range of radar
    public LayerMask enemyLayer; //set this to Enemy layer in Inspector

    [Header("Debug")]
    public bool showRadarGizmo = true;
    public List<Transform> detectedEnemies = new List<Transform>();

    void Update()
    {
        ScanForEnemies();
    }

    void ScanForEnemies()
    {
        //clear previous list
        detectedEnemies.Clear();

        //get all enemies in range
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, detectionRadius, enemyLayer);

        foreach (Collider2D hit in hits)
        {
            detectedEnemies.Add(hit.transform);
            //highlight enemies in range
            hit.GetComponent<SpriteRenderer>().color = Color.red;
            Debug.Log("Detected " + detectedEnemies.Count + " enemies nearby");
        }
    }

    void OnDrawGizmosSelected() //circle
    {
        if (showRadarGizmo)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, detectionRadius);
        }
    }
}
