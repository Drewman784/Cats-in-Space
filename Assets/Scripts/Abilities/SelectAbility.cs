using System.Collections.Generic;
using TbsFramework.Grid;
using TMPro;
using UnityEditor;
using UnityEditor.PackageManager.UI;
using UnityEditor.Rendering;
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
            //Debug.Log("unit has "+cG.Abilities.Count + "abilities: ");
            foreach(Ability a in cG.Abilities){
                if(a.IsSelectable()){
                    SelectAbilities.Add(a);
                }
                //Debug.Log(a);
            }

            int check = 0;
            //Debug.Log("select abilities count: " + SelectAbilities.Count);
            for(int a = 0; a < SelectAbilities.Count; a++){
                SelectButtons[a].gameObject.SetActive(true);
                SelectButtons[a].transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = SelectAbilities[a].GetAbilityName();
                SelectButtons[a].gameObject.GetComponent<AbilityButtonScript>().SetAbility(SelectAbilities[a], cG);
                check++;
                if(GetComponent<SampleUnit>().ActionPoints<1){
                    SelectButtons[a].GetComponent<Image>().color = Color.grey;
                } else{
                    SelectButtons[a].GetComponent<Image>().color = Color.white;
                }
            }

            if(SelectAbilities.Count < 3){
                while(check<3){
                    //Debug.Log("inactive "+check);
                    SelectButtons[check].gameObject.SetActive(false);
                    check++;
                }
            }  
        }

        public override bool CanPerform(CellGrid cellGrid)
        {
            if(UnitReference.ActionPoints>0){
                return true;
            } else{
                return false;
            }
        }


    }
}
