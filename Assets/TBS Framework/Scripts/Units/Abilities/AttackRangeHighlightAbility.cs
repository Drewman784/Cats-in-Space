using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Mono.Cecil.Cil;
using TbsFramework.Cells;
using TbsFramework.Grid;
using UnityEngine;

namespace TbsFramework.Units.Abilities
{
    public class AttackRangeHighlightAbility : Ability
    {
        List<Unit> inRange;

        public override void OnCellSelected(Cell cell, CellGrid cellGrid)
        {
            inRange = new List<Unit>();
            var availableDestinations = UnitReference.GetComponent<MoveAbility>().availableDestinations;
            if (!availableDestinations.Contains(cell))
            {
                return;
            }

            var enemyUnits = cellGrid.GetEnemyUnits(cellGrid.CurrentPlayer);
            //inRange = enemyUnits.FindAll(u => UnitReference.IsUnitAttackable(u, cell));
            foreach (Cell c in availableDestinations){ //check each cell in walking range
                List<Unit> cellRange = enemyUnits.FindAll(u => UnitReference.IsUnitAttackable(u, c)); //find all attackable units from cell
                List<Unit> toAdd = new List<Unit>();
                //inRange = enemyUnits.FindAll(u => UnitReference.IsUnitAttackable(u, c));
                if(cellRange !=null && inRange != null){ //check that unit isn't already added to inRange
                    //UnityEngine.Debug.Log("cellrange count="+cellRange.Count);
                    foreach (Unit ce in cellRange){
                        if(!inRange.Contains(ce)){
                           toAdd.Add(ce);
                        }
                        //if(cellRange == null){break;}
                    }
                }
                if(toAdd!=null){ 
                    inRange.AddRange(toAdd);
                }
            }
            //UnityEngine.Debug.Log("in range: "+inRange);
            inRange.ForEach(u => u.MarkAsReachableEnemy()); //highlight
        }

        public override void OnCellDeselected(Cell cell, CellGrid cellGrid)
        {
            var enemyUnits = cellGrid.GetEnemyUnits(cellGrid.CurrentPlayer);
            //inRange = enemyUnits.FindAll(u => UnitReference.IsUnitAttackable(u, cell));

            inRange?.ForEach(u => u.UnMark());
            var inRangeLocal = enemyUnits.FindAll(u => UnitReference.IsUnitAttackable(u, UnitReference.Cell));

            inRangeLocal.ForEach(u => u.MarkAsReachableEnemy());
        }

        public override void OnAbilityDeselected(CellGrid cellGrid)
        {
            VerifyRange();
            inRange?.ForEach(u => u.UnMark());
        }

        public override void OnTurnEnd(CellGrid cellGrid)
        {
            inRange = null;
        }


        public void VerifyRange(){
            if(inRange !=null){
                while(inRange.Contains(null))
                    inRange.Remove(null);
            }
        }

         
        /*public override void OnAbilitySelected(CellGrid cellGrid)
        {
            base.OnAbilitySelected(cellGrid);
            var availableDestinations = UnitReference.GetComponent<MoveAbility>().availableDestinations;
            if (!availableDestinations.Contains(GetComponent<Unit>().Cell))
            {
                return;
            }

            var enemyUnits = cellGrid.GetEnemyUnits(cellGrid.CurrentPlayer);
            //inRange = enemyUnits.FindAll(u => UnitReference.IsUnitAttackable(u, cell));
            foreach (Cell c in availableDestinations){ //check each cell in walking range
                List<Unit> cellRange = enemyUnits.FindAll(u => UnitReference.IsUnitAttackable(u, c)); //find all attackable units from cell
                //inRange = enemyUnits.FindAll(u => UnitReference.IsUnitAttackable(u, c));
                if(cellRange.Count>0 && inRange.Count>0){ //check that unit isn't already added to inRange
                    UnityEngine.Debug.Log("cellrange count="+cellRange.Count);
                    foreach (Unit ce in cellRange){
                        if(inRange.Contains(ce)){
                            cellRange.Remove(ce);
                        }
                        if(cellRange.Count==0){break;}
                    }
                }
                if(cellRange.Count>0){ 
                    inRange.AddRange(cellRange);
                }
            }
            UnityEngine.Debug.Log("in range: "+inRange);
            inRange.ForEach(u => u.MarkAsReachableEnemy()); //highlight
        }*/
    }
}