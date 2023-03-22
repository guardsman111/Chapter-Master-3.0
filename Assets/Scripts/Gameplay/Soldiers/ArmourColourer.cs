using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static ChapterMaster.Data.Structs;

public class ArmourColourer : MonoBehaviour
{
    private Dictionary<string, ArmourPattern> patternDict = new Dictionary<string, ArmourPattern>();
    [SerializeField] private List<ArmourPattern> patterns;

    [SerializeField] private Material primary;
    [SerializeField] private Material secondary;

    public void SetArmourPattern(string value)
    {
        if(patternDict.Count == 0)
        {
            foreach (ArmourPattern pattern in patterns)
            {
                patternDict.Add(pattern.patternName, pattern);
            }
        }

        if(patternDict.ContainsKey(value))
        {
            if (patternDict[value].primaryMeshes != null)
            {
                foreach (SkinnedMeshRenderer mesh in patternDict[value].primaryMeshes)
                {
                    mesh.material = primary;
                }
            }

            if (patternDict[value].secondaryMeshes != null)
            {
                foreach (SkinnedMeshRenderer mesh in patternDict[value].secondaryMeshes)
                {
                    mesh.material = secondary;
                }
            }
        }
    }
}
