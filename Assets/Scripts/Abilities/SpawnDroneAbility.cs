using UnityEngine;
using TbsFramework.Grid;
using TbsFramework.Cells;
using TbsFramework.Grid.GridStates;
using TbsFramework.Units;
using System.Collections;
using System.Collections.Generic;
using System;

namespace TbsFramework.Units.Abilities
{
    //this ability should heal allow the unit to spawn a drone unit in an adjacent space
    //this script heavily references FireballSpell 
    public class SpawnDroneAbility : SelectableAbility
    {
        public int Range;
        public int Damage;
        List<Cell> inRange;
        public Cell SelectedCell { get; set; }
        [SerializeField] private SampleUnit UnitToSpawn;

        Color okLocationColor = new Color(.52f, 0.201f, .235f);
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
            if (CanPerform(cellGrid) && this.selected)
            {
                if (inRange == null)
                {
                    inRange = cellGrid.Cells.FindAll(c => c.GetDistance(SelectedCell) <= Range);
                }

                SampleUnit newUnit = Instantiate(UnitToSpawn);
                newUnit.PlayerNumber = this.GetComponent<Unit>().PlayerNumber;
                newUnit.AddInfoPanel(this.GetComponent<SampleUnit>().GetInfoPanel());

                cellGrid.AddUnit(newUnit.transform, SelectedCell);
                /*Unit tempUnit = null;
                foreach (var cell in inRange)
                {
                    foreach (Unit unit in new List<Unit>(cell.CurrentUnits))
                    {
                        //unit.DefendHandler(UnitReference, Damage);
                        unit.GetComponent<SampleUnit>().HealUnit(3);
                        Debug.Log("heal other unit?");
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
            inRange = null;

        }

        public override void OnCellSelected(Cell cell, CellGrid cellGrid)
        {

            //Debug.Log("cell selected called");
            /*if (cell == null || cell.CurrentUnits.Count > 0 && (cell.CurrentUnits[0] as  SampleUnit).PlayerNumber == 1 || !selected)
            {
                return;
            }
            
            inRange = cellGrid.Cells.FindAll(c => c.GetDistance(cell) <= Range);
            inRange.ForEach(c =>
            {
                c.MarkAsHighlighted();
                if (c.CurrentUnits.Count > 0)
                {
                    c.CurrentUnits[0].GetComponent<SampleUnit>().MarkAsReachableAlly();
                    //c.CurrentUnits[0].MarkAsReachableEnemy();
                }
            });*/

            if (selected)
            {
                if (inRange == null)
                {
                    inRange = cellGrid.Cells.FindAll(c => c.GetDistance(this.GetComponent<SampleUnit>().Cell) <= Range);
                }
            inRange.ForEach(c =>
            {
                c.MarkAsHighlighted();
                if (c.CurrentUnits.Count == 0)
                {
                    //Debug.Log("marking: " + c);
                    c.MarkAsDestination(okLocationColor);
                }
            });
            }
        }
        public override void OnCellDeselected(Cell cell, CellGrid cellGrid)
        {
            if (cell == null || cell.CurrentUnits.Count > 0 && (cell.CurrentUnits[0] as SampleUnit).PlayerNumber == 1 || !selected)
            {
                return;
            }
            if(inRange!=null){
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
                }
        }

        public override void OnUnitHighlighted(Unit unit, CellGrid cellGrid)
        {
            //Debug.Log("unit highlighted called");
            OnCellSelected(unit.Cell, cellGrid);
        }

        public override void OnUnitDehighlighted(Unit unit, CellGrid cellGrid)
        {
            //Debug.Log("unit deselected called");
            OnCellDeselected(unit.Cell, cellGrid);
        }
        public override void OnCellClicked(Cell cell, CellGrid cellGrid)
        {
            /**if (cell == null || cell.CurrentUnits.Count > 0 && (cell.CurrentUnits[0] as SampleUnit).PlayerNumber ==1 || !selected)
            {
                return;
            }*/
            if (cell.CurrentUnits.Count > 0 || cell == null) //make sure cell is valid and empty
            {
                return;
            }

            inRange = cellGrid.Cells.FindAll(c => c.GetDistance(this.GetComponent<SampleUnit>().Cell) <= Range);
            if (inRange.Contains(cell))
            {
                SelectedCell = cell;
            }
            else
            {
                return;
            }


            StartCoroutine(Execute(cellGrid,
                _ => cellGrid.cellGridState = new CellGridStateBlockInput(cellGrid),
                _ => cellGrid.cellGridState = new CellGridStateWaitingForInput(cellGrid)));
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
            OnCellDeselected(null, cellGrid);
            if(inRange != null){
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
            }

        }
        public override IEnumerator Apply(CellGrid cellGrid, IDictionary<string, string> actionParams, bool isNetworkInvoked)
        {
            SelectedCell = cellGrid.Cells.Find(c => c.OffsetCoord.Equals(new UnityEngine.Vector2(float.Parse(actionParams["selected_cell_x"]), float.Parse(actionParams["selected_cell_y"]))));
            yield return StartCoroutine(RemoteExecute(cellGrid));
        }

        public override IDictionary<string, string> Encapsulate()
        {
            var actionParams = new Dictionary<string, string>();

            actionParams.Add("selected_cell_x", SelectedCell.OffsetCoord.x.ToString());
            actionParams.Add("selected_cell_y", SelectedCell.OffsetCoord.y.ToString());

            return actionParams;
        }


        public override string GetAbilityName()
        {
            return "Spawn Drone";
        }

        public override string GetAbilityDescription()
        {
            return "Spend 2HP to create a drone unit";
        }
    }
}
