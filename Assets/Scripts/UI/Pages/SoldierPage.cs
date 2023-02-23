using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoldierPage : MonoBehaviour
{
    [SerializeField] private ChapterPageManager manager;
    [SerializeField] private RectTransform soldierParent;
    [SerializeField] private GameObject soldierBoxPrefab;

    [SerializeField] private TMP_InputField soldierName;

    SoldierModel soldierModel;

    public void Load(SoldierModel soldier)
    {
        soldierModel = soldier;
        soldierName.text = soldier.SoldierData.firstName + " " + soldier.SoldierData.secondName;
    }

    public void Clear()
    {
        //Empty
    }

    public void Back()
    {
        manager.BackToSquadPage();
    }
}
