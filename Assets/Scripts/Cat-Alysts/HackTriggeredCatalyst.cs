using TbsFramework.Cells;
using TbsFramework.Grid;
using UnityEngine;

public class HackTriggeredCatalyst : BaseCatalyst
{
    //this catalyst should replace the current unit with the equivalent enemy/player unit by spawning in a new one
    //afterwards, the hacking ability should destroy the original unit

    public SampleUnit equivalentUnitToSpawn;

    public override void TriggerCatalystEffect()
    {
        CellGrid cellGrid = GetComponent<SampleUnit>().GetCellGrid();
        Cell currentCell = GetComponent<SampleUnit>().Cell;
        SampleUnit newUnit = Instantiate(equivalentUnitToSpawn); //inst new unit

        if (GetComponent<SampleUnit>().PlayerNumber == 0) //set which side based on original unit
        {
            newUnit.PlayerNumber = 1;
        }
        else
        {
            newUnit.PlayerNumber = 0;
        }

        newUnit.AddInfoPanel(this.GetComponent<SampleUnit>().GetInfoPanel());
        cellGrid.AddUnit(newUnit.transform, currentCell);
        //this.GetComponent<SampleUnit>().SetHacked();

        newUnit.HitPoints = GetComponent<SampleUnit>().HitPoints;
    }
}
