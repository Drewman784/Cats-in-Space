using System.Collections.Generic;
using TbsFramework.Grid;
using UnityEngine;

public class CatalystMonitor : MonoBehaviour
{
    //this script should keep track of stats required for Cat-alyst actions and report their availability
    private int totalFatalities;
    private int allyFatalities;
    private int enemyFatalities;
    private int turnCount;

    private int totalDamage;
    private int enemyDamage;
    private int allyDamage;

    private GameObject ActivationWindow;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        totalDamage = 0;
        enemyDamage = 0;
        allyDamage = 0;

        totalFatalities = 0;
        allyFatalities = 0;
        enemyFatalities = 0;

        turnCount = 0;

        ActivationWindow = GameObject.Find("CatalystActivationScreen");
        ActivationWindow.SetActive(false);
    }

    public GameObject GetActivationWindow(){
        return ActivationWindow;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void RegisterKill(int whichPlayer){
        if(whichPlayer == 0){
            allyFatalities++;
        } else{
            enemyFatalities++;
        }
        totalFatalities++;
        //Debug.Log("kill registered");
    }

    public void RegisterTurnCount(){
        turnCount++;
    }

    public void RegisterDamage(int dmg, int whichPlayer, SampleUnit attacker, SampleUnit defender){
        //Debug.Log("got to register damage");
        if(whichPlayer == 0){
            allyDamage+=dmg;
        } else{
            enemyDamage+=dmg;
        }
        totalDamage+=dmg;

        attacker.lastTarget = defender;
        defender.totalDamageTaken += dmg;

        Debug.Log(defender +" : "+defender.totalDamageTaken + "<-totaldmg");

       //Debug.Log("damage registered: " + dmg);
    }

    public bool AllAttackingSameUnit(int whichPlayer){ //check if all units of a side are attacking the same target
        bool samePlayerTargeted = true;
        List<SampleUnit> theTargets = new List<SampleUnit>();
        foreach(SampleUnit sU in GetComponent<CellGrid>().Units){
            if(sU.PlayerNumber == whichPlayer){
                theTargets.Add(sU.lastTarget);
                /*if(thebase == null){ //for first loop, set thebase as the last target of current unit
                    thebase = sU.lastTarget;
                } else{
                    if(thebase != sU.lastTarget){ //comparisons after first loop
                        samePlayerTargeted = false;
                    }
                }*/
            }
        }

        SampleUnit thebase = theTargets[0];
        foreach(SampleUnit t in theTargets){
            //Debug.Log(t);
            if(t!= thebase || t==null){ //false if null or mismatched
                samePlayerTargeted = false;
            } 
        }
        //Debug.Log(samePlayerTargeted);
        return samePlayerTargeted;
    }

    public bool UnitTotalDamageHasReached(SampleUnit which, int threshold){ // return if unit has reached a total damage amount
        //Debug.Log("evaluating unit" + which + "total damage " + which.totalDamageTaken +" >= " + threshold);
        return which.totalDamageTaken >= threshold;
    }
    
    public bool SideUnitsTotalDamageHasReached(int whichPlayer, int threshold){ //return if one side has cumilatively reached a total damage amount
        if(whichPlayer ==0){
            return allyDamage >= threshold;
        } else{
            return enemyDamage >= threshold;
        }
    }

    public bool TotalDamageHasReached(int threshold){ //return if total damage has cumitively reached threshold
        return totalDamage >= threshold;
    }

    public bool PlayerKillsReached(int threshold){ //return if player has killed given number of enemies
        return enemyFatalities >=threshold;
    }

    public bool EnemyKillsReached(int threshold){ // return if enemy has killed given number of allies
        return allyFatalities >= threshold;
    }

    public bool AllUnitsSideHealthBelow(int whichPlayer, int threshold){ //check if all units on side have health below threshold
        bool healthIsBelow = true;
        foreach(SampleUnit sU in GetComponent<CellGrid>().Units){
            if(sU.PlayerNumber == whichPlayer){
                if(sU.HitPoints>threshold){
                    healthIsBelow = false;
                }
            }
        }
        return healthIsBelow;
    }
    


}
