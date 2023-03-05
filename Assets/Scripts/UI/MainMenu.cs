using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
    private Main main;
    [SerializeField] private Button organisationButton;
    [SerializeField] private Button battleButton;

    private void Start()
    {
        main = FindObjectOfType<Main>();

        organisationButton.onClick.AddListener(OpenOrganisationPage);
        battleButton.onClick.AddListener(OpenBattlePage);
    }

    private void OpenOrganisationPage()
    {
        main.OpenChapterOrganisation();
    }

    private void OpenBattlePage()
    {
        //Change - this should go to battle setup when it's done
        main.OpenBattlefield();
    }
}
