using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using TbsFramework.Cells;
using TbsFramework.Grid;
using TbsFramework.Grid.GridStates;
using UnityEngine;
using TbsFramework.Units;
using System.IO;


namespace TbsFramework.Units.Abilities
{
    public class MoveAbility : Ability
    {
        public Cell Destination { get; set; }
        private IList<Cell> currentPath;
        public HashSet<Cell> availableDestinations;

        public override IEnumerator Act(CellGrid cellGrid, bool isNetworkInvoked = false)
        {
            if (UnitReference.ActionPoints > 0 && availableDestinations.Contains(Destination))
            {
                var path = UnitReference.FindPath(cellGrid.Cells, Destination);
                yield return UnitReference.Move(Destination, path);

                //Debug.Log("here?");
                foreach(var cell in path){//CAL EDIT
                    Square c = (Square)cell;
                    //Debug.Log("list going: " + c);
                    c.CheckAsPathway(this.GetComponent<Unit>());
                }
                Square d = (Square) Destination;
                d.CheckAsTargetPosition(GetComponent<Unit>());
            }
            yield return base.Act(cellGrid, isNetworkInvoked);
        }
        public override void Display(CellGrid cellGrid)
        {
            if (UnitReference.ActionPoints > 0)
            {
                foreach (var cell in availableDestinations)
                {
                    cell.MarkAsReachable();
                }
                if(lastClickedCell!=null){
                    IList<Cell> tempPath = UnitReference.FindPath(cellGrid.Cells, lastClickedCell);
                    foreach (var c in tempPath)
                    {
                        c.MarkAsPath();
                    }
                }
                //foreach (var cell in currentPath)
            }
        }

        private Cell lastClickedCell;

        public override void OnUnitClicked(Unit unit, CellGrid cellGrid)
        {
            if (cellGrid.GetCurrentPlayerUnits().Contains(unit))
            {
            cellGrid.cellGridState = new CellGridStateAbilitySelected(cellGrid, unit, unit.GetComponents<Ability>().ToList());
            }
        }

        public override void OnCellClicked(Cell cell, CellGrid cellGrid)
        {
            //Debug.Log("clicked: " + cell);
            if (availableDestinations.Contains(cell))
            {
                if (lastClickedCell == cell) //confirm destination
                {
                    Destination = cell;
                    currentPath = null;
                    lastClickedCell = null; // Reset after double-click
                    StartCoroutine(HumanExecute(cellGrid));
                }
                else //change destination
                {
                    if (lastClickedCell != null)
                    {
                        lastClickedCell.UnMark(); // Unmark the previously clicked cell
                    }

                    lastClickedCell = cell;
                    lastClickedCell.MarkAsHighlighted(); // Highlight the newly clicked cell

                    //Keep unit from deselecting
                    GetComponent<Unit>().OnMouseDown();
                    GetComponent<Unit>().MarkAsSelected();
                    GetComponent<Unit>().OnUnitSelected();
                    OnAbilitySelected(cellGrid);
                    Display(cellGrid);
                }
            }
            else //not available destination
            {
                if (lastClickedCell != null)
                {
                    lastClickedCell.UnMark(); // Unmark the previously clicked cell if an invalid cell is clicked
                }

                lastClickedCell = null; // Reset if clicked on an invalid cell
            }
        }
        public override void OnCellSelected(Cell cell, CellGrid cellGrid)
        {
            //Debug.Log("cell selected: "+ cell);
            if (UnitReference.ActionPoints > 0 && availableDestinations.Contains(cell))
            {
                currentPath = UnitReference.FindPath(cellGrid.Cells, cell);
                foreach (var c in currentPath)
                {
                    c.MarkAsPath();
                }
            }
        }
        public override void OnCellDeselected(Cell cell, CellGrid cellGrid)
        {
            //Debug.Log("cell deselected: "+ cell);
            if (UnitReference.ActionPoints > 0 && availableDestinations.Contains(cell))
            {
                if(currentPath == null)
                {
                    return;
                }
                foreach(var c in currentPath)
                {
                    c.MarkAsReachable();
                }
            }
        }

        public override void OnAbilitySelected(CellGrid cellGrid)
        {
            UnitReference.CachePaths(cellGrid.Cells);
            availableDestinations = UnitReference.GetAvailableDestinations(cellGrid.Cells);
        }

        public override void CleanUp(CellGrid cellGrid)
        {
           // Debug.Log("cleanup");
            foreach (var cell in availableDestinations)
            {
                cell.UnMark();
            }
        }

        public override bool CanPerform(CellGrid cellGrid)
        {
            return UnitReference.ActionPoints > 0 && UnitReference.GetAvailableDestinations(cellGrid.Cells).Count > 0;
        }

        public override IDictionary<string, string> Encapsulate()
        {
            var actionParams = new Dictionary<string, string>();

            actionParams.Add("destination_x", Destination.OffsetCoord.x.ToString());
            actionParams.Add("destination_y", Destination.OffsetCoord.y.ToString());

            return actionParams;
        }

        public override IEnumerator Apply(CellGrid cellGrid, IDictionary<string, string> actionParams, bool isNetworkInvoked = false)
        {
            var actionDestination = cellGrid.Cells.Find(c => c.OffsetCoord.Equals(new UnityEngine.Vector2(float.Parse(actionParams["destination_x"]), float.Parse(actionParams["destination_y"]))));
            Destination = actionDestination;
            //gameObject.transform.GetChild(1).GetComponent<Animator>().SetBool("Walking", true); //CAL EDIT
            yield return StartCoroutine(RemoteExecute(cellGrid));
            //gameObject.transform.GetChild(1).GetComponent<Animator>().SetBool("Walking", false); //CAL EDIT
        }
    }
}
