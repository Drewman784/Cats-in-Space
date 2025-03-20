using System;
using System.Collections.Generic;
using System.Linq;
using TbsFramework.Cells;
using TbsFramework.Units;
using UnityEngine;

namespace TbsFramework.Grid.UnitGenerators
{
    public class CustomUnitGenerator : MonoBehaviour, IUnitGenerator
    {
        public Transform UnitsParent;
        public Transform CellsParent;

        public int TurnCounter;
        //Specifies how many turns it takes to be spawned
        public int AppliesToPlayerNo;
        //Specifies the player unit being spawned
        public bool isPositive;
        //Specifies if the spawner is active

        private int nEndTurns;
        private int nTurn;

        //public event EventHandler UnitSpawned;
        //private Unit SpawnedUnit;

        private void Start()
        {
            ///GetComponent<CellGrid>().TurnEnded += OnTurnEnded;
        }

        private void OnTurnEnded(object sender, bool isNetworkInvoked)
        {
            nEndTurns++;
            var distinctPlayersAlive = (sender as CellGrid).Units.Select(u => u.PlayerNumber)
                                                                 .Distinct()
                                                                 .ToList().Count;
            if (nEndTurns % distinctPlayersAlive == 0)
            {
                nTurn += 1;
            }
        }

        /// <summary>
        /// Returns units that are children of UnitsParent object.
        /// </summary>
        public List<Unit> SpawnUnits(List<Cell> cells)
        {
            List<Unit> ret = new List<Unit>();
            for (int i = 0; i < UnitsParent.childCount; i++)
            {
                var unit = UnitsParent.GetChild(i).GetComponent<Unit>();
                if (unit != null)
                {
                    ret.Add(unit);
                }
                else
                {
                    Debug.LogError("Invalid object in Units Parent game object");
                }
            }
            return ret;
        }

        public void SnapToGrid()
        {

        }
    }
}
