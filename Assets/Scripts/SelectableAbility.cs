using UnityEngine;

namespace TbsFramework.Units.Abilities
{
    public class SelectableAbility : Ability
    {
        private string AbilityName;

        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
        
        }

         // Update is called once per frame
        void Update()
        {
        
        }

        public override bool IsSelectable()
        {
            return true;
        }

        public override string GetAbilityName(){
            //return AbilityName;
            return "Unnamed Ability";
        }
    }
}
