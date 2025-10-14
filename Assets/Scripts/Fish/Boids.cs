using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class Boids : MonoBehaviour
{

    //private void CheckCollision()
    //{

    //}

    private float viewAngle;
    private float viewAngleRadians;
    private int numViewDirections = 20;

    private void InitializeViewAngleDirections()
    {
        viewAngleRadians = Mathf.Deg2Rad * viewAngle;
        float angleIncrement = viewAngleRadians / numViewDirections;

        //RaycastHit2D hitRightSide = Physics2D.Raycast(transform.position,
        ////(transform.up * angle + transform.right * (10 - Mathf.Abs(angle))).normalized,
        ////seeDistance);
    }

}
