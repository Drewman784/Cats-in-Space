using TbsFramework.Units;
using UnityEngine;

public class DamageTakenCatalyst : BaseCatalyst
{
    [SerializeField] int threshold;
    [SerializeField] Buff buffToApply;

    private bool hasBeenUsed;
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
        if(!hasBeenUsed){
            //Debug.Log("evaluation?" + catMon);
            base.CheckCatalyst();
            if(this.catMon.UnitTotalDamageHasReached(this.GetComponent<SampleUnit>(), threshold)){
                //Debug.Log("mmhmm");
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
        //base.CheckCatalyst();
    }

    public override void TriggerCatalystEffect()
    {
        //Debug.Log("yay effect triggered!");
        hasBeenUsed = true;
        GetComponent<SampleUnit>().AddBuff(buffToApply);
        //base.TriggerCatalystEffect();
    }
}
