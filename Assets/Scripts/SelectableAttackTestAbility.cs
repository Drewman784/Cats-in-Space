using UnityEngine;
using TbsFramework.Cells;
using TbsFramework.Grid;
using TbsFramework.Grid.GridStates;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;

namespace TbsFramework.Units.Abilities
{
    public class SelectableAttackTestAbility : AttackAbility
    {
        private bool available;
        private int origAttackRange;
        private AttackAbility standardAttack;
        List<Unit> inAttackRange;

        void Start()
        {
          available = false;  
        }

        public override IEnumerator Act(CellGrid cellGrid, bool isNetworkInvoked = false)
        {
            if (CanPerform(cellGrid) && UnitReference.IsUnitAttackable(UnitToAttack, UnitReference.Cell) && available)
            {
                // Cal Edit
                Debug.Log("Special Attack");
                UnitReference.GetComponent<SampleUnit>().NewAttackHandler(UnitToAttack.GetComponent<SampleUnit>(), "MELEE");
                //UnitReference.AttackHandler(UnitToAttack);
                Reset();
                yield return new WaitForSeconds(0.5f);
            }
            yield return null;
        }

        public override void Display(CellGrid cellGrid)
        {
            var enemyUnits = cellGrid.GetEnemyUnits(cellGrid.CurrentPlayer);
            inAttackRange = enemyUnits.Where(u => UnitReference.IsUnitAttackable(u, UnitReference.Cell)).ToList();
            inAttackRange.ForEach(u => u.MarkAsReachableEnemy());
        }

        public override bool IsSelectable()
        {
            return true;
        }

        public override string GetAbilityName(){
            //return AbilityName;
            return "Special Attack";
        }

        public override bool CanPerform(CellGrid cellGrid)
        {
            //Debug.Log("can perform");
            if (UnitReference.ActionPoints <= 0)
            {
                return false;
            } else if(!available){
                return false;
            }

            var enemyUnits = cellGrid.GetEnemyUnits(cellGrid.CurrentPlayer);
            inAttackRange = enemyUnits.Where(u => UnitReference.IsUnitAttackable(u, UnitReference.Cell)).ToList();

            return inAttackRange.Count > 0;
        }

        public override void DoAction(CellGrid cellGrid){
            Debug.Log("CALLED");
            available = true;
            origAttackRange = GetComponent<SampleUnit>().AttackRange;
            GetComponent<SampleUnit>().AttackRange = 3;

            StartCoroutine(HumanExecute(cellGrid));
        }

        public override void OnAbilityDeselected(CellGrid cellGrid)
        {
            Reset();
            base.OnAbilityDeselected(cellGrid);
        }

        public override void OnCellDeselected(Cell cell, CellGrid cellGrid)
        {
            Reset();
            base.OnCellDeselected(cell, cellGrid);
        }

        private void Reset()
        {
            if(available){
                available = false;
                GetComponent<SampleUnit>().AttackRange = origAttackRange;
            }

        }
    }
}
