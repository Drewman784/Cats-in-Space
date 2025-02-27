using System.Collections.Generic;
using TbsFramework.Cells;
using TbsFramework.Grid;

namespace TbsFramework.Units.Abilities
{
    public class AttackRangeHighlightAbility : Ability
    {
        List<Unit> inRange;

        public override void OnCellSelected(Cell cell, CellGrid cellGrid)
        {
            var availableDestinations = UnitReference.GetComponent<MoveAbility>().availableDestinations;
            if (!availableDestinations.Contains(cell))
            {
                return;
            }

            var enemyUnits = cellGrid.GetEnemyUnits(cellGrid.CurrentPlayer);
            inRange = enemyUnits.FindAll(u => UnitReference.IsUnitAttackable(u, cell));

            inRange.ForEach(u => u.MarkAsReachableEnemy());
        }

        public override void OnCellDeselected(Cell cell, CellGrid cellGrid)
        {
            var enemyUnits = cellGrid.GetEnemyUnits(cellGrid.CurrentPlayer);
            inRange = enemyUnits.FindAll(u => UnitReference.IsUnitAttackable(u, cell));

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
    }
}