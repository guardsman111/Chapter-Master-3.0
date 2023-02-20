using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CompanyBox : MonoBehaviour
{
    [SerializeField] private TMP_InputField companyName;
    [SerializeField] private TMP_InputField companyNickname;
    [SerializeField] private TextMeshProUGUI companySquadInfo;

    private CompanyModel companyModel;

    public void LoadCompany(CompanyModel company)
    {
        companyModel = company; 
        companyName.text = company.CompanyData.CompanyName;
        companyName.onValueChanged.AddListener(SetNewName);
        companyNickname.onValueChanged.AddListener(SetNewNickname);
    }

    private void SetNewName(string newName)
    {
        Debug.Log($"Set {companyName.text} to {newName}");
        companyModel.CompanyData.CompanyName = newName;
    }

    private void SetNewNickname(string newName)
    {
        Debug.Log($"Set {companyNickname.text} to {newName}");
        companyModel.CompanyData.CompanyNickname = newName;
    }
}
