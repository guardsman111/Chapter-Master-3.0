using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    [SerializeField] private UnitManager manager;

    [SerializeField] private Dictionary<int, SquadObject> playerUnits;
    [SerializeField] private List<SquadObject> selectedUnits;

    [SerializeField] private LayerMask mask;

    private bool reverseSelected = false;

    public void Initialize(Dictionary<int, SquadObject> PlayerUnits, UnitManager Manager)
    {
        playerUnits = PlayerUnits;
        manager = Manager;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            selectedUnits.Clear();

            RaycastHit hit;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if(Physics.Raycast(ray, out hit))
            {
                if(hit.collider.tag == "Collider")
                {
                    Debug.Log("Hit collider");
                    UnitCollider unitCollider = hit.collider.GetComponent<UnitCollider>();
                    if (unitCollider.Unit.IsPlayer)
                    {
                        selectedUnits.Add(unitCollider.Unit);
                    }
                }
                if (hit.collider.tag == "Unit")
                {
                    Debug.Log("Hit unit");
                    UnitObject unit = hit.collider.GetComponent<UnitObject>();
                    if (unit.ParentSquad.IsPlayer)
                    {
                        selectedUnits.Add(unit.ParentSquad);
                    }
                }
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit, mask))
            {
                if (hit.collider.tag == "Collider")
                {
                    if (!hit.collider.GetComponent<UnitCollider>().Unit.IsPlayer)
                    {
                        foreach(SquadObject unit in selectedUnits)
                        {
                            unit.SetTargetEnemy(hit.collider.GetComponent<UnitCollider>().Unit);
                        }
                    }
                }
                if (hit.collider.tag == "Terrain")
                {
                    foreach (SquadObject unit in selectedUnits)
                    {
                        unit.SetTargetLocation(hit.point);
                    }
                }
            }

            reverseSelected = false;
        }
    }
}
