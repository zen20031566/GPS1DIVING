using UnityEngine;

public class ItemGrid : MonoBehaviour
{

    const float tileSizeWidth = 76;
    const float tileSizeHeight = 76;

    private RectTransform rectTransform;

    Vector2 positionOnTheGrid = new Vector2();
    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public Vector2Int GetGridPosition(Vector2 mousePosition)
    {
        positionOnTheGrid.x = mousePosition.x - rectTransform.position.x;
    }
}
