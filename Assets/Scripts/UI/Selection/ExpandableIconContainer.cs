using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ExpandableIconContainer : MonoBehaviour
{
    [SerializeField] private SelectPage selectPage;

    [SerializeField] private VerticalLayoutGroup sizeFitter;
    [SerializeField] private RectTransform content;
    [SerializeField] private Dictionary<string, ExpandableIcon> icons = new Dictionary<string, ExpandableIcon>();

    public VerticalLayoutGroup SizeFitter { get => sizeFitter; }
    public SelectPage SelectPage { get => selectPage; }

    public void SetIcon(GameObject expandablePrefab, CompanyInfo company, bool isActive)
    {
        ExpandableIcon icon = Instantiate(expandablePrefab, content).GetComponent<ExpandableIcon>();
        icon.Initialise(this, company, isActive);
        icons.Add(company.CompanyName, icon);
    }

    public Dictionary<string, ExpandableIcon> GetIcons()
    {
        return icons;
    }

    public void SwitchIcon(string selectionParentName, SelectionIcon icon)
    {
        if (icons.ContainsKey(selectionParentName))
        {
            selectPage.SwitchIcon(this, selectionParentName, icon);
            return;
        }

    }

    //Coming from another expandable
    public void SetIcon(string selectionParentName, SelectionIcon selected)
    {
        if (icons.ContainsKey(selectionParentName))
        {
            icons[selectionParentName].SetChild(selected);
        }
    }

    public void ForceUpdate()
    {
        selectPage.ForceUpdate();
    }

    public void UpdateLayout()
    {
        LayoutRebuilder.ForceRebuildLayoutImmediate(sizeFitter.GetComponent<RectTransform>());
    }
}
