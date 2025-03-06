using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TbsFramework.Cells;
using TbsFramework.Grid;
using TbsFramework.Grid.GridStates;
using UnityEngine;

//most of this is taken directly from the AttackAbility script
namespace TbsFramework.Units.Abilities
{
    public class MeleeAttackAbility : AttackAbility
    {
        public override IEnumerator Act(CellGrid cellGrid, bool isNetworkInvoked = false)
        {
            if (CanPerform(cellGrid) && UnitReference.IsUnitAttackable(UnitToAttack, UnitReference.Cell))
            {
                // Cal Edit
                UnitReference.GetComponent<SampleUnit>().NewAttackHandler(UnitToAttack.GetComponent<SampleUnit>(), "PHYSICAL");
                //UnitReference.AttackHandler(UnitToAttack);
                yield return new WaitForSeconds(0.5f);
            }
            yield return null;
        }
    }
}
