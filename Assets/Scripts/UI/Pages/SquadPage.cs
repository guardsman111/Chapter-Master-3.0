using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static ChapterMaster.Data.Enums;

public class SquadPage : MonoBehaviour
{
    [SerializeField] private ChapterPageManager manager;
    [SerializeField] private RectTransform soldierParent;
    [SerializeField] private GameObject soldierBoxPrefab;

    [SerializeField] private TMP_InputField squadName;
    [SerializeField] private Dropdown squadType;

    [SerializeField] private Dictionary<int, SoldierBox> soldierBoxes;

    SquadModel squadModel;

    public void Initialise()
    {
        squadName.onValueChanged.AddListener(SetNewName);
        squadType.onValueChanged.AddListener(SetNewDesignation);

        string[] typeNames = Enum.GetNames(typeof(SquadType));

        foreach (string type in typeNames)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData(type);
        }

        squadType.ClearOptions();
        squadType.AddOptions(typeNames.ToList());

        gameObject.SetActive(false);
    }

    private void SetNewName(string newName)
    {
        squadModel.SquadData.SquadName = newName;
    }

    private void SetNewDesignation(int newDesignation)
    {
        squadModel.SquadData.SquadType = (SquadType)newDesignation;
    }

    public void Load()
    {
        Load(squadModel);
    }

    public void Load(SquadModel squad)
    {
        squadModel = squad;
        squadName.text = squad.SquadData.SquadName;
        squadType.value = (int)squad.SquadData.SquadType;

        soldierBoxes = new Dictionary<int, SoldierBox>();

        foreach (SoldierModel soldier in squad.SoldierDataDictionary.Values)
        {
            SoldierBox soldierBox = Instantiate(soldierBoxPrefab, soldierParent).GetComponent<SoldierBox>();
            soldierBox.LoadSoldier(soldier, manager);
            soldierBoxes.Add(soldier.SoldierData.soldierID, soldierBox);
        }

        soldierParent.sizeDelta = new Vector2((425 * soldierBoxes.Count) + 25, 0);
    }

    public void Clear()
    {
        foreach (SoldierBox box in soldierBoxes.Values)
        {
            Destroy(box.gameObject);
        }

        soldierBoxes.Clear();
    }

    public void Back()
    {
        manager.BackToCompanyPage();
    }

    public void ReloadSoldier(SoldierBox soldier)
    {
        if(soldierBoxes.ContainsValue(soldier))
        {
            soldier.ReloadSoldier(soldier);
        }
    }
}
