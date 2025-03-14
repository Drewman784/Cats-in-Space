using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TbsFramework.Cells;
using TbsFramework.Grid;
using TbsFramework.Grid.GridStates;
using UnityEngine;


namespace TbsFramework.Units.Abilities
{
    public class TestAbility : Ability
    {

        public override IEnumerator Act(CellGrid cellGrid, bool isNetworkInvoked = false)
        {
            if (CanPerform(cellGrid) )
            {
                Debug.Log("TASK PERFORMED");
            }
            yield return null;
        }
        public override bool IsSelectable(){
            return true;
        }
    }
}
