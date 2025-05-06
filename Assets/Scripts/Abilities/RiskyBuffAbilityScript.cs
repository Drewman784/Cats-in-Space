using UnityEngine;
using TbsFramework.Grid;
using System.Collections;
//using UnityEditor.UI;
using System.Collections.Generic;

namespace TbsFramework.Units.Abilities
{
    // this ability should buff a unit for 2 turns and then debuff them for 2 turns
    public class RiskyBuffAbilityScript : SelectableAbility
    {
        private int turnsPassed;
        private bool inUse;
        [SerializeField] Buff buff;
        [SerializeField] Buff deBuff;

         // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            //turnsPassed = 0;
            inUse = false;
        }

        public override IEnumerator Act(CellGrid cellGrid, bool isNetworkInvoked = false)
        {
            if (CanPerform(cellGrid) )
            {
                Debug.Log("SelfBuff!");
                GetComponent<SampleUnit>().AddBuff(buff);
                GetComponent<SampleUnit>().AddBuff(deBuff);
                inUse = true;
                yield return new WaitForSeconds(0.5f);
            } else{
                Debug.LogError("buff failed!");
            }
            yield return null;
        }

        public override string GetAbilityName()
        {
            return "Risky Buff";
        }

        public override bool IsSelectable() //return true when the buff isn't in use
        {
            if(inUse){
                return false;
            }else{
                return true;
            }
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

        public override string GetAbilityDescription()
        {
            return "Buff Attack by 3 for 2 turns, then debuff by 2 for 2 turns";
        }

        
    }
}
