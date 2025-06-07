using System.Collections.Generic;
using TbsFramework.Cells;
using TbsFramework.Grid;
using TMPro;
using UnityEditor;
//using UnityEditor.PackageManager.UI;
//using UnityEditor.Rendering;
using UnityEngine;
using UnityEngine.UI;

namespace TbsFramework.Units.Abilities
{
    public class SelectAbility : Ability
    {
        public List<Button> SelectButtons;
        public List<SelectableAbility> SelectAbilities;
        public UnitInfoPanelScript selectPanel;
        // Start is called once before the first execution of Update after the MonoBehaviour is created

        private bool firstclick;

        void Start()
        {
            firstclick = true;
            /*firstclick = false;
            selectPanel = gameObject.GetComponent<SampleUnit>().GetSelectionPanel().GetComponent<UnitInfoPanelScript>();
            //selectPanel = GameObject.Find("UnitSelected").GetComponent<UnitInfoPanelScript>();
            // GameObject g = GameObject.Find("AbilityButtons");
            GameObject g = selectPanel.gameObject.transform.GetChild(1).gameObject;
            SelectButtons.Add(g.transform.GetChild(0).GetComponent<Button>());
            SelectButtons.Add(g.transform.GetChild(1).GetComponent<Button>());
            SelectButtons.Add(g.transform.GetChild(2).GetComponent<Button>()); */

            /*Unit cG = gameObject.GetComponent<Unit>();
            Debug.Log("unit has "+cG.Abilities.Count + "abilities: ");
            foreach(Ability a in cG.Abilities){
                if(a.IsSelectable()){
                    SelectAbilities.Add(a);
                }
                Debug.Log(a);
            }*/
            if (SelectButtons.Count != 3)
            {
                Debug.Log("Missing button!");
            }
            else
            {
                // Debug.Log("got all buttons");
            }
        }

        public override void OnUnitClicked(Unit unit, CellGrid cellGrid)
        {
            //DisplayAbilities();

            base.OnUnitClicked(unit, cellGrid);
        }

        public override void OnCellClicked(Cell cell, CellGrid cellGrid)
        {
            //DisplayAbilities();
            base.OnCellClicked(cell, cellGrid);
        }

        void OnMouseEnter()
        {
            if (!gameObject.GetComponent<SampleUnit>().DialogueOngoing)
            {
                if (firstclick) //import necessary variables first time
                {
                    selectPanel = gameObject.GetComponent<SampleUnit>().GetSelectionPanel().GetComponent<UnitInfoPanelScript>();
                    GameObject g = selectPanel.gameObject.transform.GetChild(1).gameObject;
                    SelectButtons.Add(g.transform.GetChild(0).GetComponent<Button>());
                    SelectButtons.Add(g.transform.GetChild(1).GetComponent<Button>());
                    SelectButtons.Add(g.transform.GetChild(2).GetComponent<Button>());

                    firstclick = false;
                }
                Debug.Log(selectPanel);
                if (selectPanel.isSelected == false)
                {
                    DisplayAbilities();
                }
            }  
        }

        public void DisplayAbilities(){
            //Debug.Log(GetComponent<SampleUnit>().UnitName + " displaying?");

            SelectAbilities = new List<SelectableAbility>();

            Unit cG = GetComponent<SampleUnit>();
            foreach(Ability a in cG.Abilities){
                if(a.IsSelectable()){
                   SelectAbilities.Add((SelectableAbility)a);
                   //a.enabled = false;
                }
                //Debug.Log(a);
            }
            //Debug.Log(SelectAbilities);

            //Unit cG = GetComponent<Unit>();
            //Debug.Log("unit has "+cG.Abilities.Count + "abilities: ");
            /*foreach(SelectableAbility a in SelectAbilities){
                    a.selected = false;
                //Debug.Log(a);
            }*/

            int check = 0;
            //Debug.Log("select abilities count: " + SelectAbilities.Count);
            //Debug.Log("list: "+ SelectAbilities.Count);
                for(int a = 0; a < SelectAbilities.Count; a++){
                   // Debug.Log(a + " ability name = " + SelectAbilities[a].GetAbilityName());
                SelectButtons[a].gameObject.SetActive(true);
                SelectButtons[a].transform.GetChild(0).GetComponent<Text>().text = SelectAbilities[a].GetAbilityName();
                SelectButtons[a].gameObject.GetComponent<AbilityButtonScript>().SetAbility((SelectableAbility)SelectAbilities[a], cG);
                check++;
                if(GetComponent<SampleUnit>().ActionPoints<1){
                    SelectButtons[a].GetComponent<Image>().color = Color.grey;
                } else{
                    SelectButtons[a].GetComponent<Image>().color = Color.white;
                }
            }

            //Debug.Log("sab = " + SelectAbilities.Count + " butttons = " + SelectButtons.Count);
            if(SelectAbilities.Count < 3){
                while(check<3){
                    //Debug.Log("inactive "+check);
                    SelectButtons[check].gameObject.SetActive(false);
                    check++;
                }
            }

            /*foreach(SelectableAbility a in SelectAbilities){
                    a.enabled = false;
                //Debug.Log(a);
            }*/  
        }

        public override bool CanPerform(CellGrid cellGrid)
        {
            if(UnitReference.ActionPoints>0){
                return true;
            } else{
                return false;
            }
        }

        //public override onUnitSelected


    }
}
