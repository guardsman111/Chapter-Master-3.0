using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using static ChapterMaster.Data.Enums;

public class SquadPage : MonoBehaviour
{
    [SerializeField] private ChapterPageManager manager;
    [SerializeField] private RectTransform soldierParent;
    [SerializeField] private GameObject soldierBoxPrefab;

    [SerializeField] private TMP_InputField squadName;
    [SerializeField] private TMP_Dropdown squadType;

    [SerializeField] private Dictionary<int, SoldierBox> soldierBoxes;

    SquadModel squadModel;

    private void Start()
    {
        squadName.onValueChanged.AddListener(SetNewName);

        string[] typeNames = Enum.GetNames(typeof(SquadType));
        List<TMP_Dropdown.OptionData> options = new List<TMP_Dropdown.OptionData>();

        foreach (string type in typeNames)
        {
            TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData(type);
            Debug.Log(type);
        }

        squadType.ClearOptions();
        squadType.options = options;

        gameObject.SetActive(false);
    }

    private void SetNewName(string newName)
    {
        squadModel.SquadData.SquadName = newName;
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

        soldierParent.sizeDelta = new Vector2(425 * soldierBoxes.Count, 0);
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
}
