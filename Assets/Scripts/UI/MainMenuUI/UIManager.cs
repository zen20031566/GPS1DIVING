using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject OptionsPanel;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ToggleSettings()
    {
        OptionsPanel.SetActive(!OptionsPanel.activeSelf);
    }

    public void CloseSettings()
    {
        OptionsPanel.SetActive(false);
    }
}
