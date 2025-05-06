using UnityEngine;
using TbsFramework.Grid;
using System.Collections;
using System.Collections.Generic;
//using UnityEditor.PackageManager.UI;

namespace TbsFramework.Units.Abilities
{
    //this ability should heal the unit performing the action
    public class HealSelfAbility : SelectableAbility
    {
     public override IEnumerator Act(CellGrid cellGrid, bool isNetworkInvoked = false)
        {
            if (CanPerform(cellGrid) )
            {
                Debug.Log("HEAL");
                GetComponent<SampleUnit>().HealUnit(4);
                yield return new WaitForSeconds(0.5f);
            } else{
                Debug.LogError("heal failed!");
            }
            yield return null;
        }

        public override string GetAbilityName()
        {
            return "Self Heal";
        }

        public override string GetAbilityDescription()
        {
            return "Heal self for 4 HP";
        }

        public override IEnumerator Apply(CellGrid cellGrid, IDictionary<string, string> actionParams, bool isNetworkInvoked)
        {
            yield return StartCoroutine(RemoteExecute(cellGrid));
        }

        public override void OnAbilitySelected(CellGrid cellGrid) { 
            //DoAction(cellGrid);
        }

        public override bool IsInstant()
        {
            return true;
        }  
}
}
