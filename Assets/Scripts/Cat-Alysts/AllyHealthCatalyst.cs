using System.Collections.Generic;
using TbsFramework.Cells;
using TbsFramework.Grid;
using UnityEngine;

public class AllyHealthCatalyst : BaseCatalyst
{
    [SerializeField] int threshold;
    private bool hasBeenUsed;
    [SerializeField] int radius;
    [SerializeField] int amountHealed;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        hasBeenUsed = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool CheckCatalyst()
    {
        if(!hasBeenUsed && GetComponent<SampleUnit>().HitPoints>0){
            //Debug.Log("evaluation?" + catMon);
            base.CheckCatalyst();
            if(this.catMon.AllUnitsSideHealthBelow(GetComponent<SampleUnit>().PlayerNumber, threshold)){
                if(GetComponent<SampleUnit>().PlayerNumber == 0){
                    //Debug.Log("yeah");
                    this.CheckActivation();
                } else{
                    //Debug.Log("nah");
                    TriggerCatalystEffect();
                }
                return true;
            }
        }
        return false;
    }

    public override void TriggerCatalystEffect()
    {
        hasBeenUsed = true;
        List<Cell> inRange;
        inRange = GameObject.Find("CellGrid").GetComponent<CellGrid>().Cells.FindAll(c => c.GetDistance(GetComponent<SampleUnit>().Cell) <= radius);
            inRange.ForEach(c =>
            {
                c.MarkAsHighlighted();
                if (c.CurrentUnits.Count > 0 && c.CurrentUnits[0].PlayerNumber == GetComponent<SampleUnit>().PlayerNumber)
                {
                    c.CurrentUnits[0].GetComponent<SampleUnit>().HealUnit(amountHealed);
                    //c.CurrentUnits[0].MarkAsReachableEnemy();
                }
            });
        //base.TriggerCatalystEffect();
    }
}
