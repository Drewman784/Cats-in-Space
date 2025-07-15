using UnityEngine;
using TbsFramework.Grid;
using TbsFramework.Cells;
using TbsFramework.Grid.GridStates;
using TbsFramework.Units;
using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;

namespace TbsFramework.Units.Abilities
{
    //this ability should heal allow the unit to attack an adjacent unit
    //this script heavily references FireballSpell 
    public class ParalysisAbility : SelectableAbility
    {
        //public int Range;
        public int addedDamage;
        List<Unit> inRange;

        public Unit UnitToAttack { get; set; }
        public int UnitToAttackID { get; set; }
        public Cell SelectedCell { get; set; }

        Color canParalyzeColor = new Color(.245f, 0.161f, .066f); //245, 161, 66
                                                                  // Start is called once before the first execution of Update after the MonoBehaviour is created
        [SerializeField] Buff ParalysisBuff;

        public override IEnumerator Act(CellGrid cellGrid, bool isNetworkInvoked = false)
        {
            Debug.Log("act called");
            if (CanPerform(cellGrid) && this.selected && UnitReference.IsUnitAttackable(UnitToAttack, UnitReference.Cell))
            {
                Debug.Log("Paralyze!");
                //UnitReference.AttackHandler(UnitToAttack);
                //Debug.Log()
                //UnitReference.GetComponent<SampleUnit>().NewAttackHandler(UnitToAttack.GetComponent<SampleUnit>(), "PHYSICAL", addedDamage);
                //gameObject.transform.GetChild(1).GetComponent<Animator>().SetTrigger("Shoot");

                //UnitReference.GetComponent<SampleUnit>().NewAttackHandler(UnitToAttack.GetComponent<SampleUnit>(), "PSIONIC", addedDamage);
                UnitToAttack.GetComponent<SampleUnit>().AddBuff(ParalysisBuff);
                yield return new WaitForSeconds(0.5f);
                ActionCost();

            }

            yield return base.Act(cellGrid, false);
            selected = false;
            //SampleUnit.

        }


        public override void OnCellDeselected(Cell cell, CellGrid cellGrid)
        {
            if (cell == null || cell.CurrentUnits.Count > 0 && (cell.CurrentUnits[0] as SampleUnit).PlayerNumber == 0 || !selected)
            {

                //Debug.Log("deselected!");
                return;
            }

        }

        public override void OnUnitHighlighted(Unit unit, CellGrid cellGrid)
        {
            //Debug.Log("unit highlighted called");
            //OnCellSelected(unit.Cell, cellGrid);
        }

        public override void OnUnitDehighlighted(Unit unit, CellGrid cellGrid)
        {
            //Debug.Log("unit deselected called");
            //OnCellDeselected(unit.Cell, cellGrid);
        }

        public override void Display(CellGrid cellGrid)
        {
            var enemyUnits = cellGrid.GetEnemyUnits(cellGrid.CurrentPlayer);
            CleanUp(cellGrid);
 
            inRange = enemyUnits.Where(u => UnitReference.IsUnitAttackable(u, UnitReference.Cell)).ToList();

            inRange.ForEach(u => u.Cell.MarkAsDestination(canParalyzeColor));

        }

        public override void OnCellClicked(Cell cell, CellGrid cellGrid)
        {
            if ((cell == null) || (cell.CurrentUnits == null) || (cell.CurrentUnits.Count > 0 && (cell.CurrentUnits[0] as SampleUnit).PlayerNumber == 0) || (!selected) || (cell.CurrentUnits.Count == 0))
            {
                return;
            }
            /*SelectedCell = cell;

            StartCoroutine(Execute(cellGrid,
                _ => cellGrid.cellGridState = new CellGridStateBlockInput(cellGrid),
                _ => cellGrid.cellGridState = new CellGridStateWaitingForInput(cellGrid)));*/

            if (UnitReference.IsUnitAttackable(cell.CurrentUnits[0], UnitReference.Cell))
            {
                SampleUnit su = (SampleUnit)cell.CurrentUnits[0];
                UnitToAttack = cell.CurrentUnits[0];
                UnitToAttackID = UnitToAttack.UnitID;
                StartCoroutine(HumanExecute(cellGrid));

            }
        }

        public override void OnUnitClicked(Unit unit, CellGrid cellGrid)
        {
            OnCellClicked(unit.Cell, cellGrid);
        }

        public override void OnTurnStart(CellGrid cellGrid)
        {
            inRange = null;
            SelectedCell = null;
        }

        public override void CleanUp(CellGrid cellGrid)
        {
            base.CleanUp(cellGrid);
            //OnCellDeselected(null, cellGrid);
            if (inRange != null)
            {
                inRange.ForEach(u =>
                {
                    if (u != null)
                    {
                        u.UnMark();
                    }
                });
            }

        }

        public override bool CanPerform(CellGrid cellGrid)
        {
            //Debug.Log("can perform");
            if (UnitReference.ActionPoints <= 0)
            {
                selected = false;
                return false;
            }

            var enemyUnits = cellGrid.GetEnemyUnits(cellGrid.CurrentPlayer);
            inRange = enemyUnits.Where(u => UnitReference.IsUnitAttackable(u, UnitReference.Cell)).ToList();

            Debug.Log(inRange.Count + " in range");

            return inRange.Count > 0;
        }
        public override IEnumerator Apply(CellGrid cellGrid, IDictionary<string, string> actionParams, bool isNetworkInvoked)
        {
            SelectedCell = cellGrid.Cells.Find(c => c.OffsetCoord.Equals(new UnityEngine.Vector2(float.Parse(actionParams["selected_cell_x"]), float.Parse(actionParams["selected_cell_y"]))));
            yield return StartCoroutine(RemoteExecute(cellGrid));
        }

        public override IDictionary<string, string> Encapsulate()
        {
            var actionParams = new Dictionary<string, string>();

            //actionParams.Add("selected_cell_x", SelectedCell.OffsetCoord.x.ToString());
            //actionParams.Add("selected_cell_y", SelectedCell.OffsetCoord.y.ToString());

            Dictionary<string, string> actionParameters = new Dictionary<string, string>();
            actionParameters.Add("target_id", UnitToAttackID.ToString());

            return actionParams;
        }

        public override string GetAbilityName()
        {
            return "Paralyze";
        }
        
        public override string GetAbilityDescription()
        {
            return "Paralyze an opposing unit for 1 turn";
        }
    }
}
