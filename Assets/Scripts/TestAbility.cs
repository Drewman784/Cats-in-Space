using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TbsFramework.Cells;
using TbsFramework.Grid;
using TbsFramework.Grid.GridStates;
using UnityEngine;


namespace TbsFramework.Units.Abilities
{
    public class TestAbility : SelectableAbility
    {

        public override IEnumerator Act(CellGrid cellGrid, bool isNetworkInvoked = false)
        {
            if (CanPerform(cellGrid) )
            {
                Debug.Log("TASK PERFORMED");
                yield return new WaitForSeconds(0.5f);
            } else{
                Debug.LogError("task failed!");
            }
            yield return null;
        }

        /*public override bool IsSelectable(){
            return true;
        }*/

        public override string GetAbilityName()
        {
            return "TestAbility!";
        }

        public override IEnumerator Apply(CellGrid cellGrid, IDictionary<string, string> actionParams, bool isNetworkInvoked = false)
        {
            Debug.Log("hit here");
            yield return StartCoroutine(RemoteExecute(cellGrid));
        }
        /*public override void OnUnitClicked(Unit unit, CellGrid cellGrid)
        {
            if (cellGrid.GetCurrentPlayerUnits().Contains(unit))
            {
                cellGrid.cellGridState = new CellGridStateAbilitySelected(cellGrid, unit, unit.GetComponents<Ability>().ToList());
            }
        }*/

        
    }
}
