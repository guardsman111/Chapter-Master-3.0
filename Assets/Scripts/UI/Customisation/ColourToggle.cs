using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ChapterMaster.Data.Structs;

public class ColourToggle : MonoBehaviour
{
    [SerializeField] private ColourPage colourPage;
    [SerializeField] private Image colourDisplay;
    public string colourName;
    public Material linkedMaterial;

    public Toggle toggle;
    private bool selected = false;

    [SerializeField] private bool isEmissive = false;

    private void Start()
    {
        toggle = this.GetComponent<Toggle>();

        colourDisplay.color = linkedMaterial.color;

        toggle.onValueChanged.AddListener(ToggleSelected);
    }

    private void ToggleSelected(bool toggle)
    {
        colourPage.SetToggle(this);
    }

    public void ColourChanged(Color colour)
    {
        linkedMaterial.color = colour;
        colourDisplay.color = colour;
        if(isEmissive == true)
        {
            linkedMaterial.SetColor("_EmissionColor", colour);
        }
    }

    public void MetallicChanged(float value)
    {
        linkedMaterial.SetFloat("_Metallic", value);
    }

    public void SmoothnessChanged(float value)
    {
        linkedMaterial.SetFloat("_Smoothness", value);
    }

    public MaterialDef ReturnColour()
    {
        MaterialDef def = new MaterialDef();
        def.colour = linkedMaterial.color;
        def.materialName = colourName;
        def.metallic = linkedMaterial.GetFloat("_Metallic");
        def.smoothness = linkedMaterial.GetFloat("_Smoothness");

        return def;
    }
}
