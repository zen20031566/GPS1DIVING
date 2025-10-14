using UnityEngine;
using TMPro;

public class DepthCounter : MonoBehaviour
{
    [SerializeField] TMP_Text depthText;
    float depth;

    void Update()
    {
        depth = Mathf.Round(GameManager.Instance.PlayerTransform.position.y);

        if (depth <= 0)
        {
            depthText.text = "Depth: " + Mathf.Abs(depth) + "m";
        }
        else if (depth > 0)
        {
            depthText.text = "Depth: " + 0 + "m";
        }
        
    }
}
