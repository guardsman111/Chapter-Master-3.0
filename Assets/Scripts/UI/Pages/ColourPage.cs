using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static ChapterMaster.Data.Enums;
using static ChapterMaster.Data.Structs;

public class ColourPage : MonoBehaviour
{
    [SerializeField] private ChapterPageManager manager;
    [SerializeField] private UnitObject soldierDisplay;

    [SerializeField] private Dropdown armourPatternDropdown;
    [SerializeField] private List<ColourToggle> toggles;
    private Dictionary<string, ColourToggle> toggleDict = new Dictionary<string, ColourToggle>();
    [SerializeField] private ColourToggle selectedToggle;
    [SerializeField] private Image colourPreview;

    [SerializeField] private Slider r, g, b, m, s;
    [SerializeField] private TextMeshProUGUI red, green, blue, metal, smooth;

    [SerializeField] private List<string> patternNames;

    private void Start()
    {
        r.onValueChanged.AddListener(SliderRValueChanged);
        g.onValueChanged.AddListener(SliderGValueChanged);
        b.onValueChanged.AddListener(SliderBValueChanged);
        m.onValueChanged.AddListener(SliderMValueChanged);
        s.onValueChanged.AddListener(SliderSValueChanged);
        armourPatternDropdown.onValueChanged.AddListener(ArmourPatternChanged);

        armourPatternDropdown.ClearOptions();
        armourPatternDropdown.AddOptions(patternNames);

        foreach(ColourToggle toggle in toggles)
        {
            toggleDict.Add(toggle.colourName, toggle);
        }
    }

    public void Initialize(string patternName)
    {
        soldierDisplay.colourer.gameObject.SetActive(true);
        soldierDisplay.colourer.SetArmourPattern(patternName);
    }

    public void SetToggle(ColourToggle toggle)
    {
        if (selectedToggle != null)
        {
            selectedToggle.toggle.SetIsOnWithoutNotify(false);

            if (selectedToggle == toggle)
            {
                selectedToggle = null;
                return;
            }
        }

        if (toggleDict.ContainsValue(toggle))
        {
            selectedToggle = toggle;
            r.SetValueWithoutNotify(toggle.linkedMaterial.color.r);
            g.SetValueWithoutNotify(toggle.linkedMaterial.color.g);
            b.SetValueWithoutNotify(toggle.linkedMaterial.color.b);
            m.SetValueWithoutNotify(toggle.linkedMaterial.GetFloat("_Metallic"));
            s.SetValueWithoutNotify(toggle.linkedMaterial.GetFloat("_Smoothness"));
            colourPreview.color = toggle.linkedMaterial.color;
        }
    }

    private void SliderRValueChanged(float value)
    {
        if (selectedToggle != null)
        {
            Color colour = new Color(value, g.value, b.value);
            selectedToggle.ColourChanged(colour);
            colourPreview.color = colour;
        }
    }

    private void SliderGValueChanged(float value)
    {
        if (selectedToggle != null)
        {
            Color colour = new Color(r.value, value, b.value);
            selectedToggle.ColourChanged(colour);
            colourPreview.color = colour;
        }
    }

    private void SliderBValueChanged(float value)
    {
        if (selectedToggle != null)
        {
            Color colour = new Color(r.value, g.value, value);
            selectedToggle.ColourChanged(colour);
            colourPreview.color = colour;
        }
    }

    private void SliderMValueChanged(float value)
    {
        if (selectedToggle != null)
        {
            selectedToggle.MetallicChanged(value);
        }
    }

    private void SliderSValueChanged(float value)
    {
        if (selectedToggle != null)
        {
            selectedToggle.SmoothnessChanged(value);
        }
    }

    private void ArmourPatternChanged(int value)
    {
        soldierDisplay.colourer.SetArmourPattern(patternNames[value]);
    }

    public void Back()
    {
        List<MaterialDef> defs = new List<MaterialDef>();
        foreach(ColourToggle toggle in toggleDict.Values)
        {
            defs.Add(toggle.ReturnColour());
        }

        manager.ColourBack(patternNames[armourPatternDropdown.value], defs);
        soldierDisplay.colourer.gameObject.SetActive(false);
    }
}
