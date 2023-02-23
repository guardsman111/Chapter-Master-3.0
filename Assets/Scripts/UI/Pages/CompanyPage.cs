using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CompanyPage : MonoBehaviour
{
    [SerializeField] private ChapterPageManager manager;
    [SerializeField] private RectTransform squadParent;
    [SerializeField] private GameObject squadBoxPrefab;

    [SerializeField] private TMP_InputField companyName;
    [SerializeField] private TMP_InputField companyNickname;

    [SerializeField] private Dictionary<int, SquadBox> squadBoxes;

    private CompanyModel companyModel;

    private void Start()
    {
        companyName.onValueChanged.AddListener(SetNewName);
        companyNickname.onValueChanged.AddListener(SetNewNickname);

        gameObject.SetActive(false);
    }

    private void SetNewName(string newName)
    {
        companyModel.CompanyData.CompanyName = newName;
    }

    private void SetNewNickname(string newName)
    {
        companyModel.CompanyData.CompanyNickname = newName;
    }

    public void Load(CompanyModel company)
    {
        companyModel = company;
        companyName.text = company.CompanyData.CompanyName;
        companyNickname.text = company.CompanyData.CompanyNickname;

        squadBoxes = new Dictionary<int, SquadBox>();

        foreach (SquadModel squad in company.SquadDataDictionary.Values)
        {
            SquadBox squadBox = Instantiate(squadBoxPrefab, squadParent).GetComponent<SquadBox>();
            squadBox.LoadSquad(squad, manager);
            squadBoxes.Add(squad.SquadData.SquadID, squadBox);
        }

        squadParent.sizeDelta = new Vector2((425 * squadBoxes.Count) + 25, 0);
    }

    public void Clear()
    {
        foreach(SquadBox box in squadBoxes.Values)
        {
            Destroy(box.gameObject);
        }

        squadBoxes.Clear();
    }

    public void Back()
    {
        manager.LoadChapterPage();
    }
}
