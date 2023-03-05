using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeployableUnitsPage : PageView
{
    [SerializeField] private DeploymentPage page;
    [SerializeField] private Dictionary<SquadInfo, UnitIcon> unitIconList = new Dictionary<SquadInfo, UnitIcon>();
    [SerializeField] private List<SquadInfo> deployedSquads = new List<SquadInfo>();
    [SerializeField] private Transform unitContent;
    [SerializeField] private RectTransform unitContentRect;

    [SerializeField] private GameObject unitIconPrefab;
    [SerializeField] private GameObject unitPrefab;


    public void SetActiveCompany(CompanyIcon newCompany)
    {
        Clear();

        foreach(SquadInfo info in newCompany.Info.Squads)
        {
            if(deployedSquads.Contains(info) || unitIconList.ContainsKey(info))
            {
                continue;
            }
            UnitIcon icon = Instantiate(unitIconPrefab, unitContent).GetComponent<UnitIcon>();
            icon.SetupIcon(info, page);
            unitIconList.Add(info, icon);
        }

        unitContentRect = unitContent.GetComponent<RectTransform>();

        unitContentRect.sizeDelta = new Vector2(210 * unitIconList.Count, unitContent.GetComponent<RectTransform>().sizeDelta.y);
    }

    public void RemoveUsedUnit(SquadInfo info)
    {
        Destroy(unitIconList[info].gameObject);
        unitIconList.Remove(info);
        deployedSquads.Add(info);

        unitContentRect.sizeDelta = new Vector2(210 * unitIconList.Count, unitContent.GetComponent<RectTransform>().sizeDelta.y);
    }

    public void Clear()
    {
        foreach (UnitIcon icon in unitIconList.Values)
        {
            Destroy(icon.gameObject);
        }

        unitIconList.Clear();
    }
}
