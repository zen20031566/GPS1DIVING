using System.Collections.Generic;
using UnityEngine;

public class MenuTabGroup : MonoBehaviour
{
    [SerializeField] private List<MenuTabButton> tabButtons;
    [SerializeField] private Color tabIdleColor = new Color(41f / 255f, 41f / 255f, 41f / 255f);
    [SerializeField] private Color tabHoverColor = Color.gray;
    [SerializeField] private Color tabActiveColor = Color.gray;
    [SerializeField] private MenuTabButton selectedTab;
    [SerializeField] private List<GameObject> pages;

    private void OnEnable()
    {
        OnTabSelected(tabButtons[tabButtons.Count - 1]);
    }

    private void OnDisable()
    {
        ResetTabs();
    }

    public void SubscribeTab(MenuTabButton button)
    {
        if (tabButtons == null)
        {
            tabButtons = new List<MenuTabButton>();
        }

        tabButtons.Add(button);
    }

    public void OnTabEnter(MenuTabButton button)
    {
        ResetTabs();
        if (selectedTab == null || button != selectedTab)
        {
            button.Background.color = tabHoverColor;
        } 
    }

    public void OnTabExit(MenuTabButton button)
    {
        ResetTabs();
    }

    public void OnTabSelected(MenuTabButton button)
    {
        selectedTab = button;
        ResetTabs();
        button.Background.color = tabActiveColor;
        int index = button.transform.GetSiblingIndex(); 
        for (int i = 0; i < pages.Count; i++)
        {
            if (i == index)
            {
                pages[i].SetActive(true);
            }
            else
            {
                pages[i].SetActive(false);
            }
        }
    }

    public void ResetTabs()
    {
        foreach (MenuTabButton button in tabButtons)
        {
            if (selectedTab != null && button == selectedTab) continue;
            button.Background.color = tabIdleColor;
        }
    }
    
}
