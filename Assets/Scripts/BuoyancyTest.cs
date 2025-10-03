using UnityEngine;

public class BuoyancyTest : MonoBehaviour
{

    public float waterLevel = 0f;  // The Y position of the water level
    public float density = 1f;  // Density of the object (less than 1 for floating)
    public float volume = 1f;  // Volume of the object (in 2D, it could be width * height of the collider)
    public Rigidbody2D rb2D;

    public float buoyancyStrength = 10f;  // Strength of the buoyant force
    public float dragInWater = 2f;  // Drag when submerged in water

    void Start()
    {
        rb2D = GetComponent<Rigidbody2D>();
        volume = GetComponent<Collider2D>().bounds.size.y;
    }

    void FixedUpdate()
    {
        // Check if the object is submerged
        float submergedHeight = Mathf.Max(0, transform.position.y - waterLevel);  // How deep the object is below the water

        if (submergedHeight > 0)
        {
            // Calculate the buoyant force (Archimedes' principle)
            float buoyantForce = density * volume * Physics2D.gravity.magnitude;  // Simplified buoyancy calculation

            // Apply buoyant force (upward)
            rb2D.AddForce(Vector2.up * buoyantForce * buoyancyStrength);

            // Apply additional drag to simulate water resistance
            rb2D.linearDamping = dragInWater;
        }
        else
        {
            // No buoyancy if above the water level
            rb2D.linearDamping = 0;  // No drag if the object is above the water level
        }
    }
}

