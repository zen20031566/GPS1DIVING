using System.Collections;
using System.Net;
using UnityEngine;
using UnityEngine.UIElements;

public class Lines : MonoBehaviour
{
    [SerializeField] private int segments = 5;
    [SerializeField] private float fovCenterAngle = 90f;
    [SerializeField] private float fovAngle = 90f;
    [SerializeField] private float radius = 2f;
   

    [SerializeField] private float speed = 5f;
    [SerializeField] private float repelForce = 5f;
    Rigidbody2D rb;

    [SerializeField] private float rotationSpeed = 5f;
    private Vector2 targetDirection;
    [SerializeField] private float rayDistance = 2f;
    [SerializeField] private LayerMask playerMask;

    private void Start()
    {
        targetDirection = transform.up;
        rb = GetComponent<Rigidbody2D>();
    }

   
    private void FixedUpdate()
    {
        rb.linearVelocity = transform.up * speed;
        TurnTowardsTarget();
        HandleObstacle();
    }

    private void TurnTowardsTarget()
    {
        Quaternion targetRotation = Quaternion.LookRotation(transform.forward, targetDirection);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
        rb.SetRotation(rotation);
    }

    private void HandleObstacle()
    {
        RaycastHit2D rayHit = Physics2D.Raycast(transform.position, transform.up, rayDistance, ~playerMask);
        Debug.DrawRay(transform.position, transform.up * rayDistance, Color.red);

        if (rayHit)
        {
            Quaternion targetRotation = Quaternion.LookRotation(transform.forward, rayHit.normal);
            Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            targetDirection = rotation * Vector2.up; //after rotation we want to know the direction so we point there
        }
    }

    private void DrawFovLines()
    {
        Gizmos.color = Color.green;
        float startAngle = fovCenterAngle - (fovAngle / 2);
        float anglePerSegment = fovAngle / segments; //Angle inbetween each points

        for (int i = 0; i < segments; i++)
        {
            float angle = startAngle + (anglePerSegment * i);
            float angleRad = angle * Mathf.Deg2Rad; //Convert to radians

            float x = Mathf.Cos(angleRad) * radius;
            float y = Mathf.Sin(angleRad) * radius;

            Vector3 endPoint = transform.position + new Vector3(x, y, 0); //Add transform position offset
            Gizmos.DrawLine(transform.position, endPoint);
        }
    }

    private void OnDrawGizmos()
    {
        
        //DrawFovLines();
    }

}
