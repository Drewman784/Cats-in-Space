using TbsFramework.Grid;
using TbsFramework.Grid.GridStates;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;
using TMPro;
using UnityEngine;

public class AbilityButtonScript : MonoBehaviour
{
    private Ability theAbility;
    CellGrid cG;
    Unit unit;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        cG = GameObject.Find("CellGrid").GetComponent<CellGrid>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CallAbility(){
        cG.cellGridState = new CellGridStateAbilitySelected(cG, unit, theAbility);
    }

    public void SetAbility(Ability toSet, Unit tUnit){
        theAbility = toSet;
        unit = tUnit;
    }
}
