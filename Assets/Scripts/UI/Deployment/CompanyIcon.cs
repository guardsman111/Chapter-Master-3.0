using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CompanyIcon : MonoBehaviour
{
    private CompanyInfo info;
    public CompanyInfo Info => info;
    private DeploymentPage deploymentPage;
    [SerializeField] private TextMeshProUGUI iconText;
    [SerializeField] private TextMeshProUGUI iconNicknameText;

    public void SetupIcon(CompanyInfo companyInfo, DeploymentPage page)
    {
        info = companyInfo;
        deploymentPage = page;
        iconText.text = info.CompanyName;
        iconNicknameText.text = info.CompanyNickname;
    }

    public void IconPressed()
    {
        if(deploymentPage.activeCompany == this)
        {
            deploymentPage.ChangeCompany(null);
        }

        deploymentPage.ChangeCompany(this);
    }
}
