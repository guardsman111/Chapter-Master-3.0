using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleViewManager : MonoBehaviour
{
    [SerializeField] private UnitManager unitManager;
    [SerializeField] private Camera battleCamera;
    [SerializeField] private GameObject battleMenu;

    public Main Main;



    private void Start()
    {
        Main = FindObjectOfType<Main>();
        Main.RetrieveSelectedInfo(unitManager);
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            SetBattleMenuState(!battleMenu.activeSelf);
        }
    }

    public void SetBattleMenuState(bool state)
    {
        battleMenu.SetActive(state);
    }

    public void ExitBattle()
    {
        Main.QuitToMenu();
    }
}
