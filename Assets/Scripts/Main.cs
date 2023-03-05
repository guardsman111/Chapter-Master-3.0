using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Model model = new Model();
    [SerializeField] private MainMenu mainMenu;



    private void Start()
    {
        model.Initialise();
    }

    public void OpenChapterOrganisation()
    {
        mainMenu.gameObject.SetActive(false);
        SceneManager.LoadScene("Menus", LoadSceneMode.Additive);
        mainCamera.gameObject.SetActive(false);
    }

    public void RetrieveChapterOrg(ChapterPageManager manager)
    {
        manager.Initialise(model.EquipmentModel, model.ChapterModel, model.ChapterModel.ChapterDataPublic);
    }

    public void OpenBattleSetupScreen()
    {
        //SceneManager.LoadScene("Menus", LoadSceneMode.Additive); To be created
    }

    public void OpenBattlefield(SelectionInfo info = null)
    {
        if(info == null)
        {
            Debug.LogError("Nothing selected");
            return;
        }
        model.SetSelectedInfo(info);
        mainMenu.gameObject.SetActive(false);
        //Change - needs to dynamic based on the strategy layer or the map chosen for skirmish
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Additive);
        mainCamera.gameObject.SetActive(false);
    }

    public void RetrieveSelectedInfo(UnitManager manager)
    {
        manager.Initialize(model.EquipmentModel);
        manager.SelectUnitsForBattle(model.GetSelectedInfo());
        model.SetSelectedInfo(null);
    }

    public void QuitToMenu()
    {
        mainMenu.gameObject.SetActive(true);
        mainCamera.gameObject.SetActive(true);
        SceneManager.UnloadSceneAsync("Menus");
        SceneManager.UnloadSceneAsync("SampleScene");
    }
}
