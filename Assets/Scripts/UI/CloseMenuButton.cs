using Unity.VisualScripting;
using UnityEngine;

public class CloseMenuButton : MonoBehaviour
{
    public void OnClick()
    {
        GameEventsManager.Instance.GameUIEvents.CloseMenu();
    }
}
