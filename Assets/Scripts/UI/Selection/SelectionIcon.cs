using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SelectionIcon : MonoBehaviour
{
    private ExpandableIcon parent;
    public string parentName { get; private set; }
    [SerializeField] private TextMeshProUGUI iconName;
    [SerializeField] private TextMeshProUGUI iconType;
    [SerializeField] private Button selectButton;

    public SquadInfo Info { get; private set; }
    public bool IsActive;

    private void Start()
    {
        selectButton.onClick.AddListener(SelectIcon);
    }

    public void Initialise(ExpandableIcon newParent, SquadInfo newInfo)
    {
        parent = newParent;
        parentName = newParent.companyInfo.CompanyName;
        Info = newInfo;

        iconName.text = newInfo.SquadName;
        iconType.text = newInfo.SquadType.ToString();
    }

    public void SelectIcon()
    {
        parent.SelectChild(this);
    }
}
