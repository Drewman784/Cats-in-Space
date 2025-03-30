using System;
using System.Collections.Generic;
using System.Linq;
using TbsFramework.Cells;
using TbsFramework.Units;
using UnityEngine;

namespace TbsFramework.Grid.UnitGenerators
{
    public class CustomUnitGenerator : MonoBehaviour
    {
        public GameObject UnitsParent;
        public GameObject CellsParent;

        public event EventHandler UnitSpawned;

        public int TurnCounter;
        //Specifies how many turns it takes to be spawned
        public int AppliesToPlayerNo;
        //Specifies the player unit being spawned
        public bool isPositive;
        //Specifies if the spawner is active

        private int nEndTurns;
        private int nTurn;

        private void Start()
        {
            nTurn = 0;
        }

        private void Update()
        {
            if (isPositive)
            {
                nTurn++;
                if (nTurn % 2 == 0)
                {
                    SpawnUnit();
                }
            }
        }

        private void SpawnUnit()
        {
            // Implement the logic to spawn a unit here
            // Example:
            GameObject newUnit = new GameObject("Unit");
            if (UnitsParent != null && UnitsParent.scene.IsValid())
            {
                newUnit.transform.parent = UnitsParent.transform;
            }
            else
            {
                Debug.LogError("UnitsParent is not set or is not part of a valid scene.");
            }
            UnitSpawned?.Invoke(this, EventArgs.Empty);
        }
    }
}

