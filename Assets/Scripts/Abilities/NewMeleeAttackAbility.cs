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
    public class NewMeleeAttackAbility : SelectableAbility
    {
        //public int Range;
        public int addedDamage;
        List<Unit> inRange;

        public Unit UnitToAttack { get; set; }
        public int UnitToAttackID { get; set; }
        public Cell SelectedCell { get; set; }
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

        // Update is called once per frame
        void Update()
        {
        
        }
        public override IEnumerator Act(CellGrid cellGrid, bool isNetworkInvoked = false)
        {
            Debug.Log("act called");
            if (CanPerform(cellGrid) && this.selected && UnitReference.IsUnitAttackable(UnitToAttack, UnitReference.Cell))
            {
                Debug.Log("NewMeleeAttack");
                //UnitReference.AttackHandler(UnitToAttack);
                //Debug.Log()
                UnitReference.GetComponent<SampleUnit>().NewAttackHandler(UnitToAttack.GetComponent<SampleUnit>(), "PHYSICAL", addedDamage);
                gameObject.transform.GetChild(1).GetComponent<Animator>().SetTrigger("Shoot");
                yield return new WaitForSeconds(0.5f);

                /*if (inRange == null)
                {
                    inRange = cellGrid.Cells.FindAll(c => c.GetDistance(SelectedCell) <= Range);
                }

                Unit tempUnit = null;
                foreach (var cell in inRange)
                {
                    foreach (Unit unit in new List<Unit>(cell.CurrentUnits))
                    {
                        //unit.DefendHandler(UnitReference, Damage);
                        Debug.Log("MeleeAttack");
                        UnitReference.GetComponent<SampleUnit>().NewAttackHandler(UnitToAttack.GetComponent<SampleUnit>(), "PHYSICAL");
                        //Debug.Log("heal other unit?");
                        if (unit != null)
                        {
                            tempUnit = unit;
                        }
                    }
                }

                if (tempUnit != null)
                {
                    //UnitReference.MarkAsAttacking(tempUnit);
                }*/
            }
            ActionCost();
            yield return base.Act(cellGrid, false);
            selected = false;

        }

        /*public override void OnCellSelected(Cell cell, CellGrid cellGrid)
        {
            //Debug.Log("cell selected called");
            if (cell == null || cell.CurrentUnits.Count > 0 && (cell.CurrentUnits[0] as  SampleUnit).PlayerNumber == 0 || !selected)
            {
                return;
            }

            inRange = cellGrid.Cells.FindAll(c => c.GetDistance(cell) <= Range);
            inRange.ForEach(c =>
            {
                c.MarkAsHighlighted();
                if (c.CurrentUnits.Count > 0)
                {
                    c.CurrentUnits[0].GetComponent<SampleUnit>().MarkAsReachableEnemy();
                    //c.CurrentUnits[0].MarkAsReachableEnemy();
                }
            });
        }*/
        public override void OnCellDeselected(Cell cell, CellGrid cellGrid)
        {
            if (cell == null || cell.CurrentUnits.Count > 0 && (cell.CurrentUnits[0] as SampleUnit).PlayerNumber == 0 || !selected)
            {
                return;
            }
            //CleanUp(cellGrid);
            /*if(inRange!=null){
                inRange.ForEach(c =>
                {
                    c.UnMark();
                    if (c.CurrentUnits.Count > 0)
                    {
                        if (cellGrid.GetCurrentPlayerUnits().Contains(c.CurrentUnits[0]))
                        {
                          c.CurrentUnits[0].MarkAsFriendly();
                        }
                        else
                        {
                            c.CurrentUnits[0].UnMark();
                        }
                    }
                });
                }*/
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
            inRange = enemyUnits.Where(u => UnitReference.IsUnitAttackable(u, UnitReference.Cell)).ToList();
            inRange.ForEach(u => u.MarkAsReachableEnemy());
        }

        public override void OnCellClicked(Cell cell, CellGrid cellGrid)
        {
            if (cell == null || cell.CurrentUnits== null ||cell.CurrentUnits.Count > 0 && (cell.CurrentUnits[0] as SampleUnit).PlayerNumber ==0 || !selected)
            {
                return;
            }
            /*SelectedCell = cell;

            StartCoroutine(Execute(cellGrid,
                _ => cellGrid.cellGridState = new CellGridStateBlockInput(cellGrid),
                _ => cellGrid.cellGridState = new CellGridStateWaitingForInput(cellGrid)));*/

            if (UnitReference.IsUnitAttackable(cell.CurrentUnits[0], UnitReference.Cell))
            {
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
            if(inRange != null){
            inRange.ForEach(u =>
            {
                if (u != null)
                {
                    u.UnMark();
                }
            });
            /*inRange.ForEach(c =>
            {
                c.UnMark();
                
                if (c.CurrentUnits.Count > 0)
                {
                    if (cellGrid.GetCurrentPlayerUnits().Contains(c.CurrentUnits[0]))
                    {
                        c.CurrentUnits[0].MarkAsFriendly();
                    }
                    else
                    {
                        c.CurrentUnits[0].UnMark();
                    }
                }
            });*/
            }

        }

        public override bool CanPerform(CellGrid cellGrid)
        {
            //Debug.Log("can perform");
            if (UnitReference.ActionPoints <= 0)
            {
                return false;
            }

            var enemyUnits = cellGrid.GetEnemyUnits(cellGrid.CurrentPlayer);
            inRange = enemyUnits.Where(u => UnitReference.IsUnitAttackable(u, UnitReference.Cell)).ToList();

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
            return "Special Melee Attack";
        }
    }
}
