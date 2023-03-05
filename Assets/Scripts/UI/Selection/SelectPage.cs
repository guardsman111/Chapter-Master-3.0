using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectPage : MonoBehaviour
{
    [SerializeField] private ChapterPageManager manager;

    [SerializeField] private ExpandableIconContainer availableIcons;
    [SerializeField] private ExpandableIconContainer selectionIcons;

    private SelectionInfo info;

    public GameObject expandablePrefab;
    public GameObject selectablePrefab;

    public void Initialise(SelectionInfo info)
    {
        foreach(CompanyInfo company in info.companies)
        {
            availableIcons.SetIcon(expandablePrefab, company, true);
            selectionIcons.SetIcon(expandablePrefab, company, false);
        }
    }

    public void GoToBattle()
    {
        SelectionInfo selectionInfo = new SelectionInfo();

        foreach(ExpandableIcon company in selectionIcons.GetIcons().Values)
        {
            CompanyInfo companyInfo = new CompanyInfo();
            if(company.IsCountMax())
            {
                companyInfo = company.companyInfo;
                selectionInfo.companies.Add(companyInfo);
                continue;
            }

            foreach(SelectionIcon squad in company.Icons.Values)
            {
                if (squad.IsActive == true)
                {
                    companyInfo.Squads.Add(squad.Info);
                }
            }

            if(companyInfo.Squads.Count == 0)
            {
                continue;
            }

            selectionInfo.companies.Add(companyInfo);
        }

        manager.GoToBattle(info);
    }

    public void Back()
    {
        manager.LoadChapterPage();
    }

    public void SwitchIcon(ExpandableIconContainer parentContainer, string selectionParentName, SelectionIcon icon)
    {
        if(parentContainer == availableIcons)
        {
            selectionIcons.SetIcon(selectionParentName, icon);
            return;
        }

        availableIcons.SetIcon(selectionParentName, icon);
    }

    public void ForceUpdate()
    {
        availableIcons.UpdateLayout();
        selectionIcons.UpdateLayout();
    }
}
