using TbsFramework.Cells;
using UnityEngine;
using UnityEngine.EventSystems;
using TbsFramework.Units;

public class EnvHazardTileCenter : Tile_Script
{
    [SerializeField] private int damageWhenStanding;
    [SerializeField] private int damageWhenWalking;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void CheckAsPathway(Unit unit){
        Debug.Log("check path");
        SampleUnit cUnit = unit as SampleUnit;
        cUnit.TakeEnvironmentalDamage(damageWhenWalking);
    }

    public override void CheckAsTargetPosition(Unit unit){
        Debug.Log("check tile");
        SampleUnit cUnit = unit as SampleUnit;
        cUnit.TakeEnvironmentalDamage(damageWhenStanding);

    }
}
