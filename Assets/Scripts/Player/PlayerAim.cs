using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAim : MonoBehaviour
{
    Player player;
    [SerializeField] private GameObject playerHead;
    private Vector3 mousePosition;
    [SerializeField] float moveRotationSmoothing = 0.5f;
    [SerializeField] private float maxAngle = 60f;
    [SerializeField] private float minAngle = -60f;

    private void Update()
    {
        mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 dirVector = (mousePosition - playerHead.transform.position).normalized;
        float targetAngle = Mathf.Atan2(dirVector.y, dirVector.x) * Mathf.Rad2Deg;
        targetAngle = Mathf.Clamp(targetAngle, minAngle, maxAngle);

        float currentAngle = playerHead.transform.eulerAngles.z;
        float newAngle = Mathf.LerpAngle(currentAngle, targetAngle, moveRotationSmoothing);

        playerHead.transform.rotation = Quaternion.Euler(0, 0, newAngle);  
    }
}
