using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ExpandableIcon : MonoBehaviour
{
    private ExpandableIconContainer container;

    [SerializeField] private Transform selectableContent;
    [SerializeField] private RectTransform ExpandableRect;
    [SerializeField] private TextMeshProUGUI iconName;
    [SerializeField] private TextMeshProUGUI iconNickname;
    [SerializeField] private TextMeshProUGUI iconBreakdown;
    [SerializeField] private Button selectButton;
    [SerializeField] private Toggle selectAllToggle;

    public Dictionary<string, SelectionIcon> Icons = new Dictionary<string, SelectionIcon>();

    public CompanyInfo companyInfo { get; private set; }

    private bool expanded = false;

    public void Initialise(ExpandableIconContainer newContainer, CompanyInfo info, bool isActive)
    {
        container = newContainer;
        companyInfo = info;

        foreach(SquadInfo squad in info.Squads)
        {
            GameObject selectable = Instantiate(container.SelectPage.selectablePrefab, selectableContent);
            SelectionIcon icon = selectable.GetComponent<SelectionIcon>();
            icon.Initialise(this, squad);
            Icons.Add(icon.Info.SquadName, icon);
            icon.IsActive = isActive;
        }

        Expand(false, true);
        gameObject.SetActive(isActive);
    }

    public void ToggleExpand()
    {
        Expand(!expanded);
    }

    private void Expand(bool state, bool forceOverride = false)
    {
        expanded = state;

        int activeIconsCount = 0;

        foreach (SelectionIcon icon in Icons.Values)
        {
            if (icon.IsActive == true || forceOverride == true)
            {
                icon.gameObject.SetActive(state);
                activeIconsCount += 1;
            }
        }

        if (state == true)
        {
            ExpandableRect.sizeDelta = new Vector3(ExpandableRect.sizeDelta.x, 210 + (160 * activeIconsCount));

            if (this.gameObject.activeSelf == true)
            {
                StartCoroutine(UpdateCanvas());
            }
            return;
        }

        ExpandableRect.sizeDelta = new Vector3(ExpandableRect.sizeDelta.x, 200);

        if (this.gameObject.activeSelf == true)
        {
            StartCoroutine(UpdateCanvas());
        }
    }

    public void SelectAll()
    {
        if(selectAllToggle.isOn == false)
        {
            foreach(SelectionIcon icon in Icons.Values)
            {
                if(icon.IsActive == false)
                {
                    SelectChild(icon, true);
                }
            }
            
            ExpandableRect.sizeDelta = new Vector3(ExpandableRect.sizeDelta.x, 210 +  (160 * Icons.Count));

            if (this.gameObject.activeSelf == true)
            {
                StartCoroutine(UpdateCanvas());
            }
            return;
        }

        foreach (SelectionIcon icon in Icons.Values)
        {
            if (icon.IsActive == true)
            {
                SelectChild(icon, true);
            }
        }

        ExpandableRect.sizeDelta = new Vector3(ExpandableRect.sizeDelta.x, 200);

        if (this.gameObject.activeSelf == true)
        {
            StartCoroutine(UpdateCanvas());
        }

        CheckSelectAllToggleStatus();
    }

    public bool IsCountMax()
    {
        foreach (SelectionIcon icon in Icons.Values)
        {
            if (icon.IsActive == false)
            {
                return false;
            }
        }
        return true;
    }

    //Coming from a selectionIcon of this expandable
    public void SelectChild(SelectionIcon icon, bool isFromSelectAll = false)
    {
        icon.IsActive = !icon.IsActive;
        icon.gameObject.SetActive(icon.IsActive);
        container.SwitchIcon(companyInfo.CompanyName, icon);

        int activeIconsCount = 0;

        foreach (SelectionIcon selectedIcon in Icons.Values)
        {
            if (selectedIcon.IsActive == true)
            {
                activeIconsCount += 1;
            }
        }

        ExpandableRect.sizeDelta = new Vector3(ExpandableRect.sizeDelta.x, 210 + (160 * activeIconsCount));

        if (this.gameObject.activeSelf == true)
        {
            StartCoroutine(UpdateCanvas());
        }

        if(isFromSelectAll == true)
        {
            return;
        }
        CheckSelectAllToggleStatus();
    }

    //Coming from another expandable
    public void SetChild(SelectionIcon selected, bool isFromSelectAll = false)
    {
        if (this.gameObject.activeSelf == true)
        {
            StartCoroutine(UpdateCanvas());
        }

        if(!Icons[selected.Info.SquadName].gameObject.activeSelf == true && this.gameObject.activeSelf == false)
        {
            this.gameObject.SetActive(true);
        }

        Icons[selected.Info.SquadName].IsActive = !selected.IsActive;
        Icons[selected.Info.SquadName].gameObject.SetActive(Icons[selected.Info.SquadName].IsActive);

        int activeIconsCount = 0;

        foreach (SelectionIcon icon in Icons.Values)
        {
            if (icon.IsActive == true )
            {
                activeIconsCount += 1;
            }
        }

        ExpandableRect.sizeDelta = new Vector3(ExpandableRect.sizeDelta.x, 210 + (160 * activeIconsCount));

        if (this.gameObject.activeSelf == true)
        {
            StartCoroutine(UpdateCanvas());
        }

        if (isFromSelectAll == true)
        {
            return;
        }
        CheckSelectAllToggleStatus();
    }

    private void CheckSelectAllToggleStatus()
    {
        if (selectAllToggle.isOn == true)
        {
            foreach (SelectionIcon icon in Icons.Values)
            {
                if (icon.IsActive == true)
                {
                    if (this.gameObject.activeSelf == false)
                    {
                        this.gameObject.SetActive(true);
                    }
                    selectAllToggle.SetIsOnWithoutNotify(false);
                    return;
                }
            }
        }

        foreach (SelectionIcon icon in Icons.Values)
        {
            if (icon.IsActive == true)
            {
                if(this.gameObject.activeSelf == false)
                {
                    this.gameObject.SetActive(true);
                }
                return;
            }
        }

        selectAllToggle.SetIsOnWithoutNotify(true);
        this.gameObject.SetActive(false);
    }

    private IEnumerator UpdateCanvas()
    {
        yield return new WaitForSeconds(0.01f);

        container.ForceUpdate();
    }
}
