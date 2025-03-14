using System.Collections.Generic;
using TbsFramework.Grid;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace TbsFramework.Units.Abilities
{
    public class SelectAbility : Ability
    {
        public List<Button> SelectButtons;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Start()
        {
            GameObject g = GameObject.Find("AbilityButtons");
            SelectButtons.Add(g.transform.GetChild(0).GetComponent<Button>());
            SelectButtons.Add(g.transform.GetChild(1).GetComponent<Button>());
            SelectButtons.Add(g.transform.GetChild(2).GetComponent<Button>());

            /*Unit cG = gameObject.GetComponent<Unit>();
            Debug.Log("unit has "+cG.Abilities.Count + "abilities: ");
            foreach(Ability a in cG.Abilities){
                if(a.IsSelectable()){
                    SelectAbilities.Add(a);
                }
                Debug.Log(a);
            }*/
        }

        public override void OnUnitClicked(Unit unit, CellGrid cellGrid)
        {
            DisplayAbilities();

            base.OnUnitClicked(unit, cellGrid);
        }

        void OnMouseEnter()
        {
          DisplayAbilities();   
        }

        public void DisplayAbilities(){

            List<Ability> SelectAbilities = new List<Ability>();

            Unit cG = GetComponent<Unit>();
            Debug.Log("unit has "+cG.Abilities.Count + "abilities: ");
            foreach(Ability a in cG.Abilities){
                if(a.IsSelectable()){
                    SelectAbilities.Add(a);
                }
                //Debug.Log(a);
            }

            int check = 0;
            Debug.Log("select abilities count: " + SelectAbilities.Count);
            for(int a = 0; a < SelectAbilities.Count; a++){
                SelectButtons[a].gameObject.SetActive(true);
                SelectButtons[a].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = SelectAbilities[a].GetAbilityName();
                SelectButtons[a].gameObject.GetComponent<AbilityButtonScript>().SetAbility(SelectAbilities[a], cG);
                check++;
            }

            if(SelectAbilities.Count < 3){
                while(check<3){
                    //Debug.Log("inactive "+check);
                    SelectButtons[check].gameObject.SetActive(false);
                    check++;
                }
            }  
        }
    }
}
