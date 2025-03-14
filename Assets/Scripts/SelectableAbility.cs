using UnityEngine;
using TbsFramework.Cells;
using TbsFramework.Grid;
using TbsFramework.Grid.GridStates;

namespace TbsFramework.Units.Abilities
{
    public class SelectableAbility : Ability
    {
        private string AbilityName;

        public override bool IsSelectable()
        {
            return true;
        }

        public override string GetAbilityName(){
            //return AbilityName;
            return "Unnamed Ability";
        }

        public override bool CanPerform(CellGrid cellGrid)
        {
            if (UnitReference.ActionPoints <= 0)
            {
                return false;
            }
            return true;
        }

        public override void DoAction(CellGrid cellGrid){
            Debug.Log("CALLED");
            StartCoroutine(HumanExecute(cellGrid));
            ActionCost();
        }

        public virtual void ActionCost(){
            int actionCost = 1;
            GetComponent<SampleUnit>().ActionPoints -= actionCost;
        }
    }
}
