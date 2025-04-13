using System.Collections.Generic;
using TbsFramework.Grid;
using TbsFramework.Grid.GridStates;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;
using TMPro;
using UnityEngine;

public class AbilityButtonScript : MonoBehaviour
{
    private SelectableAbility theAbility;
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
        //cG.cellGridState = new CellGridStateAbilitySelected(cG, unit, theAbility);
        //theAbility.DoAction(cG);
        //Debug.Log("button click!");
        if(theAbility.IsInstant()){
            theAbility.DoAction(cG);
            //Debug.Log("instant! " + theAbility);
        } else{
            //Debug.Log("noninstant");
            cG.cellGridState = new CellGridStateAbilitySelected(cG, unit, new List<Ability>() { theAbility });
            theAbility.GetComponent<SelectableAbility>().selected = true;
        }
        unit.GetComponent<AttackAbility>().enabled  = false;

    }

    public void SetAbility(SelectableAbility toSet, Unit tUnit){
        theAbility = toSet;
        unit = tUnit;
    }
}
