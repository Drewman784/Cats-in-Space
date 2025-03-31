using System;
using System.Collections.Generic;
using System.Linq;
using TbsFramework.Cells;
using TbsFramework.Units;
using TbsFramework.Units.Abilities;
using UnityEngine;

namespace TbsFramework.Grid.UnitGenerators
{
    public class CustomUnitGenerator : MonoBehaviour
    {
        public Transform UnitsParent;
        public Transform CellsParent;
        [SerializeField] GameObject UnitToSpawn;

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
            Debug.Log("try to spawn");
            // Implement the logic to spawn a unit here  
            // Example:  
            GameObject newUnit = Instantiate(UnitToSpawn);

            isPositive = false;
            if (UnitsParent != null)
            {
                GetComponent<Tile_Script>().CurrentUnits.Add(newUnit.GetComponent<Unit>());
                newUnit.GetComponent<Unit>().Cell = GetComponent<Tile_Script>();
                newUnit.gameObject.transform.position = this.transform.position;
                //newUnit.GetComponent<MoveAbility>().Act()
                //newUnit.transform.parent = UnitsParent.transform;
            }
            else
            {
                Debug.LogError("UnitsParent is not set or is not part of a valid scene.");
            }
            UnitSpawned?.Invoke(this, EventArgs.Empty);
        }
    }
}

