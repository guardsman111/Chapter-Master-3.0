using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Main : MonoBehaviour
{
    [SerializeField] private Model model = new Model();
    [SerializeField] private MainMenu mainMenu;

    public void OpenChapterOrganisation()
    {
        mainMenu.gameObject.SetActive(false);
        SceneManager.LoadScene("Menus", LoadSceneMode.Additive);
        FindObjectOfType<ChapterPageManager>().Initialise(model.EquipmentModel, model.ChapterModel, model.ChapterModel.ChapterDataPublic);
    }

    public void OpenBattleSetupScreen()
    {
        //SceneManager.LoadScene("Menus", LoadSceneMode.Additive); To be created
    }

    public void OpenBattlefield()
    {
        mainMenu.gameObject.SetActive(false);
        //Change - needs to dynamic based on the strategy layer or the map chosen for skirmish
        SceneManager.LoadScene("SampleScene", LoadSceneMode.Additive);
    }

    public void QuitToMenu()
    {
        mainMenu.gameObject.SetActive(true);
        SceneManager.UnloadSceneAsync("Menus");
        SceneManager.UnloadSceneAsync("SampleScene");
    }
}
