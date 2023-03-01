using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class GhostUnit : MonoBehaviour
{
    [SerializeField] private EquipmentModel equipmentModel;
    [SerializeField] private GameObject unit;
    public GameObject UnitPrefab => unit;

    [SerializeField] private SquadInfo info;
    [SerializeField] private Material ghostMat;

    public SquadInfo Info => info;

    public void SetGhost(SquadInfo newInfo, EquipmentModel equip)
    {
        equipmentModel = equip;
        info = newInfo;
        int count = 1;

        foreach(SoldierInfo soldier in info.Soldiers)
        {
            GameObject soldierArmour = equip.armours[soldier.armour].modelObject;
            GameObject GO = Instantiate(soldierArmour, this.transform);
            GO.transform.position = CirclePosition(transform.position, 3.0f, count);
            foreach (Renderer renderer in GO.GetComponentsInChildren<Renderer>()) 
            {
                renderer.material = ghostMat;
            }
            count++;
        }
    }

    Vector3 CirclePosition(Vector3 center, float radius, int count)
    {
        float ang = (360 / info.Soldiers.Count) * count;
        Vector3 pos;
        pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
        pos.y = center.y;
        pos.z = center.z + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
        return pos;
    }
}
