using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UnitIcon : MonoBehaviour
{
    private DeploymentPage deploymentPage;
    private SquadInfo info;
    [SerializeField] private TextMeshProUGUI iconText;
    [SerializeField] private TextMeshProUGUI iconTypeText;

    public void SetupIcon(SquadInfo squadInfo, DeploymentPage page)
    {
        info = squadInfo;
        deploymentPage = page;
        iconText.text = info.SquadName;
        iconTypeText.text = info.SquadType.ToString();
    }

    public void IconPressed()
    {
        deploymentPage.SpawnGhost(info);
    }
}
